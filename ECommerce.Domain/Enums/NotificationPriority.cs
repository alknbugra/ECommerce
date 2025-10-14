namespace ECommerce.Domain.Enums;

/// <summary>
/// Bildirim öncelik seviyeleri
/// </summary>
public enum NotificationPriority
{
    /// <summary>
    /// Düşük öncelik
    /// </summary>
    Low = 1,

    /// <summary>
    /// Normal öncelik
    /// </summary>
    Normal = 2,

    /// <summary>
    /// Yüksek öncelik
    /// </summary>
    High = 3,

    /// <summary>
    /// Kritik öncelik
    /// </summary>
    Critical = 4
}
