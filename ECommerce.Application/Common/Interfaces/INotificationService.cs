using ECommerce.Application.DTOs;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Bildirim servisi interface'i
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Yeni bir bildirim oluşturur ve kaydeder.
    /// </summary>
    Task<Guid> CreateNotificationAsync(
        Guid userId,
        string title,
        string content,
        NotificationType type = NotificationType.System,
        NotificationPriority priority = NotificationPriority.Normal,
        string? relatedEntityType = null,
        Guid? relatedEntityId = null,
        string? data = null,
        DateTime? expiresAt = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Bir şablondan bildirim oluşturur ve kaydeder.
    /// </summary>
    Task<Guid> CreateNotificationFromTemplateAsync(
        Guid userId,
        string templateCode,
        Dictionary<string, string> variables,
        string? relatedEntityType = null,
        Guid? relatedEntityId = null,
        string? data = null,
        DateTime? expiresAt = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcının bildirimlerini getirir.
    /// </summary>
    Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcının okunmamış bildirim sayısını getirir.
    /// </summary>
    Task<int> GetUnreadNotificationCountAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bir bildirimi okundu olarak işaretler.
    /// </summary>
    Task<bool> MarkNotificationAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tüm bildirimleri okundu olarak işaretler.
    /// </summary>
    Task<bool> MarkAllNotificationsAsReadAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// SignalR üzerinden belirli bir kullanıcıya bildirim gönderir.
    /// </summary>
    Task SendNotificationToUserViaSignalRAsync(Guid userId, NotificationDto notification, CancellationToken cancellationToken = default);
}