namespace ECommerce.Application.DTOs;

/// <summary>
/// Arama önerisi DTO'su
/// </summary>
public class SearchSuggestionDto
{
    /// <summary>
    /// Öneri metni
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Öneri türü
    /// </summary>
    public SuggestionType Type { get; set; }

    /// <summary>
    /// İlgili ID (ürün, kategori vb.)
    /// </summary>
    public Guid? RelatedId { get; set; }

    /// <summary>
    /// Öneri skoru (0-100)
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Ek bilgi
    /// </summary>
    public string? AdditionalInfo { get; set; }
}

/// <summary>
/// Öneri türleri
/// </summary>
public enum SuggestionType
{
    /// <summary>
    /// Ürün adı
    /// </summary>
    Product = 0,

    /// <summary>
    /// Kategori
    /// </summary>
    Category = 1,

    /// <summary>
    /// Marka
    /// </summary>
    Brand = 2,

    /// <summary>
    /// Özellik
    /// </summary>
    Attribute = 3,

    /// <summary>
    /// Popüler arama
    /// </summary>
    PopularSearch = 4
}
