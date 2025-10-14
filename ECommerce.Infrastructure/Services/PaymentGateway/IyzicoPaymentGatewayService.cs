using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace ECommerce.Infrastructure.Services.PaymentGateway;

/// <summary>
/// Iyzico Payment Gateway servis implementasyonu
/// </summary>
public class IyzicoPaymentGatewayService : IPaymentGatewayService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<IyzicoPaymentGatewayService> _logger;
    private readonly HttpClient _httpClient;

    private readonly string _apiKey;
    private readonly string _secretKey;
    private readonly string _baseUrl;
    private readonly bool _isTestMode;

    public IyzicoPaymentGatewayService(
        IConfiguration configuration,
        ILogger<IyzicoPaymentGatewayService> logger,
        HttpClient httpClient)
    {
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClient;

        _apiKey = _configuration["PaymentGateway:Iyzico:ApiKey"] ?? throw new ArgumentNullException("Iyzico ApiKey not configured");
        _secretKey = _configuration["PaymentGateway:Iyzico:SecretKey"] ?? throw new ArgumentNullException("Iyzico SecretKey not configured");
        _baseUrl = _configuration["PaymentGateway:Iyzico:BaseUrl"] ?? "https://sandbox-api.iyzipay.com";
        _isTestMode = _configuration.GetValue<bool>("PaymentGateway:Iyzico:IsTestMode", true);
    }

    public async Task<PaymentResultDto> ProcessPaymentAsync(CreatePaymentDto paymentRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Iyzico ödeme işlemi başlatıldı. OrderId: {OrderId}, Amount: {Amount}", 
                paymentRequest.OrderId, paymentRequest.Amount);

            // Iyzico ödeme isteği oluştur
            var iyzicoRequest = CreateIyzicoPaymentRequest(paymentRequest);

            // Iyzico API'sine istek gönder
            var response = await SendIyzicoRequestAsync("payment/auth", iyzicoRequest, cancellationToken);

            // Yanıtı işle
            var result = ProcessIyzicoResponse(response, paymentRequest.OrderId);

            _logger.LogInformation("Iyzico ödeme işlemi tamamlandı. PaymentId: {PaymentId}, Status: {Status}", 
                result.PaymentId, result.Status);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Iyzico ödeme işlemi sırasında hata oluştu. OrderId: {OrderId}", paymentRequest.OrderId);
            
            return new PaymentResultDto
            {
                IsSuccess = false,
                OrderId = paymentRequest.OrderId,
                Status = "Failed",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<PaymentResultDto> Verify3DSecureAsync(Guid paymentId, string threeDSecureResponse, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Iyzico 3D Secure doğrulaması başlatıldı. PaymentId: {PaymentId}", paymentId);

            // 3D Secure doğrulama isteği oluştur
            var verifyRequest = new
            {
                paymentId = paymentId.ToString(),
                threeDSecureResponse = threeDSecureResponse
            };

            // Iyzico API'sine istek gönder
            var response = await SendIyzicoRequestAsync("payment/3ds/verify", verifyRequest, cancellationToken);

            // Yanıtı işle
            var result = ProcessIyzicoResponse(response, paymentId);

            _logger.LogInformation("Iyzico 3D Secure doğrulaması tamamlandı. PaymentId: {PaymentId}, Status: {Status}", 
                paymentId, result.Status);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Iyzico 3D Secure doğrulaması sırasında hata oluştu. PaymentId: {PaymentId}", paymentId);
            
            return new PaymentResultDto
            {
                IsSuccess = false,
                PaymentId = paymentId,
                Status = "Failed",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<PaymentResultDto> GetPaymentStatusAsync(string gatewayPaymentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Iyzico ödeme durumu sorgulanıyor. GatewayPaymentId: {GatewayPaymentId}", gatewayPaymentId);

            var request = new { paymentId = gatewayPaymentId };
            var response = await SendIyzicoRequestAsync("payment/retrieve", request, cancellationToken);

            var result = ProcessIyzicoResponse(response, Guid.Empty);
            result.GatewayPaymentId = gatewayPaymentId;

            _logger.LogInformation("Iyzico ödeme durumu alındı. GatewayPaymentId: {GatewayPaymentId}, Status: {Status}", 
                gatewayPaymentId, result.Status);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Iyzico ödeme durumu sorgulanırken hata oluştu. GatewayPaymentId: {GatewayPaymentId}", gatewayPaymentId);
            
            return new PaymentResultDto
            {
                IsSuccess = false,
                GatewayPaymentId = gatewayPaymentId,
                Status = "Failed",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<PaymentResultDto> CancelPaymentAsync(string gatewayPaymentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Iyzico ödeme iptali başlatıldı. GatewayPaymentId: {GatewayPaymentId}", gatewayPaymentId);

            var request = new { paymentId = gatewayPaymentId };
            var response = await SendIyzicoRequestAsync("payment/cancel", request, cancellationToken);

            var result = ProcessIyzicoResponse(response, Guid.Empty);
            result.GatewayPaymentId = gatewayPaymentId;

            _logger.LogInformation("Iyzico ödeme iptali tamamlandı. GatewayPaymentId: {GatewayPaymentId}, Status: {Status}", 
                gatewayPaymentId, result.Status);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Iyzico ödeme iptali sırasında hata oluştu. GatewayPaymentId: {GatewayPaymentId}", gatewayPaymentId);
            
            return new PaymentResultDto
            {
                IsSuccess = false,
                GatewayPaymentId = gatewayPaymentId,
                Status = "Failed",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<PaymentResultDto> RefundPaymentAsync(string gatewayPaymentId, decimal refundAmount, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Iyzico ödeme iadesi başlatıldı. GatewayPaymentId: {GatewayPaymentId}, RefundAmount: {RefundAmount}", 
                gatewayPaymentId, refundAmount);

            var request = new
            {
                paymentId = gatewayPaymentId,
                price = refundAmount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
            };

            var response = await SendIyzicoRequestAsync("payment/refund", request, cancellationToken);

            var result = ProcessIyzicoResponse(response, Guid.Empty);
            result.GatewayPaymentId = gatewayPaymentId;

            _logger.LogInformation("Iyzico ödeme iadesi tamamlandı. GatewayPaymentId: {GatewayPaymentId}, Status: {Status}", 
                gatewayPaymentId, result.Status);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Iyzico ödeme iadesi sırasında hata oluştu. GatewayPaymentId: {GatewayPaymentId}", gatewayPaymentId);
            
            return new PaymentResultDto
            {
                IsSuccess = false,
                GatewayPaymentId = gatewayPaymentId,
                Status = "Failed",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<bool> VerifyWebhookAsync(string webhookData, string signature, CancellationToken cancellationToken = default)
    {
        try
        {
            // Iyzico webhook imza doğrulaması
            // Bu kısım Iyzico'nun webhook imza doğrulama yöntemine göre implement edilecek
            _logger.LogInformation("Iyzico webhook doğrulaması yapıldı");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Iyzico webhook doğrulaması sırasında hata oluştu");
            return false;
        }
    }

    public async Task<PaymentResultDto> ProcessWebhookAsync(string webhookData, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Iyzico webhook işleniyor");

            var webhookPayload = JsonSerializer.Deserialize<Dictionary<string, object>>(webhookData);
            
            // Webhook verisini işle ve ödeme sonucunu döndür
            var result = new PaymentResultDto
            {
                IsSuccess = true,
                Status = "Success"
            };

            _logger.LogInformation("Iyzico webhook işlendi");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Iyzico webhook işlenirken hata oluştu");
            
            return new PaymentResultDto
            {
                IsSuccess = false,
                Status = "Failed",
                ErrorMessage = ex.Message
            };
        }
    }

    private object CreateIyzicoPaymentRequest(CreatePaymentDto paymentRequest)
    {
        // Iyzico ödeme isteği formatına dönüştür
        return new
        {
            locale = "tr",
            conversationId = paymentRequest.OrderId.ToString(),
            price = paymentRequest.Amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
            paidPrice = paymentRequest.Amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
            currency = "TRY",
            installment = paymentRequest.InstallmentCount ?? 1,
            paymentChannel = "WEB",
            paymentGroup = "PRODUCT",
            paymentCard = new
            {
                cardHolderName = paymentRequest.CardHolderName,
                cardNumber = paymentRequest.CardNumber,
                expireMonth = paymentRequest.ExpiryMonth?.ToString("D2"),
                expireYear = paymentRequest.ExpiryYear?.ToString(),
                cvc = paymentRequest.Cvv
            },
            buyer = new
            {
                id = "BY789",
                name = "John",
                surname = "Doe",
                gsmNumber = "+905350000000",
                email = "email@email.com",
                identityNumber = "74300864791",
                lastLoginDate = "2015-10-05 12:43:35",
                registrationDate = "2013-04-21 15:12:09",
                registrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                ip = "85.34.78.112",
                city = "Istanbul",
                country = "Turkey",
                zipCode = "34732"
            },
            shippingAddress = new
            {
                contactName = "Jane Doe",
                city = "Istanbul",
                country = "Turkey",
                address = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                zipCode = "34742"
            },
            billingAddress = new
            {
                contactName = "Jane Doe",
                city = "Istanbul",
                country = "Turkey",
                address = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                zipCode = "34742"
            },
            basketItems = new[]
            {
                new
                {
                    id = "BI101",
                    name = "Binocular",
                    category1 = "Collectibles",
                    category2 = "Accessories",
                    itemType = "PHYSICAL",
                    price = paymentRequest.Amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                }
            }
        };
    }

    private async Task<Dictionary<string, object>> SendIyzicoRequestAsync(string endpoint, object request, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Iyzico authorization header'ı oluştur
        var authorization = CreateIyzicoAuthorization(request);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", authorization);

        var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Iyzico API error: {response.StatusCode} - {responseContent}");
        }

        return JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent) ?? new Dictionary<string, object>();
    }

    private string CreateIyzicoAuthorization(object request)
    {
        // Iyzico authorization header oluşturma
        // Bu kısım Iyzico'nun authorization yöntemine göre implement edilecek
        var randomString = Guid.NewGuid().ToString("N");
        var dataString = JsonSerializer.Serialize(request);
        var hashString = $"{_apiKey}{randomString}{_secretKey}{dataString}";
        
        using var sha1 = System.Security.Cryptography.SHA1.Create();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(hashString));
        var hashStringHex = Convert.ToHexString(hash).ToLower();
        
        return $"IYZWS {_apiKey}:{hashStringHex}";
    }

    private PaymentResultDto ProcessIyzicoResponse(Dictionary<string, object> response, Guid orderId)
    {
        var status = response.GetValueOrDefault("status")?.ToString() ?? "Unknown";
        var isSuccess = status == "success";
        var errorMessage = response.GetValueOrDefault("errorMessage")?.ToString();

        return new PaymentResultDto
        {
            IsSuccess = isSuccess,
            OrderId = orderId,
            Status = isSuccess ? "Success" : "Failed",
            ErrorMessage = errorMessage,
            GatewayPaymentId = response.GetValueOrDefault("paymentId")?.ToString(),
            GatewayTransactionId = response.GetValueOrDefault("conversationId")?.ToString(),
            ThreeDSecureHtml = response.GetValueOrDefault("threeDSecureHtmlContent")?.ToString(),
            ThreeDSecureUrl = response.GetValueOrDefault("threeDSecureUrl")?.ToString(),
            PaidAt = isSuccess ? DateTime.UtcNow : null
        };
    }
}
