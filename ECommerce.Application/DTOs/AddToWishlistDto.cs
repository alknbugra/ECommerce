namespace ECommerce.Application.DTOs;

/// <summary>
/// Favorilere ekleme DTO'su
/// </summary>
public class AddToWishlistDto
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Favori liste ID'si (opsiyonel - varsayılan liste kullanılır)
    /// </summary>
    public Guid? WishlistId { get; set; }

    /// <summary>
    /// Notlar
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Öncelik seviyesi
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// Hedef fiyat
    /// </summary>
    public decimal? TargetPrice { get; set; }

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
