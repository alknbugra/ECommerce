using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Results;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Notifications.Queries.GetUnreadCount;

/// <summary>
/// Okunmamış bildirim sayısını getirme sorgusu
/// </summary>
public record GetUnreadNotificationCountQuery : IQuery<Result<int>>
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; init; }
}

/// <summary>
/// Okunmamış bildirim sayısını getirme sorgu handler'ı
/// </summary>
public class GetUnreadNotificationCountQueryHandler : IQueryHandler<GetUnreadNotificationCountQuery, int>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<GetUnreadNotificationCountQueryHandler> _logger;

    public GetUnreadNotificationCountQueryHandler(
        INotificationService notificationService,
        ILogger<GetUnreadNotificationCountQueryHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(GetUnreadNotificationCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var count = await _notificationService.GetUnreadNotificationCountAsync(request.UserId, cancellationToken);

            _logger.LogInformation("Okunmamış bildirim sayısı getirildi. UserId: {UserId}, Count: {Count}",
                request.UserId, count);

            return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Okunmamış bildirim sayısı getirilirken hata oluştu. UserId: {UserId}", request.UserId);
            return Result.Failure<int>(Error.Problem("Notification.GetCountError", "Okunmamış bildirim sayısı getirilirken hata oluştu."));
        }
    }
}
