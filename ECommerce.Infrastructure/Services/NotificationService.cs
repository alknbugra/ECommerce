using AutoMapper;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Infrastructure.Services;

/// <summary>
/// Bildirim servisi implementasyonu
/// </summary>
public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ECommerceDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<NotificationService> _logger;
    
    public NotificationService(
        IUnitOfWork unitOfWork,
        ECommerceDbContext context,
        IMapper mapper,
        ILogger<NotificationService> logger)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Guid> CreateNotificationAsync(
        Guid userId,
        string title,
        string content,
        NotificationType type = NotificationType.System,
        NotificationPriority priority = NotificationPriority.Normal,
        string? relatedEntityType = null,
        Guid? relatedEntityId = null,
        string? data = null,
        DateTime? expiresAt = null,
        CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Content = content,
            Type = type,
            Priority = priority,
            RelatedEntityType = relatedEntityType,
            RelatedEntityId = relatedEntityId,
            Data = data,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        await _unitOfWork.Notifications.AddAsync(notification);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Yeni bildirim oluşturuldu. UserId: {UserId}, Title: {Title}", userId, title);
        return notification.Id;
    }

    public async Task<Guid> CreateNotificationFromTemplateAsync(
        Guid userId,
        string templateCode,
        Dictionary<string, string> variables,
        string? relatedEntityType = null,
        Guid? relatedEntityId = null,
        string? data = null,
        DateTime? expiresAt = null,
        CancellationToken cancellationToken = default)
    {
        var template = await _context.NotificationTemplates
            .FirstOrDefaultAsync(t => t.Code == templateCode && t.IsActive, cancellationToken);

        if (template == null)
        {
            _logger.LogWarning("Bildirim şablonu bulunamadı: {TemplateCode}", templateCode);
            throw new InvalidOperationException($"Bildirim şablonu bulunamadı: {templateCode}");
        }

        var title = ReplaceVariables(template.TitleTemplate, variables);
        var content = ReplaceVariables(template.ContentTemplate, variables);

        return await CreateNotificationAsync(
            userId,
            title,
            content,
            template.Type,
            template.DefaultPriority,
            relatedEntityType,
            relatedEntityId,
            data,
            expiresAt,
            cancellationToken);
    }

    public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
    }

    public async Task<int> GetUnreadNotificationCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);
    }

    public async Task<bool> MarkNotificationAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId, cancellationToken);

        if (notification == null)
        {
            _logger.LogWarning("Bildirim bulunamadı veya kullanıcıya ait değil. NotificationId: {NotificationId}, UserId: {UserId}", notificationId, userId);
            return false;
        }

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _unitOfWork.Notifications.UpdateAsync(notification);
            await _unitOfWork.CompleteAsync(cancellationToken);
            _logger.LogInformation("Bildirim okundu olarak işaretlendi. NotificationId: {NotificationId}", notificationId);
        }
        return true;
    }

    public async Task<bool> MarkAllNotificationsAsReadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var unreadNotifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _unitOfWork.Notifications.UpdateAsync(notification);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
        _logger.LogInformation("{Count} bildirim okundu olarak işaretlendi. UserId: {UserId}", unreadNotifications.Count, userId);
        return true;
    }

    public async Task SendNotificationToUserViaSignalRAsync(Guid userId, NotificationDto notification, CancellationToken cancellationToken = default)
    {
        // SignalR implementasyonu API katmanında yapılacak
        _logger.LogInformation("SignalR bildirim gönderimi için hazır. UserId: {UserId}, Title: {Title}", userId, notification.Title);
        await Task.CompletedTask;
    }

    private string ReplaceVariables(string template, Dictionary<string, string> variables)
    {
        foreach (var entry in variables)
        {
            template = template.Replace($"{{{entry.Key}}}", entry.Value);
        }
        return template;
    }
}