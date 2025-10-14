# VS Code Postman Extension Kurulum Rehberi

## 1. Collection Oluşturma

### Adım 1: Yeni Collection

1. VS Code'da Postman panelini açın
2. "New Collection" butonuna tıklayın
3. Collection adı: "ECommerce Rate Limiting Tests"

### Adım 2: Request'leri Ekleme

#### Request 1: Health Check

- **Method**: GET
- **URL**: `{{base_url}}/health`
- **Name**: "1. Health Check (Normal)"

#### Request 2: Rate Limit Test

- **Method**: GET
- **URL**: `{{base_url}}/health`
- **Name**: "2. Rate Limit Test - Health Endpoint"

#### Request 3: Auth Login Test

- **Method**: POST
- **URL**: `{{base_url}}/api/auth/login`
- **Headers**:
  - Content-Type: application/json
- **Body** (raw JSON):

```json
{
  "email": "test@example.com",
  "password": "WrongPassword123!"
}
```

- **Name**: "3. Auth Login Rate Limit Test"

#### Request 4: Security Headers Test

- **Method**: GET
- **URL**: `{{base_url}}/health`
- **Name**: "4. Security Headers Test"

#### Request 5: Input Validation Test

- **Method**: POST
- **URL**: `{{base_url}}/api/auth/login`
- **Headers**:
  - Content-Type: application/json
- **Body** (raw JSON):

```json
{
  "email": "test@example.com",
  "password": "<script>alert('xss')</script>Password123!"
}
```

- **Name**: "5. Input Validation Test"

## 2. Environment Oluşturma

### Adım 1: Yeni Environment

1. Postman panelinde "Environments" sekmesine gidin
2. "New Environment" butonuna tıklayın
3. Environment adı: "ECommerce Local Environment"

### Adım 2: Variables Ekleme

- **base_url**: `https://localhost:2021`
- **api_key**: `{{$randomUUID}}`
- **user_id**: `{{$randomUUID}}`
- **auth_token**: (boş bırakın)
- **category_id**: `00000000-0000-0000-0000-000000000001`

## 3. Test Scripts Ekleme

Her request için "Tests" sekmesinde aşağıdaki script'leri ekleyin:

### Health Check Test Script:

```javascript
pm.test("Status code is 200", function () {
  pm.response.to.have.status(200);
});

pm.test("Response time is less than 1000ms", function () {
  pm.expect(pm.response.responseTime).to.be.below(1000);
});

console.log(
  "Request #" + (pm.info.iteration + 1) + " - Status: " + pm.response.status
);
```

### Rate Limit Test Script:

```javascript
if (pm.response.status === 429) {
  pm.test("Rate limit exceeded (429)", function () {
    pm.response.to.have.status(429);
  });
  console.log("Rate limit exceeded! Request #" + (pm.info.iteration + 1));
} else {
  pm.test("Request successful (not rate limited)", function () {
    pm.response.to.have.status(200);
  });
  console.log(
    "Request #" + (pm.info.iteration + 1) + " - Status: " + pm.response.status
  );
}
```

### Security Headers Test Script:

```javascript
pm.test("X-Content-Type-Options header present", function () {
  pm.expect(pm.response.headers.get("X-Content-Type-Options")).to.eql(
    "nosniff"
  );
});

pm.test("X-Frame-Options header present", function () {
  pm.expect(pm.response.headers.get("X-Frame-Options")).to.eql("DENY");
});

pm.test("X-XSS-Protection header present", function () {
  pm.expect(pm.response.headers.get("X-XSS-Protection")).to.eql(
    "1; mode=block"
  );
});

pm.test("Content-Security-Policy header present", function () {
  pm.expect(pm.response.headers.get("Content-Security-Policy")).to.exist;
});

pm.test("Referrer-Policy header present", function () {
  pm.expect(pm.response.headers.get("Referrer-Policy")).to.exist;
});

console.log("Security headers test completed");
```

## 4. Test Çalıştırma

### Tekil Test:

1. Request'i seçin
2. "Send" butonuna tıklayın
3. "Test Results" sekmesinde sonuçları görün

### Collection Runner:

1. Collection'a sağ tıklayın
2. "Run Collection" seçin
3. Iterations: 10
4. Delay: 100ms
5. Environment: ECommerce Local Environment
6. "Run" butonuna tıklayın
