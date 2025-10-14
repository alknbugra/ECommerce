namespace ECommerce.Domain.Enums;

/// <summary>
/// Ödeme durumu enum'u
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Beklemede
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Başarılı
    /// </summary>
    Success = 1,

    /// <summary>
    /// Başarısız
    /// </summary>
    Failed = 2,

    /// <summary>
    /// İptal edildi
    /// </summary>
    Cancelled = 3,

    /// <summary>
    /// İade edildi
    /// </summary>
    Refunded = 4,

    /// <summary>
    /// Kısmi iade
    /// </summary>
    PartiallyRefunded = 5,

    /// <summary>
    /// 3D Secure bekliyor
    /// </summary>
    WaitingFor3DSecure = 6
}