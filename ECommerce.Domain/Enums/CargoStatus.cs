namespace ECommerce.Domain.Enums;

/// <summary>
/// Kargo durumları
/// </summary>
public enum CargoStatus
{
    /// <summary>
    /// Kargo oluşturuldu
    /// </summary>
    Created = 0,

    /// <summary>
    /// Kargo hazırlanıyor
    /// </summary>
    Preparing = 1,

    /// <summary>
    /// Kargo şirketine teslim edildi
    /// </summary>
    PickedUp = 2,

    /// <summary>
    /// Kargo yolda
    /// </summary>
    InTransit = 3,

    /// <summary>
    /// Kargo dağıtım merkezinde
    /// </summary>
    AtDistributionCenter = 4,

    /// <summary>
    /// Kargo teslim için hazır
    /// </summary>
    OutForDelivery = 5,

    /// <summary>
    /// Kargo teslim edildi
    /// </summary>
    Delivered = 6,

    /// <summary>
    /// Kargo teslim edilemedi
    /// </summary>
    DeliveryFailed = 7,

    /// <summary>
    /// Kargo iade edildi
    /// </summary>
    Returned = 8,

    /// <summary>
    /// Kargo kayboldu
    /// </summary>
    Lost = 9,

    /// <summary>
    /// Kargo hasarlı
    /// </summary>
    Damaged = 10
}
