using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Değerlendirme resmi entity'si
/// </summary>
public class ReviewImage : BaseEntity
{
    /// <summary>
    /// Değerlendirme ID'si
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Resim dosya adı
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Resim URL'si
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Resim boyutu (byte)
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Resim türü
    /// </summary>
    [MaxLength(50)]
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Resim sırası
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// Resim onaylandı mı?
    /// </summary>
    public bool IsApproved { get; set; } = false;

    /// <summary>
    /// Değerlendirme
    /// </summary>
    public virtual ProductReview Review { get; set; } = null!;
}
