using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Ürün resmi entity'si
/// </summary>
public class ProductImage : BaseEntity
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Resim URL'si
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Resim açıklaması
    /// </summary>
    [MaxLength(200)]
    public string? AltText { get; set; }

    /// <summary>
    /// Resim açıklaması (Description)
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Sıralama düzeni
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// Ana resim mi?
    /// </summary>
    public bool IsMain { get; set; } = false;

    /// <summary>
    /// Ana resim mi? (IsMainImage)
    /// </summary>
    public bool IsMainImage { get; set; } = false;

    /// <summary>
    /// Dosya adı
    /// </summary>
    [MaxLength(255)]
    public string? FileName { get; set; }

    /// <summary>
    /// Dosya boyutu (byte)
    /// </summary>
    public long? FileSize { get; set; }

    /// <summary>
    /// Dosya tipi (MIME type)
    /// </summary>
    [MaxLength(100)]
    public string? ContentType { get; set; }

    /// <summary>
    /// Ürün
    /// </summary>
    public virtual Product Product { get; set; } = null!;
}
