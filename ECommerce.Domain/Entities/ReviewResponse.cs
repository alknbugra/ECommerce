using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Değerlendirme yanıtı entity'si
/// </summary>
public class ReviewResponse : BaseEntity
{
    /// <summary>
    /// Değerlendirme ID'si
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Yanıtlayan kullanıcı ID'si
    /// </summary>
    public Guid RespondedByUserId { get; set; }

    /// <summary>
    /// Yanıt içeriği
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Yanıt türü
    /// </summary>
    [MaxLength(20)]
    public string ResponseType { get; set; } = "Seller"; // Seller, Admin, Customer

    /// <summary>
    /// Yanıt onaylandı mı?
    /// </summary>
    public bool IsApproved { get; set; } = false;

    /// <summary>
    /// Onaylayan kullanıcı ID'si
    /// </summary>
    public Guid? ApprovedByUserId { get; set; }

    /// <summary>
    /// Onay tarihi
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// Değerlendirme
    /// </summary>
    public virtual ProductReview Review { get; set; } = null!;

    /// <summary>
    /// Yanıtlayan kullanıcı
    /// </summary>
    public virtual User RespondedByUser { get; set; } = null!;

    /// <summary>
    /// Onaylayan kullanıcı
    /// </summary>
    public virtual User? ApprovedByUser { get; set; }
}
