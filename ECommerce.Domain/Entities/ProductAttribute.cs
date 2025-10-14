using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Ürün özelliği entity'si (renk, beden, materyal vb.)
/// </summary>
public class ProductAttribute : BaseEntity
{
    /// <summary>
    /// Özellik adı
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Özellik açıklaması
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Özellik tipi (Color, Size, Material, Brand vb.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Özellik aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Sıralama düzeni
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// Bu özelliğe sahip varyantlar
    /// </summary>
    public virtual ICollection<ProductVariantAttribute> VariantAttributes { get; set; } = new List<ProductVariantAttribute>();

    /// <summary>
    /// Bu özelliğe sahip ürünler
    /// </summary>
    public virtual ICollection<ProductProductAttribute> ProductAttributes { get; set; } = new List<ProductProductAttribute>();
}
