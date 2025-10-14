using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ECommerce.API.Hubs;

/// <summary>
/// Bildirim SignalR Hub'ı
/// </summary>
[Authorize]
public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Kullanıcı bağlandığında çalışır
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        if (userId.HasValue)
        {
            // Kullanıcıyı kendi grubuna ekle
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            
            _logger.LogInformation("Kullanıcı {UserId} bildirim hub'ına bağlandı. ConnectionId: {ConnectionId}", 
                userId, Context.ConnectionId);
        }

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Kullanıcı bağlantısı kesildiğinde çalışır
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        if (userId.HasValue)
        {
            // Kullanıcıyı kendi grubundan çıkar
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
            
            _logger.LogInformation("Kullanıcı {UserId} bildirim hub'ından ayrıldı. ConnectionId: {ConnectionId}", 
                userId, Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Belirli bir kullanıcıya bildirim gönder
    /// </summary>
    public async Task SendToUser(Guid userId, string method, object data)
    {
        await Clients.Group($"User_{userId}").SendAsync(method, data);
    }

    /// <summary>
    /// Tüm kullanıcılara bildirim gönder
    /// </summary>
    public async Task SendToAll(string method, object data)
    {
        await Clients.All.SendAsync(method, data);
    }

    /// <summary>
    /// Belirli bir gruba bildirim gönder
    /// </summary>
    public async Task SendToGroup(string groupName, string method, object data)
    {
        await Clients.Group(groupName).SendAsync(method, data);
    }

    /// <summary>
    /// JWT token'dan kullanıcı ID'sini al
    /// </summary>
    private Guid? GetUserId()
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }
        return null;
    }
}
