namespace ECommerce.Domain.Enums;

/// <summary>
/// Bildirim türleri
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// Sipariş bildirimi
    /// </summary>
    Order = 1,

    /// <summary>
    /// Stok bildirimi
    /// </summary>
    Stock = 2,

    /// <summary>
    /// Fiyat bildirimi
    /// </summary>
    Price = 3,

    /// <summary>
    /// Kullanıcı etkileşim bildirimi
    /// </summary>
    UserInteraction = 4,

    /// <summary>
    /// Sistem bildirimi
    /// </summary>
    System = 5,

    /// <summary>
    /// Kampanya bildirimi
    /// </summary>
    Campaign = 6,

    /// <summary>
    /// Ödeme bildirimi
    /// </summary>
    Payment = 7,

    /// <summary>
    /// Kargo bildirimi
    /// </summary>
    Shipping = 8,

    /// <summary>
    /// İade bildirimi
    /// </summary>
    Return = 9,

    /// <summary>
    /// Genel bildirim
    /// </summary>
    General = 10
}
