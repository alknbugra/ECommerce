using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Değerlendirme oyu entity'si
/// </summary>
public class ReviewVote : BaseEntity
{
    /// <summary>
    /// Değerlendirme ID'si
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Oy veren kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Oy türü
    /// </summary>
    [MaxLength(10)]
    public string VoteType { get; set; } = string.Empty; // Helpful, NotHelpful

    /// <summary>
    /// Kullanıcı IP adresi
    /// </summary>
    [MaxLength(45)]
    public string? UserIpAddress { get; set; }

    /// <summary>
    /// Değerlendirme
    /// </summary>
    public virtual ProductReview Review { get; set; } = null!;

    /// <summary>
    /// Oy veren kullanıcı
    /// </summary>
    public virtual User User { get; set; } = null!;
}
