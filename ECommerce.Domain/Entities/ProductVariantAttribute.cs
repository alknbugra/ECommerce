using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Ürün varyantı özellik değeri entity'si
/// </summary>
public class ProductVariantAttribute : BaseEntity
{
    /// <summary>
    /// Varyant ID'si
    /// </summary>
    public Guid ProductVariantId { get; set; }

    /// <summary>
    /// Varyant
    /// </summary>
    public virtual ProductVariant ProductVariant { get; set; } = null!;

    /// <summary>
    /// Özellik ID'si
    /// </summary>
    public Guid ProductAttributeId { get; set; }

    /// <summary>
    /// Özellik
    /// </summary>
    public virtual ProductAttribute ProductAttribute { get; set; } = null!;

    /// <summary>
    /// Özellik değeri
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Ek bilgi (hex renk kodu, beden numarası vb.)
    /// </summary>
    [MaxLength(100)]
    public string? AdditionalInfo { get; set; }
}
