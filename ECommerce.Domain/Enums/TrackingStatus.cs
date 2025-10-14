namespace ECommerce.Domain.Enums;

/// <summary>
/// Kargo takip durumları
/// </summary>
public enum TrackingStatus
{
    /// <summary>
    /// Takip bilgisi mevcut değil
    /// </summary>
    NotAvailable = 0,

    /// <summary>
    /// Takip bilgisi güncelleniyor
    /// </summary>
    Updating = 1,

    /// <summary>
    /// Takip bilgisi güncel
    /// </summary>
    Current = 2,

    /// <summary>
    /// Takip bilgisi gecikmeli
    /// </summary>
    Delayed = 3,

    /// <summary>
    /// Takip bilgisi hatalı
    /// </summary>
    Error = 4
}
