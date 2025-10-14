using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Services.Email;

/// <summary>
/// Bildirim servis implementasyonu
/// </summary>
public class NotificationService : INotificationService
{
    private readonly IEmailService _emailService;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IEmailService emailService,
        ILogger<NotificationService> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<bool> SendUserRegistrationEmailAsync(string userEmail, string userName, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Kullanıcı kayıt emaili gönderiliyor. Email: {Email}, Name: {Name}", userEmail, userName);

            var variables = new Dictionary<string, object>
            {
                { "UserName", userName },
                { "UserEmail", userEmail },
                { "LoginUrl", "https://yourapp.com/login" },
                { "SupportEmail", "support@yourapp.com" }
            };

            var sendEmailDto = new SendEmailDto
            {
                ToEmail = userEmail,
                ToName = userName,
                EmailType = "UserRegistration",
                Variables = variables
            };

            var result = await _emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);

            _logger.LogInformation("Kullanıcı kayıt emaili gönderim sonucu. Email: {Email}, Success: {Success}", userEmail, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı kayıt emaili gönderim sırasında hata oluştu. Email: {Email}", userEmail);
            return false;
        }
    }

    public async Task<bool> SendPasswordResetEmailAsync(string userEmail, string userName, string resetToken, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Şifre sıfırlama emaili gönderiliyor. Email: {Email}, Name: {Name}", userEmail, userName);

            var resetUrl = $"https://yourapp.com/reset-password?token={resetToken}";

            var variables = new Dictionary<string, object>
            {
                { "UserName", userName },
                { "UserEmail", userEmail },
                { "ResetUrl", resetUrl },
                { "ExpiryHours", 24 }
            };

            var sendEmailDto = new SendEmailDto
            {
                ToEmail = userEmail,
                ToName = userName,
                EmailType = "PasswordReset",
                Variables = variables
            };

            var result = await _emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);

            _logger.LogInformation("Şifre sıfırlama emaili gönderim sonucu. Email: {Email}, Success: {Success}", userEmail, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Şifre sıfırlama emaili gönderim sırasında hata oluştu. Email: {Email}", userEmail);
            return false;
        }
    }

    public async Task<bool> SendOrderConfirmationEmailAsync(OrderDto orderDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Sipariş onay emaili gönderiliyor. OrderNumber: {OrderNumber}", orderDto.OrderNumber);

            var variables = new Dictionary<string, object>
            {
                { "OrderNumber", orderDto.OrderNumber },
                { "OrderDate", orderDto.CreatedAt.ToString("dd.MM.yyyy HH:mm") },
                { "TotalAmount", orderDto.TotalAmount.ToString("C") },
                { "CustomerName", orderDto.UserName ?? "Müşteri" },
                { "ShippingAddress", orderDto.ShippingAddress?.ToString() ?? "" },
                { "BillingAddress", orderDto.BillingAddress?.ToString() ?? "" },
                { "OrderItems", string.Join(", ", orderDto.OrderItems?.Select(oi => $"{oi.ProductName} x{oi.Quantity}") ?? new List<string>()) },
                { "OrderUrl", $"https://yourapp.com/orders/{orderDto.Id}" }
            };

            var sendEmailDto = new SendEmailDto
            {
                ToEmail = orderDto.UserEmail ?? "",
                ToName = orderDto.UserName,
                EmailType = "OrderConfirmation",
                RelatedEntityId = orderDto.Id,
                RelatedEntityType = "Order",
                Variables = variables
            };

            var result = await _emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);

            _logger.LogInformation("Sipariş onay emaili gönderim sonucu. OrderNumber: {OrderNumber}, Success: {Success}", 
                orderDto.OrderNumber, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş onay emaili gönderim sırasında hata oluştu. OrderNumber: {OrderNumber}", orderDto.OrderNumber);
            return false;
        }
    }

    public async Task<bool> SendPaymentSuccessEmailAsync(PaymentDto paymentDto, OrderDto orderDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Ödeme başarılı emaili gönderiliyor. OrderNumber: {OrderNumber}, PaymentId: {PaymentId}", 
                orderDto.OrderNumber, paymentDto.Id);

            var variables = new Dictionary<string, object>
            {
                { "OrderNumber", orderDto.OrderNumber },
                { "PaymentAmount", paymentDto.Amount.ToString("C") },
                { "PaymentDate", paymentDto.PaidAt?.ToString("dd.MM.yyyy HH:mm") ?? DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm") },
                { "PaymentMethod", paymentDto.PaymentMethod },
                { "CustomerName", orderDto.UserName ?? "Müşteri" },
                { "OrderUrl", $"https://yourapp.com/orders/{orderDto.Id}" }
            };

            var sendEmailDto = new SendEmailDto
            {
                ToEmail = orderDto.UserEmail ?? "",
                ToName = orderDto.UserName,
                EmailType = "PaymentSuccess",
                RelatedEntityId = paymentDto.Id,
                RelatedEntityType = "Payment",
                Variables = variables
            };

            var result = await _emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);

            _logger.LogInformation("Ödeme başarılı emaili gönderim sonucu. OrderNumber: {OrderNumber}, Success: {Success}", 
                orderDto.OrderNumber, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ödeme başarılı emaili gönderim sırasında hata oluştu. OrderNumber: {OrderNumber}", orderDto.OrderNumber);
            return false;
        }
    }

    public async Task<bool> SendOrderStatusChangeEmailAsync(OrderDto orderDto, string oldStatus, string newStatus, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Sipariş durumu değişiklik emaili gönderiliyor. OrderNumber: {OrderNumber}, OldStatus: {OldStatus}, NewStatus: {NewStatus}", 
                orderDto.OrderNumber, oldStatus, newStatus);

            var statusTexts = new Dictionary<string, string>
            {
                { "Pending", "Beklemede" },
                { "Confirmed", "Onaylandı" },
                { "Processing", "Hazırlanıyor" },
                { "Shipped", "Kargoya Verildi" },
                { "Delivered", "Teslim Edildi" },
                { "Cancelled", "İptal Edildi" },
                { "Returned", "İade Edildi" }
            };

            var variables = new Dictionary<string, object>
            {
                { "OrderNumber", orderDto.OrderNumber },
                { "OldStatus", statusTexts.GetValueOrDefault(oldStatus, oldStatus) },
                { "NewStatus", statusTexts.GetValueOrDefault(newStatus, newStatus) },
                { "CustomerName", orderDto.UserName ?? "Müşteri" },
                { "OrderUrl", $"https://yourapp.com/orders/{orderDto.Id}" }
            };

            var sendEmailDto = new SendEmailDto
            {
                ToEmail = orderDto.UserEmail ?? "",
                ToName = orderDto.UserName,
                EmailType = "OrderStatusChange",
                RelatedEntityId = orderDto.Id,
                RelatedEntityType = "Order",
                Variables = variables
            };

            var result = await _emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);

            _logger.LogInformation("Sipariş durumu değişiklik emaili gönderim sonucu. OrderNumber: {OrderNumber}, Success: {Success}", 
                orderDto.OrderNumber, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş durumu değişiklik emaili gönderim sırasında hata oluştu. OrderNumber: {OrderNumber}", orderDto.OrderNumber);
            return false;
        }
    }

    public async Task<bool> SendShippingInfoEmailAsync(OrderDto orderDto, string trackingNumber, string shippingCompany, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Kargo bilgisi emaili gönderiliyor. OrderNumber: {OrderNumber}, TrackingNumber: {TrackingNumber}", 
                orderDto.OrderNumber, trackingNumber);

            var variables = new Dictionary<string, object>
            {
                { "OrderNumber", orderDto.OrderNumber },
                { "TrackingNumber", trackingNumber },
                { "ShippingCompany", shippingCompany },
                { "CustomerName", orderDto.UserName ?? "Müşteri" },
                { "ShippingAddress", orderDto.ShippingAddress?.ToString() ?? "" },
                { "TrackingUrl", $"https://yourapp.com/tracking/{trackingNumber}" }
            };

            var sendEmailDto = new SendEmailDto
            {
                ToEmail = orderDto.UserEmail ?? "",
                ToName = orderDto.UserName,
                EmailType = "ShippingInfo",
                RelatedEntityId = orderDto.Id,
                RelatedEntityType = "Order",
                Variables = variables
            };

            var result = await _emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);

            _logger.LogInformation("Kargo bilgisi emaili gönderim sonucu. OrderNumber: {OrderNumber}, Success: {Success}", 
                orderDto.OrderNumber, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo bilgisi emaili gönderim sırasında hata oluştu. OrderNumber: {OrderNumber}", orderDto.OrderNumber);
            return false;
        }
    }

    public async Task<bool> SendPriceChangeNotificationAsync(string userEmail, string productName, decimal oldPrice, decimal newPrice, string userName, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fiyat değişikliği bildirimi gönderiliyor. UserEmail: {UserEmail}, ProductName: {ProductName}", 
                userEmail, productName);

            var priceChangePercentage = oldPrice > 0 ? ((newPrice - oldPrice) / oldPrice) * 100 : 0;
            var isPriceDrop = newPrice < oldPrice;

            var variables = new Dictionary<string, object>
            {
                { "ProductName", productName },
                { "OldPrice", oldPrice.ToString("C") },
                { "NewPrice", newPrice.ToString("C") },
                { "PriceChangePercentage", Math.Abs(priceChangePercentage).ToString("F1") },
                { "IsPriceDrop", isPriceDrop },
                { "CustomerName", userName },
                { "ProductUrl", $"https://yourapp.com/products/{productName.Replace(" ", "-").ToLower()}" }
            };

            var sendEmailDto = new SendEmailDto
            {
                ToEmail = userEmail,
                ToName = userName,
                EmailType = "PriceChange",
                RelatedEntityType = "Wishlist",
                Variables = variables
            };

            var result = await _emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);

            _logger.LogInformation("Fiyat değişikliği bildirimi gönderim sonucu. UserEmail: {UserEmail}, Success: {Success}", 
                userEmail, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fiyat değişikliği bildirimi gönderim sırasında hata oluştu. UserEmail: {UserEmail}", userEmail);
            return false;
        }
    }

    public async Task<bool> SendStockChangeNotificationAsync(string userEmail, string productName, bool isInStock, string userName, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Stok değişikliği bildirimi gönderiliyor. UserEmail: {UserEmail}, ProductName: {ProductName}, IsInStock: {IsInStock}", 
                userEmail, productName, isInStock);

            var variables = new Dictionary<string, object>
            {
                { "ProductName", productName },
                { "IsInStock", isInStock },
                { "StockStatus", isInStock ? "Stokta" : "Stokta Yok" },
                { "CustomerName", userName },
                { "ProductUrl", $"https://yourapp.com/products/{productName.Replace(" ", "-").ToLower()}" }
            };

            var sendEmailDto = new SendEmailDto
            {
                ToEmail = userEmail,
                ToName = userName,
                EmailType = "StockChange",
                RelatedEntityType = "Wishlist",
                Variables = variables
            };

            var result = await _emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);

            _logger.LogInformation("Stok değişikliği bildirimi gönderim sonucu. UserEmail: {UserEmail}, Success: {Success}", 
                userEmail, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok değişikliği bildirimi gönderim sırasında hata oluştu. UserEmail: {UserEmail}", userEmail);
            return false;
        }
    }
}
