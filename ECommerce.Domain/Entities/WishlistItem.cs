using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Favori liste ürünü entity'si
/// </summary>
public class WishlistItem : BaseEntity
{
    /// <summary>
    /// Favori liste ID'si
    /// </summary>
    public Guid WishlistId { get; set; }

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Ürün fiyatı (eklendiği zamanki fiyat)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal PriceAtTime { get; set; }

    /// <summary>
    /// Ürün indirimli fiyatı (eklendiği zamanki)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountedPriceAtTime { get; set; }

    /// <summary>
    /// Ürün stok durumu (eklendiği zamanki)
    /// </summary>
    public bool WasInStock { get; set; } = true;

    /// <summary>
    /// Notlar
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Öncelik seviyesi
    /// </summary>
    public int Priority { get; set; } = 0; // 0: Normal, 1: Yüksek, 2: Çok Yüksek

    /// <summary>
    /// Hedef fiyat (kullanıcının beklediği fiyat)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
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

    /// <summary>
    /// Son fiyat bildirimi tarihi
    /// </summary>
    public DateTime? LastPriceNotificationAt { get; set; }

    /// <summary>
    /// Son stok bildirimi tarihi
    /// </summary>
    public DateTime? LastStockNotificationAt { get; set; }

    /// <summary>
    /// Favori liste
    /// </summary>
    public virtual Wishlist Wishlist { get; set; } = null!;

    /// <summary>
    /// Ürün
    /// </summary>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Fiyat geçmişi
    /// </summary>
    public virtual ICollection<WishlistItemPriceHistory> PriceHistory { get; set; } = new List<WishlistItemPriceHistory>();

    /// <summary>
    /// Stok geçmişi
    /// </summary>
    public virtual ICollection<WishlistItemStockHistory> StockHistory { get; set; } = new List<WishlistItemStockHistory>();

    /// <summary>
    /// Ürün aktif mi?
    /// </summary>
    [NotMapped]
    public bool IsProductActive => Product?.IsActive ?? false;

    /// <summary>
    /// Fiyat düştü mü?
    /// </summary>
    [NotMapped]
    public bool HasPriceDropped => Product != null && Product.Price < PriceAtTime;

    /// <summary>
    /// Hedef fiyata ulaşıldı mı?
    /// </summary>
    [NotMapped]
    public bool HasReachedTargetPrice => TargetPrice.HasValue && Product != null && Product.Price <= TargetPrice.Value;

    /// <summary>
    /// Stok durumu değişti mi?
    /// </summary>
    [NotMapped]
    public bool HasStockStatusChanged => Product != null && Product.StockQuantity > 0 != WasInStock;
}
