namespace ECommerce.Application.DTOs;

/// <summary>
/// Favori liste istatistikleri DTO'su
/// </summary>
public class WishlistStatsDto
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Toplam favori liste sayısı
    /// </summary>
    public int TotalWishlists { get; set; }

    /// <summary>
    /// Toplam favori ürün sayısı
    /// </summary>
    public int TotalWishlistItems { get; set; }

    /// <summary>
    /// Paylaşılan liste sayısı
    /// </summary>
    public int SharedWishlists { get; set; }

    /// <summary>
    /// Fiyat takibi aktif ürün sayısı
    /// </summary>
    public int PriceTrackingItems { get; set; }

    /// <summary>
    /// Stok takibi aktif ürün sayısı
    /// </summary>
    public int StockTrackingItems { get; set; }

    /// <summary>
    /// Fiyat düşen ürün sayısı
    /// </summary>
    public int PriceDroppedItems { get; set; }

    /// <summary>
    /// Hedef fiyata ulaşan ürün sayısı
    /// </summary>
    public int TargetPriceReachedItems { get; set; }

    /// <summary>
    /// Stok durumu değişen ürün sayısı
    /// </summary>
    public int StockStatusChangedItems { get; set; }

    /// <summary>
    /// Toplam tasarruf miktarı
    /// </summary>
    public decimal TotalSavings { get; set; }

    /// <summary>
    /// Ortalama fiyat değişim yüzdesi
    /// </summary>
    public decimal AveragePriceChangePercentage { get; set; }

    /// <summary>
    /// En çok favoriye eklenen kategori
    /// </summary>
    public string? MostFavoritedCategory { get; set; }

    /// <summary>
    /// En çok favoriye eklenen marka
    /// </summary>
    public string? MostFavoritedBrand { get; set; }

    /// <summary>
    /// Son güncelleme tarihi
    /// </summary>
    public DateTime? LastUpdatedAt { get; set; }
}
