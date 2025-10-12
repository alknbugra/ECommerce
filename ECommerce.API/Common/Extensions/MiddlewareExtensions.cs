using ECommerce.API.Common.Middleware;

namespace ECommerce.API.Common.Extensions;

/// <summary>
/// Middleware extension metodları
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Global exception handling middleware'ini ekle
    /// </summary>
    /// <param name="app">Web application</param>
    /// <returns>Web application</returns>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
