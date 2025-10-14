using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure.Services.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Configuration;

/// <summary>
/// Email yapılandırması
/// </summary>
public static class EmailConfiguration
{
    /// <summary>
    /// Email servislerini kaydet
    /// </summary>
    public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Email servislerini kaydet
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
