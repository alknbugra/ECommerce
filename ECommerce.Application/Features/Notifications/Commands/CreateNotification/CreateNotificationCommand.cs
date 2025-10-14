using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Results;
using ECommerce.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Notifications.Commands.CreateNotification;

/// <summary>
/// Bildirim oluşturma komutu
/// </summary>
public record CreateNotificationCommand : ICommand<Guid>
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Bildirim başlığı
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Bildirim içeriği
    /// </summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>
    /// Bildirim türü
    /// </summary>
    public Domain.Enums.NotificationType Type { get; init; }

    /// <summary>
    /// Bildirim önceliği
    /// </summary>
    public Domain.Enums.NotificationPriority Priority { get; init; } = Domain.Enums.NotificationPriority.Normal;

    /// <summary>
    /// Bildirim verisi
    /// </summary>
    public string? Data { get; init; }

    /// <summary>
    /// İlgili entity ID'si
    /// </summary>
    public Guid? RelatedEntityId { get; init; }

    /// <summary>
    /// İlgili entity türü
    /// </summary>
    public string? RelatedEntityType { get; init; }

    /// <summary>
    /// Bildirim süresi
    /// </summary>
    public DateTime? ExpiresAt { get; init; }
}

/// <summary>
/// Bildirim oluşturma komut handler'ı
/// </summary>
public class CreateNotificationCommandHandler : ICommandHandler<CreateNotificationCommand, Guid>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<CreateNotificationCommandHandler> _logger;

    public CreateNotificationCommandHandler(
        INotificationService notificationService,
        ILogger<CreateNotificationCommandHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var notificationId = await _notificationService.CreateNotificationAsync(
                request.UserId,
                request.Title,
                request.Content,
                request.Type,
                request.Priority,
                request.RelatedEntityType,
                request.RelatedEntityId,
                request.Data,
                request.ExpiresAt,
                cancellationToken);

            _logger.LogInformation("Bildirim başarıyla oluşturuldu. UserId: {UserId}, Type: {Type}",
                request.UserId, request.Type);

            return Result<Guid>.Success(notificationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bildirim oluşturulurken hata oluştu. UserId: {UserId}", request.UserId);
            return Result.Failure<Guid>(Error.Problem("Notification.CreateError", "Bildirim oluşturulurken hata oluştu."));
        }
    }
}
