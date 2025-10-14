using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Results;
using ECommerce.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Notifications.Queries.GetUserNotifications;

/// <summary>
/// Kullanıcı bildirimlerini getirme sorgusu
/// </summary>
public record GetUserNotificationsQuery : IQuery<Result<IEnumerable<NotificationDto>>>
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Sayfa numarası
    /// </summary>
    public int Page { get; init; } = 1;

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    public int PageSize { get; init; } = 20;
}

/// <summary>
/// Kullanıcı bildirimlerini getirme sorgu handler'ı
/// </summary>
public class GetUserNotificationsQueryHandler : IQueryHandler<GetUserNotificationsQuery, IEnumerable<NotificationDto>>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<GetUserNotificationsQueryHandler> _logger;

    public GetUserNotificationsQueryHandler(
        INotificationService notificationService,
        ILogger<GetUserNotificationsQueryHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<NotificationDto>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(
                request.UserId, request.Page, request.PageSize, cancellationToken);

            _logger.LogInformation("Kullanıcı bildirimleri getirildi. UserId: {UserId}, Count: {Count}",
                request.UserId, notifications.Count());

            return Result<IEnumerable<NotificationDto>>.Success(notifications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı bildirimleri getirilirken hata oluştu. UserId: {UserId}", request.UserId);
            return Result.Failure<IEnumerable<NotificationDto>>(Error.Problem("Notification.GetError", "Bildirimler getirilirken hata oluştu."));
        }
    }
}
