# Rate Limiting Test Script
# PowerShell ile rate limiting testi

Write-Host "=== ECommerce Rate Limiting Test ===" -ForegroundColor Green
Write-Host ""

# SSL sertifika doğrulamasını devre dışı bırak
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

$baseUrl = "https://localhost:2021"
$testResults = @()

# Test 1: Health Check
Write-Host "1. Health Check Test..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/health" -Method GET
    Write-Host "   Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   Response Time: $($response.Headers.'X-Response-Time')" -ForegroundColor Green
    
    # Security headers kontrolü
    Write-Host "   Security Headers:" -ForegroundColor Cyan
    Write-Host "   - X-Content-Type-Options: $($response.Headers.'X-Content-Type-Options')" -ForegroundColor Cyan
    Write-Host "   - X-Frame-Options: $($response.Headers.'X-Frame-Options')" -ForegroundColor Cyan
    Write-Host "   - X-XSS-Protection: $($response.Headers.'X-XSS-Protection')" -ForegroundColor Cyan
    Write-Host "   - Content-Security-Policy: $($response.Headers.'Content-Security-Policy')" -ForegroundColor Cyan
    
    $testResults += "Health Check: PASS"
} catch {
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "Health Check: FAIL"
}

Write-Host ""

# Test 2: Rate Limiting Test (10 istek)
Write-Host "2. Rate Limiting Test (10 requests)..." -ForegroundColor Yellow
$rateLimitResults = @()

for ($i = 1; $i -le 10; $i++) {
    try {
        $response = Invoke-WebRequest -Uri "$baseUrl/health" -Method GET
        $status = $response.StatusCode
        
        if ($status -eq 429) {
            Write-Host "   Request $i`: Rate Limited (429)" -ForegroundColor Red
            $rateLimitResults += "Request $i`: 429"
        } else {
            Write-Host "   Request $i`: Success ($status)" -ForegroundColor Green
            $rateLimitResults += "Request $i`: $status"
        }
        
        # Rate limit headers kontrolü
        if ($response.Headers.'X-RateLimit-Limit') {
            Write-Host "     Rate Limit: $($response.Headers.'X-RateLimit-Limit')" -ForegroundColor Cyan
            Write-Host "     Remaining: $($response.Headers.'X-RateLimit-Remaining')" -ForegroundColor Cyan
        }
        
        Start-Sleep -Milliseconds 100
    } catch {
        Write-Host "   Request $i`: Error - $($_.Exception.Message)" -ForegroundColor Red
        $rateLimitResults += "Request $i`: ERROR"
    }
}

Write-Host ""

# Test 3: Auth Login Rate Limiting
Write-Host "3. Auth Login Rate Limiting Test..." -ForegroundColor Yellow
$authResults = @()

for ($i = 1; $i -le 6; $i++) {
    try {
        $body = @{
            email = "test@example.com"
            password = "WrongPassword123!"
        } | ConvertTo-Json
        
        $response = Invoke-WebRequest -Uri "$baseUrl/api/auth/login" -Method POST -Body $body -ContentType "application/json"
        $status = $response.StatusCode
        
        if ($status -eq 429) {
            Write-Host "   Auth Request $i`: Rate Limited (429)" -ForegroundColor Red
            $authResults += "Auth Request $i`: 429"
        } elseif ($status -eq 401) {
            Write-Host "   Auth Request $i`: Unauthorized (401) - Expected" -ForegroundColor Yellow
            $authResults += "Auth Request $i`: 401"
        } else {
            Write-Host "   Auth Request $i`: $status" -ForegroundColor Green
            $authResults += "Auth Request $i`: $status"
        }
        
        Start-Sleep -Milliseconds 200
    } catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        if ($statusCode -eq 429) {
            Write-Host "   Auth Request $i`: Rate Limited (429)" -ForegroundColor Red
            $authResults += "Auth Request $i`: 429"
        } elseif ($statusCode -eq 401) {
            Write-Host "   Auth Request $i`: Unauthorized (401) - Expected" -ForegroundColor Yellow
            $authResults += "Auth Request $i`: 401"
        } else {
            Write-Host "   Auth Request $i`: Error - $($_.Exception.Message)" -ForegroundColor Red
            $authResults += "Auth Request $i`: ERROR"
        }
    }
}

Write-Host ""

# Test 4: Input Validation Test
Write-Host "4. Input Validation Test..." -ForegroundColor Yellow
try {
    $body = @{
        email = "test@example.com"
        password = "<script>alert('xss')</script>Password123!"
    } | ConvertTo-Json
    
    $response = Invoke-WebRequest -Uri "$baseUrl/api/auth/login" -Method POST -Body $body -ContentType "application/json"
    Write-Host "   Status: $($response.StatusCode)" -ForegroundColor Green
    $testResults += "Input Validation: PASS"
} catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    if ($statusCode -eq 400) {
        Write-Host "   XSS Content Blocked (400) - Expected" -ForegroundColor Green
        $testResults += "Input Validation: PASS (XSS Blocked)"
    } else {
        Write-Host "   Status: $statusCode" -ForegroundColor Yellow
        $testResults += "Input Validation: $statusCode"
    }
}

Write-Host ""

# Sonuçları özetle
Write-Host "=== TEST RESULTS SUMMARY ===" -ForegroundColor Green
Write-Host ""
foreach ($result in $testResults) {
    Write-Host "✓ $result" -ForegroundColor Green
}

Write-Host ""
Write-Host "Rate Limiting Results:" -ForegroundColor Cyan
foreach ($result in $rateLimitResults) {
    if ($result -like "*429*") {
        Write-Host "🚫 $result" -ForegroundColor Red
    } else {
        Write-Host "✅ $result" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "Auth Rate Limiting Results:" -ForegroundColor Cyan
foreach ($result in $authResults) {
    if ($result -like "*429*") {
        Write-Host "🚫 $result" -ForegroundColor Red
    } elseif ($result -like "*401*") {
        Write-Host "⚠️  $result" -ForegroundColor Yellow
    } else {
        Write-Host "✅ $result" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "=== Test Completed ===" -ForegroundColor Green
