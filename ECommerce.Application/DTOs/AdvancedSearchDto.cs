namespace ECommerce.Application.DTOs;

/// <summary>
/// Gelişmiş arama DTO'su
/// </summary>
public class AdvancedSearchDto
{
    /// <summary>
    /// Arama terimi
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Kategori ID'leri
    /// </summary>
    public List<Guid>? CategoryIds { get; set; }

    /// <summary>
    /// Marka ID'leri
    /// </summary>
    public List<Guid>? BrandIds { get; set; }

    /// <summary>
    /// Minimum fiyat
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Maksimum fiyat
    /// </summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Sadece stokta olan ürünler
    /// </summary>
    public bool? InStock { get; set; }

    /// <summary>
    /// Sadece indirimli ürünler
    /// </summary>
    public bool? OnSale { get; set; }

    /// <summary>
    /// Minimum değerlendirme puanı
    /// </summary>
    public decimal? MinRating { get; set; }

    /// <summary>
    /// Ürün özellikleri filtresi
    /// </summary>
    public Dictionary<string, List<string>>? Attributes { get; set; }

    /// <summary>
    /// Sıralama türü
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sıralama yönü
    /// </summary>
    public string? SortDirection { get; set; }

    /// <summary>
    /// Sayfa numarası
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    public int PageSize { get; set; } = 20;
}
