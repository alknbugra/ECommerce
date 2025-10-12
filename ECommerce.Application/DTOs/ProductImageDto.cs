namespace ECommerce.Application.DTOs;

/// <summary>
/// Ürün resmi DTO'su
/// </summary>
public class ProductImageDto
{
    /// <summary>
    /// Resim ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Resim URL'si
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Resim açıklaması
    /// </summary>
    public string? AltText { get; set; }

    /// <summary>
    /// Resim açıklaması (Description)
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Sıralama düzeni
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Ana resim mi?
    /// </summary>
    public bool IsMain { get; set; }

    /// <summary>
    /// Ana resim mi? (IsMainImage)
    /// </summary>
    public bool IsMainImage { get; set; }

    /// <summary>
    /// Dosya adı
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Dosya boyutu (byte)
    /// </summary>
    public long? FileSize { get; set; }

    /// <summary>
    /// Dosya tipi (MIME type)
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
