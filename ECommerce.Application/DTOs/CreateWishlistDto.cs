namespace ECommerce.Application.DTOs;

/// <summary>
/// Favori liste oluşturma DTO'su
/// </summary>
public class CreateWishlistDto
{
    /// <summary>
    /// Liste adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Liste açıklaması
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Liste türü
    /// </summary>
    public string ListType { get; set; } = "Personal";

    /// <summary>
    /// Liste paylaşılabilir mi?
    /// </summary>
    public bool IsShareable { get; set; } = false;

    /// <summary>
    /// Liste varsayılan mı?
    /// </summary>
    public bool IsDefault { get; set; } = false;

    /// <summary>
    /// Liste sırası
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// Liste rengi
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Liste ikonu
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Fiyat takibi aktif mi?
    /// </summary>
    public bool PriceTrackingEnabled { get; set; } = true;

    /// <summary>
    /// Stok takibi aktif mi?
    /// </summary>
    public bool StockTrackingEnabled { get; set; } = true;

    /// <summary>
    /// E-posta bildirimleri aktif mi?
    /// </summary>
    public bool EmailNotificationsEnabled { get; set; } = true;
}
