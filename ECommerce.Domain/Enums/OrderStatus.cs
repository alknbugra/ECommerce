namespace ECommerce.Domain.Enums;

/// <summary>
/// Sipariş durumları
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Beklemede
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Onaylandı
    /// </summary>
    Confirmed = 1,

    /// <summary>
    /// Hazırlanıyor
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Kargoya verildi
    /// </summary>
    Shipped = 3,

    /// <summary>
    /// Teslim edildi
    /// </summary>
    Delivered = 4,

    /// <summary>
    /// İptal edildi
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// İade edildi
    /// </summary>
    Returned = 6,

    /// <summary>
    /// İptal edildi (müşteri tarafından)
    /// </summary>
    CancelledByCustomer = 7
}
