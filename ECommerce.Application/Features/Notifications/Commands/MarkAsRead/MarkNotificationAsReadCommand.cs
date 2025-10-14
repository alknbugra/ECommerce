using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Results;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Notifications.Commands.MarkAsRead;

/// <summary>
/// Bildirimi okundu olarak işaretleme komutu
/// </summary>
public record MarkNotificationAsReadCommand : ICommand
{
    /// <summary>
    /// Bildirim ID'si
    /// </summary>
    public Guid NotificationId { get; init; }

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; init; }
}

/// <summary>
/// Bildirimi okundu olarak işaretleme komut handler'ı
/// </summary>
public class MarkNotificationAsReadCommandHandler : ICommandHandler<MarkNotificationAsReadCommand>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<MarkNotificationAsReadCommandHandler> _logger;

    public MarkNotificationAsReadCommandHandler(
        INotificationService notificationService,
        ILogger<MarkNotificationAsReadCommandHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<Result> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _notificationService.MarkNotificationAsReadAsync(request.NotificationId, request.UserId, cancellationToken);

            _logger.LogInformation("Bildirim okundu olarak işaretlendi. NotificationId: {NotificationId}, UserId: {UserId}",
                request.NotificationId, request.UserId);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bildirim okundu olarak işaretlenirken hata oluştu. NotificationId: {NotificationId}",
                request.NotificationId);
            return Result.Failure(Error.Problem("Notification.MarkAsReadError", "Bildirim okundu olarak işaretlenirken hata oluştu."));
        }
    }
}
