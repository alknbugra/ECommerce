namespace ECommerce.Application.DTOs;

/// <summary>
/// Favori liste DTO'su
/// </summary>
public class WishlistDto
{
    /// <summary>
    /// Liste ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Kullanıcı adı
    /// </summary>
    public string UserName { get; set; } = string.Empty;

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
    public string ListType { get; set; } = string.Empty;

    /// <summary>
    /// Liste paylaşılabilir mi?
    /// </summary>
    public bool IsShareable { get; set; }

    /// <summary>
    /// Paylaşım kodu
    /// </summary>
    public string? ShareCode { get; set; }

    /// <summary>
    /// Liste varsayılan mı?
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Liste sırası
    /// </summary>
    public int SortOrder { get; set; }

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
    /// Liste aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Liste paylaşılmış mı?
    /// </summary>
    public bool IsShared { get; set; }

    /// <summary>
    /// Toplam ürün sayısı
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Favori ürünler
    /// </summary>
    public List<WishlistItemDto>? WishlistItems { get; set; }

    /// <summary>
    /// Paylaşım bilgileri
    /// </summary>
    public List<WishlistShareDto>? WishlistShares { get; set; }
}
