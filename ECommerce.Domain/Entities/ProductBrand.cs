using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Ürün markası entity'si
/// </summary>
public class ProductBrand : BaseEntity
{
    /// <summary>
    /// Marka adı
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Marka açıklaması
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Marka logosu URL'si
    /// </summary>
    [MaxLength(500)]
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Marka web sitesi
    /// </summary>
    [MaxLength(200)]
    public string? Website { get; set; }

    /// <summary>
    /// Marka aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Sıralama düzeni
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// Bu markaya ait ürünler
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
