namespace ECommerce.Application.DTOs;

/// <summary>
/// Arama sonucu DTO'su
/// </summary>
public class SearchResultDto
{
    /// <summary>
    /// Ürünler
    /// </summary>
    public List<ProductDto> Products { get; set; } = new();

    /// <summary>
    /// Toplam ürün sayısı
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Sayfa numarası
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Toplam sayfa sayısı
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Arama süresi (milisaniye)
    /// </summary>
    public long SearchTimeMs { get; set; }

    /// <summary>
    /// Filtreleme seçenekleri
    /// </summary>
    public SearchFiltersDto? Filters { get; set; }

    /// <summary>
    /// Arama önerileri
    /// </summary>
    public List<string>? Suggestions { get; set; }
}
