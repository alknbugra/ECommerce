namespace ECommerce.Application.DTOs;

/// <summary>
/// Arama filtreleri DTO'su
/// </summary>
public class SearchFiltersDto
{
    /// <summary>
    /// Kategori filtreleri
    /// </summary>
    public List<FilterOptionDto> Categories { get; set; } = new();

    /// <summary>
    /// Marka filtreleri
    /// </summary>
    public List<FilterOptionDto> Brands { get; set; } = new();

    /// <summary>
    /// Fiyat aralığı
    /// </summary>
    public PriceRangeDto? PriceRange { get; set; }

    /// <summary>
    /// Özellik filtreleri
    /// </summary>
    public Dictionary<string, List<FilterOptionDto>> Attributes { get; set; } = new();

    /// <summary>
    /// Değerlendirme filtreleri
    /// </summary>
    public List<FilterOptionDto> Ratings { get; set; } = new();
}

/// <summary>
/// Filtre seçeneği DTO'su
/// </summary>
public class FilterOptionDto
{
    /// <summary>
    /// Seçenek ID'si
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Seçenek adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Ürün sayısı
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Seçili mi?
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// Ek bilgi (renk kodu, resim URL'si vb.)
    /// </summary>
    public string? AdditionalInfo { get; set; }
}

/// <summary>
/// Fiyat aralığı DTO'su
/// </summary>
public class PriceRangeDto
{
    /// <summary>
    /// Minimum fiyat
    /// </summary>
    public decimal Min { get; set; }

    /// <summary>
    /// Maksimum fiyat
    /// </summary>
    public decimal Max { get; set; }

    /// <summary>
    /// Seçili minimum fiyat
    /// </summary>
    public decimal? SelectedMin { get; set; }

    /// <summary>
    /// Seçili maksimum fiyat
    /// </summary>
    public decimal? SelectedMax { get; set; }
}
