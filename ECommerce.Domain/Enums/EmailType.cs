namespace ECommerce.Domain.Enums;

/// <summary>
/// Email türü enum'u
/// </summary>
public enum EmailType
{
    /// <summary>
    /// Kullanıcı kayıt
    /// </summary>
    UserRegistration = 0,

    /// <summary>
    /// Şifre sıfırlama
    /// </summary>
    PasswordReset = 1,

    /// <summary>
    /// Sipariş onayı
    /// </summary>
    OrderConfirmation = 2,

    /// <summary>
    /// Ödeme başarılı
    /// </summary>
    PaymentSuccess = 3,

    /// <summary>
    /// Sipariş durumu değişikliği
    /// </summary>
    OrderStatusChange = 4,

    /// <summary>
    /// Kargo bilgisi
    /// </summary>
    ShippingInfo = 5,

    /// <summary>
    /// Teslimat onayı
    /// </summary>
    DeliveryConfirmation = 6,

    /// <summary>
    /// Sipariş iptali
    /// </summary>
    OrderCancellation = 7,

    /// <summary>
    /// İade bilgisi
    /// </summary>
    RefundInfo = 8,

    /// <summary>
    /// Genel bildirim
    /// </summary>
    GeneralNotification = 9
}
