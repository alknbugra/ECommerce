namespace ECommerce.Domain.Enums;

/// <summary>
/// Email durumu enum'u
/// </summary>
public enum EmailStatus
{
    /// <summary>
    /// Beklemede
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Gönderiliyor
    /// </summary>
    Sending = 1,

    /// <summary>
    /// Başarılı
    /// </summary>
    Sent = 2,

    /// <summary>
    /// Başarısız
    /// </summary>
    Failed = 3,

    /// <summary>
    /// İptal edildi
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Yeniden denenecek
    /// </summary>
    Retry = 5
}
