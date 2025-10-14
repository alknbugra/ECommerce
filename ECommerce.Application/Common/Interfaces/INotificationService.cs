using ECommerce.Application.DTOs;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Bildirim servis interface'i
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Kullanıcı kayıt emaili gönder
    /// </summary>
    /// <param name="userEmail">Kullanıcı email adresi</param>
    /// <param name="userName">Kullanıcı adı</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Gönderim sonucu</returns>
    Task<bool> SendUserRegistrationEmailAsync(string userEmail, string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Şifre sıfırlama emaili gönder
    /// </summary>
    /// <param name="userEmail">Kullanıcı email adresi</param>
    /// <param name="userName">Kullanıcı adı</param>
    /// <param name="resetToken">Sıfırlama token'ı</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Gönderim sonucu</returns>
    Task<bool> SendPasswordResetEmailAsync(string userEmail, string userName, string resetToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sipariş onay emaili gönder
    /// </summary>
    /// <param name="orderDto">Sipariş DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Gönderim sonucu</returns>
    Task<bool> SendOrderConfirmationEmailAsync(OrderDto orderDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ödeme başarılı emaili gönder
    /// </summary>
    /// <param name="paymentDto">Ödeme DTO'su</param>
    /// <param name="orderDto">Sipariş DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Gönderim sonucu</returns>
    Task<bool> SendPaymentSuccessEmailAsync(PaymentDto paymentDto, OrderDto orderDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sipariş durumu değişiklik emaili gönder
    /// </summary>
    /// <param name="orderDto">Sipariş DTO'su</param>
    /// <param name="oldStatus">Eski durum</param>
    /// <param name="newStatus">Yeni durum</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Gönderim sonucu</returns>
    Task<bool> SendOrderStatusChangeEmailAsync(OrderDto orderDto, string oldStatus, string newStatus, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kargo bilgisi emaili gönder
    /// </summary>
    /// <param name="orderDto">Sipariş DTO'su</param>
    /// <param name="trackingNumber">Takip numarası</param>
    /// <param name="shippingCompany">Kargo şirketi</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Gönderim sonucu</returns>
    Task<bool> SendShippingInfoEmailAsync(OrderDto orderDto, string trackingNumber, string shippingCompany, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fiyat değişikliği bildirimi gönder
    /// </summary>
    /// <param name="userEmail">Kullanıcı email adresi</param>
    /// <param name="productName">Ürün adı</param>
    /// <param name="oldPrice">Eski fiyat</param>
    /// <param name="newPrice">Yeni fiyat</param>
    /// <param name="userName">Kullanıcı adı</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Gönderim sonucu</returns>
    Task<bool> SendPriceChangeNotificationAsync(string userEmail, string productName, decimal oldPrice, decimal newPrice, string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stok değişikliği bildirimi gönder
    /// </summary>
    /// <param name="userEmail">Kullanıcı email adresi</param>
    /// <param name="productName">Ürün adı</param>
    /// <param name="isInStock">Stokta var mı?</param>
    /// <param name="userName">Kullanıcı adı</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Gönderim sonucu</returns>
    Task<bool> SendStockChangeNotificationAsync(string userEmail, string productName, bool isInStock, string userName, CancellationToken cancellationToken = default);
}
