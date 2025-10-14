using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Bildirim entity'si
/// </summary>
public class Notification : BaseEntity
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Bildirim başlığı
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Bildirim içeriği
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Bildirim türü
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Bildirim önceliği
    /// </summary>
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;

    /// <summary>
    /// Okundu mu?
    /// </summary>
    public bool IsRead { get; set; } = false;

    /// <summary>
    /// Okunma tarihi
    /// </summary>
    public DateTime? ReadAt { get; set; }

    /// <summary>
    /// Bildirim verisi (JSON formatında)
    /// </summary>
    [MaxLength(2000)]
    public string? Data { get; set; }

    /// <summary>
    /// İlgili entity ID'si (sipariş, ürün vb.)
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// İlgili entity türü
    /// </summary>
    [MaxLength(50)]
    public string? RelatedEntityType { get; set; }

    /// <summary>
    /// Bildirim süresi (ne kadar süre sonra silinecek)
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Kullanıcı
    /// </summary>
    public virtual User User { get; set; } = null!;
}
