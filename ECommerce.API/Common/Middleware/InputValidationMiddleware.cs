using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Common.Middleware;

/// <summary>
/// Input validation middleware
/// </summary>
public class InputValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<InputValidationMiddleware> _logger;

    public InputValidationMiddleware(RequestDelegate next, ILogger<InputValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Sadece POST, PUT, PATCH istekleri için validation yap
        if (context.Request.Method is "POST" or "PUT" or "PATCH")
        {
            // Request body'yi oku
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (!string.IsNullOrEmpty(body))
            {
                try
                {
                    // JSON validation
                    if (context.Request.ContentType?.Contains("application/json") == true)
                    {
                        JsonDocument.Parse(body);
                    }

                    // XSS protection - tehlikeli karakterleri kontrol et
                    if (ContainsDangerousContent(body))
                    {
                        _logger.LogWarning("Potansiyel XSS saldırısı tespit edildi. IP: {IP}, UserAgent: {UserAgent}", 
                            context.Connection.RemoteIpAddress, 
                            context.Request.Headers.UserAgent);

                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            error = "Geçersiz içerik tespit edildi",
                            message = "İstek içeriği güvenlik politikalarına uygun değil"
                        }));
                        return;
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Geçersiz JSON formatı. IP: {IP}", context.Connection.RemoteIpAddress);
                    
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = "Geçersiz JSON formatı",
                        message = "İstek JSON formatında olmalıdır"
                    }));
                    return;
                }
            }
        }

        await _next(context);
    }

    /// <summary>
    /// Tehlikeli içerik kontrolü
    /// </summary>
    private static bool ContainsDangerousContent(string content)
    {
        var dangerousPatterns = new[]
        {
            "<script",
            "javascript:",
            "onload=",
            "onerror=",
            "onclick=",
            "onmouseover=",
            "vbscript:",
            "data:text/html",
            "expression(",
            "url(",
            "eval(",
            "document.cookie",
            "document.write",
            "window.location",
            "alert(",
            "confirm(",
            "prompt("
        };

        var lowerContent = content.ToLowerInvariant();
        return dangerousPatterns.Any(pattern => lowerContent.Contains(pattern));
    }
}
