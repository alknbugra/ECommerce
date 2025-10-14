namespace ECommerce.Application.DTOs;

/// <summary>
/// Değerlendirme resmi DTO'su
/// </summary>
public class ReviewImageDto
{
    /// <summary>
    /// Resim ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Değerlendirme ID'si
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Resim dosya adı
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Resim URL'si
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Resim boyutu (byte)
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Resim türü
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Resim sırası
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Resim onaylandı mı?
    /// </summary>
    public bool IsApproved { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
