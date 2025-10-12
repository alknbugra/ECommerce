using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Ürün kategorisi entity'si
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Kategori adı
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategori açıklaması
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Kategori resmi URL'si
    /// </summary>
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Üst kategori ID'si (hierarchical yapı için)
    /// </summary>
    public Guid? ParentCategoryId { get; set; }

    /// <summary>
    /// Üst kategori
    /// </summary>
    public virtual Category? ParentCategory { get; set; }

    /// <summary>
    /// Alt kategoriler
    /// </summary>
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();

    /// <summary>
    /// Bu kategorideki ürünler
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    /// <summary>
    /// Kategori aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Sıralama düzeni
    /// </summary>
    public int SortOrder { get; set; } = 0;
}
