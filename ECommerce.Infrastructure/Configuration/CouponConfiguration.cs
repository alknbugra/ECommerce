using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure.Services.Coupon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Configuration;

/// <summary>
/// Kupon servisleri konfigürasyonu
/// </summary>
public static class CouponConfiguration
{
    /// <summary>
    /// Kupon servislerini DI container'a ekle
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddCouponServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Kupon servisi
        services.AddScoped<ICouponService, CouponService>();

        return services;
    }
}
