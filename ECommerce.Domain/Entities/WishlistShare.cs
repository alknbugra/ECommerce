using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Favori liste paylaşımı entity'si
/// </summary>
public class WishlistShare : BaseEntity
{
    /// <summary>
    /// Favori liste ID'si
    /// </summary>
    public Guid WishlistId { get; set; }
    /// Paylaşılan kullanıcı ID'si (opsiyonel)
    /// </summary>
    public Guid? SharedWithUserId { get; set; }

    /// <summary>
    /// Paylaşım türü
    /// </summary>
    [MaxLength(20)]
    public string ShareType { get; set; } = "Public"; // Public, Private, Email

    /// <summary>
    /// Paylaşım kodu
    /// </summary>
    [MaxLength(50)]
    public string ShareCode { get; set; } = string.Empty;

    /// <summary>
    /// E-posta adresi (e-posta paylaşımı için)
    /// </summary>
    [MaxLength(255)]
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Paylaşım mesajı
    /// </summary>
    [MaxLength(500)]
    public string? Message { get; set; }

    /// <summary>
    /// Paylaşım süresi (gün)
    /// </summary>
    public int? ExpirationDays { get; set; }

    /// <summary>
    /// Paylaşım sona erme tarihi
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Paylaşım aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Görüntülenme sayısı
    /// </summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// Son görüntülenme tarihi
    /// </summary>
    public DateTime? LastViewedAt { get; set; }

    /// <summary>
    /// Favori liste
    /// </summary>
    public virtual Wishlist Wishlist { get; set; } = null!;

    /// <summary>
    /// Paylaşılan kullanıcı
    /// </summary>
    public virtual User? SharedWithUser { get; set; }

    /// <summary>
    /// Paylaşım süresi doldu mu?
    /// </summary>
    [NotMapped]
    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;

    /// <summary>
    /// Paylaşım geçerli mi?
    /// </summary>
    [NotMapped]
    public bool IsValid => IsActive && !IsExpired && !IsDeleted;
}
