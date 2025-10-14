using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs;

/// <summary>
/// Bildirim DTO'su
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// Bildirim ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Bildirim başlığı
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Bildirim içeriği
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Bildirim türü
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Bildirim önceliği
    /// </summary>
    public NotificationPriority Priority { get; set; }

    /// <summary>
    /// Okundu mu?
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Okunma tarihi
    /// </summary>
    public DateTime? ReadAt { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Bildirim verisi
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// İlgili entity ID'si
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// İlgili entity türü
    /// </summary>
    public string? RelatedEntityType { get; set; }

    /// <summary>
    /// Bildirim süresi
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}
