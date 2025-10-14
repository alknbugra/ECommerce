using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Favori liste entity'si
/// </summary>
public class Wishlist : BaseEntity
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Liste adı
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Liste açıklaması
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Liste türü
    /// </summary>
    [MaxLength(20)]
    public string ListType { get; set; } = "Personal"; // Personal, Shared, Public

    /// <summary>
    /// Liste paylaşılabilir mi?
    /// </summary>
    public bool IsShareable { get; set; } = false;

    /// <summary>
    /// Paylaşım kodu (unique)
    /// </summary>
    [MaxLength(50)]
    public string? ShareCode { get; set; }

    /// <summary>
    /// Liste varsayılan mı?
    /// </summary>
    public bool IsDefault { get; set; } = false;

    /// <summary>
    /// Liste sırası
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// Liste rengi (hex)
    /// </summary>
    [MaxLength(7)]
    public string? Color { get; set; }

    /// <summary>
    /// Liste ikonu
    /// </summary>
    [MaxLength(50)]
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

    /// <summary>
    /// Kullanıcı
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Favori ürünler
    /// </summary>
    public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();

    /// <summary>
    /// Paylaşım izinleri
    /// </summary>
    public virtual ICollection<WishlistShare> WishlistShares { get; set; } = new List<WishlistShare>();

    /// <summary>
    /// Liste aktif mi?
    /// </summary>
    [NotMapped]
    public bool IsActive => !IsDeleted;

    /// <summary>
    /// Liste paylaşılmış mı?
    /// </summary>
    [NotMapped]
    public bool IsShared => !string.IsNullOrEmpty(ShareCode);

    /// <summary>
    /// Toplam ürün sayısı
    /// </summary>
    [NotMapped]
    public int TotalItems => WishlistItems?.Count ?? 0;
}
