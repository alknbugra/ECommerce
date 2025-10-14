namespace ECommerce.Application.DTOs;

/// <summary>
/// Favori liste ürünü DTO'su
/// </summary>
public class WishlistItemDto
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Favori liste ID'si
    /// </summary>
    public Guid WishlistId { get; set; }

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Ürün bilgileri
    /// </summary>
    public ProductDto? Product { get; set; }

    /// <summary>
    /// Ürün fiyatı (eklendiği zamanki)
    /// </summary>
    public decimal PriceAtTime { get; set; }

    /// <summary>
    /// Ürün indirimli fiyatı (eklendiği zamanki)
    /// </summary>
    public decimal? DiscountedPriceAtTime { get; set; }

    /// <summary>
    /// Ürün stok durumu (eklendiği zamanki)
    /// </summary>
    public bool WasInStock { get; set; }

    /// <summary>
    /// Notlar
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Öncelik seviyesi
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Hedef fiyat
    /// </summary>
    public decimal? TargetPrice { get; set; }

    /// <summary>
    /// Fiyat takibi aktif mi?
    /// </summary>
    public bool PriceTrackingEnabled { get; set; }

    /// <summary>
    /// Stok takibi aktif mi?
    /// </summary>
    public bool StockTrackingEnabled { get; set; }

    /// <summary>
    /// E-posta bildirimleri aktif mi?
    /// </summary>
    public bool EmailNotificationsEnabled { get; set; }

    /// <summary>
    /// Son fiyat bildirimi tarihi
    /// </summary>
    public DateTime? LastPriceNotificationAt { get; set; }

    /// <summary>
    /// Son stok bildirimi tarihi
    /// </summary>
    public DateTime? LastStockNotificationAt { get; set; }

    /// <summary>
    /// Ürün aktif mi?
    /// </summary>
    public bool IsProductActive { get; set; }

    /// <summary>
    /// Fiyat düştü mü?
    /// </summary>
    public bool HasPriceDropped { get; set; }

    /// <summary>
    /// Hedef fiyata ulaşıldı mı?
    /// </summary>
    public bool HasReachedTargetPrice { get; set; }

    /// <summary>
    /// Stok durumu değişti mi?
    /// </summary>
    public bool HasStockStatusChanged { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Fiyat geçmişi
    /// </summary>
    public List<WishlistItemPriceHistoryDto>? PriceHistory { get; set; }

    /// <summary>
    /// Stok geçmişi
    /// </summary>
    public List<WishlistItemStockHistoryDto>? StockHistory { get; set; }
}
