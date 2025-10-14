using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Ürün özellik değeri entity'si (ürün seviyesinde özellikler)
/// </summary>
public class ProductProductAttribute : BaseEntity
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Ürün
    /// </summary>
    public virtual Product Product { get; set; } = null!;

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
    /// Ek bilgi
    /// </summary>
    [MaxLength(100)]
    public string? AdditionalInfo { get; set; }
}
