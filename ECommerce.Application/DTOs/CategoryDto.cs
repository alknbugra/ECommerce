namespace ECommerce.Application.DTOs;

/// <summary>
/// Kategori DTO'su
/// </summary>
public class CategoryDto
{
    /// <summary>
    /// Kategori ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kategori adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategori açıklaması
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Kategori resmi URL'si
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Üst kategori ID'si
    /// </summary>
    public Guid? ParentCategoryId { get; set; }

    /// <summary>
    /// Üst kategori adı
    /// </summary>
    public string? ParentCategoryName { get; set; }

    /// <summary>
    /// Alt kategoriler
    /// </summary>
    public List<CategoryDto> SubCategories { get; set; } = new();

    /// <summary>
    /// Kategori aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Sıralama düzeni
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Ürün sayısı
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Alt kategori sayısı
    /// </summary>
    public int SubCategoryCount { get; set; }
}
