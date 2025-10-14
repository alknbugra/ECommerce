namespace ECommerce.Application.DTOs;

/// <summary>
/// Ürün değerlendirme istatistikleri DTO'su
/// </summary>
public class ProductReviewStatsDto
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Toplam değerlendirme sayısı
    /// </summary>
    public int TotalReviews { get; set; }

    /// <summary>
    /// Onaylanmış değerlendirme sayısı
    /// </summary>
    public int ApprovedReviews { get; set; }

    /// <summary>
    /// Bekleyen değerlendirme sayısı
    /// </summary>
    public int PendingReviews { get; set; }

    /// <summary>
    /// Ortalama puan
    /// </summary>
    public decimal AverageRating { get; set; }

    /// <summary>
    /// 5 yıldız sayısı
    /// </summary>
    public int FiveStarCount { get; set; }

    /// <summary>
    /// 4 yıldız sayısı
    /// </summary>
    public int FourStarCount { get; set; }

    /// <summary>
    /// 3 yıldız sayısı
    /// </summary>
    public int ThreeStarCount { get; set; }

    /// <summary>
    /// 2 yıldız sayısı
    /// </summary>
    public int TwoStarCount { get; set; }

    /// <summary>
    /// 1 yıldız sayısı
    /// </summary>
    public int OneStarCount { get; set; }

    /// <summary>
    /// Puan dağılımı yüzdesi
    /// </summary>
    public Dictionary<int, decimal> RatingDistribution { get; set; } = new();

    /// <summary>
    /// Son değerlendirme tarihi
    /// </summary>
    public DateTime? LastReviewDate { get; set; }

    /// <summary>
    /// İlk değerlendirme tarihi
    /// </summary>
    public DateTime? FirstReviewDate { get; set; }
}
