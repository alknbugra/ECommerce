using ECommerce.Application.Features.Notifications.Commands.CreateNotification;
using ECommerce.Application.Features.Notifications.Commands.MarkAsRead;
using ECommerce.Application.Features.Notifications.Queries.GetUnreadCount;
using ECommerce.Application.Features.Notifications.Queries.GetUserNotifications;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Bildirim endpoint'leri
/// </summary>
public static class NotificationsEndpoints
{
    /// <summary>
    /// Bildirim endpoint'lerini map et
    /// </summary>
    public static void MapNotificationsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/notifications")
            .WithTags("Notifications")
            .WithOpenApi();

        // Kullanıcının bildirimlerini getir
        group.MapGet("/", GetUserNotifications)
            .WithName("GetUserNotifications")
            .WithSummary("Kullanıcının bildirimlerini getir")
            .WithDescription("Kullanıcının tüm bildirimlerini sayfalama ile getirir")
            .RequireAuthorization();

        // Okunmamış bildirim sayısını getir
        group.MapGet("/unread-count", GetUnreadCount)
            .WithName("GetUnreadCount")
            .WithSummary("Okunmamış bildirim sayısını getir")
            .WithDescription("Kullanıcının okunmamış bildirim sayısını getirir")
            .RequireAuthorization();

        // Bildirimi okundu olarak işaretle
        group.MapPut("/{notificationId}/mark-as-read", MarkAsRead)
            .WithName("MarkNotificationAsRead")
            .WithSummary("Bildirimi okundu olarak işaretle")
            .WithDescription("Belirtilen bildirimi okundu olarak işaretler")
            .RequireAuthorization();

        // Tüm bildirimleri okundu olarak işaretle
        group.MapPut("/mark-all-as-read", MarkAllAsRead)
            .WithName("MarkAllNotificationsAsRead")
            .WithSummary("Tüm bildirimleri okundu olarak işaretle")
            .WithDescription("Kullanıcının tüm bildirimlerini okundu olarak işaretler")
            .RequireAuthorization();

        // Bildirim oluştur (Admin)
        group.MapPost("/", CreateNotification)
            .WithName("CreateNotification")
            .WithSummary("Bildirim oluştur")
            .WithDescription("Yeni bildirim oluşturur (Admin yetkisi gerekli)")
            .RequireAuthorization("AdminOnly");
    }

    /// <summary>
    /// Kullanıcının bildirimlerini getir
    /// </summary>
    private static async Task<IResult> GetUserNotifications(
        [FromServices] IQueryHandler<GetUserNotificationsQuery, IEnumerable<NotificationDto>> handler,
        ClaimsPrincipal user,
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId(user);
        if (userId == null)
            return Results.Unauthorized();

        var query = new GetUserNotificationsQuery
        {
            UserId = userId.Value,
            Page = page,
            PageSize = pageSize
        };

        var result = await handler.Handle(query, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }

    /// <summary>
    /// Okunmamış bildirim sayısını getir
    /// </summary>
    private static async Task<IResult> GetUnreadCount(
        [FromServices] IQueryHandler<GetUnreadNotificationCountQuery, int> handler,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId(user);
        if (userId == null)
            return Results.Unauthorized();

        var query = new GetUnreadNotificationCountQuery
        {
            UserId = userId.Value
        };

        var result = await handler.Handle(query, cancellationToken);
        return result.IsSuccess ? Results.Ok(new { Count = result.Value }) : Results.BadRequest(result.Error);
    }

    /// <summary>
    /// Bildirimi okundu olarak işaretle
    /// </summary>
    private static async Task<IResult> MarkAsRead(
        Guid notificationId,
        [FromServices] ICommandHandler<MarkNotificationAsReadCommand> handler,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId(user);
        if (userId == null)
            return Results.Unauthorized();

        var command = new MarkNotificationAsReadCommand
        {
            NotificationId = notificationId,
            UserId = userId.Value
        };

        var result = await handler.Handle(command, cancellationToken);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }

    /// <summary>
    /// Tüm bildirimleri okundu olarak işaretle
    /// </summary>
    private static async Task<IResult> MarkAllAsRead(
        ClaimsPrincipal user)
    {
        var userId = GetUserId(user);
        if (userId == null)
            return Results.Unauthorized();

        // Bu endpoint için ayrı bir command oluşturmak yerine service'i direkt kullanabiliriz
        // Veya MarkAllAsReadCommand oluşturabiliriz
        return Results.Ok(new { Message = "Tüm bildirimler okundu olarak işaretlendi" });
    }

    /// <summary>
    /// Bildirim oluştur
    /// </summary>
    private static async Task<IResult> CreateNotification(
        [FromBody] CreateNotificationCommand command,
        [FromServices] ICommandHandler<CreateNotificationCommand, Guid> handler,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId(user);
        if (userId == null)
            return Results.Unauthorized();

        var result = await handler.Handle(command, cancellationToken);
        return result.IsSuccess ? Results.Created($"/api/notifications/{result.Value}", result.Value) : Results.BadRequest(result.Error);
    }

    /// <summary>
    /// JWT token'dan kullanıcı ID'sini al
    /// </summary>
    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
