using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure.Services.Inventory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Configuration;

/// <summary>
/// Stok yapılandırması
/// </summary>
public static class InventoryConfiguration
{
    /// <summary>
    /// Stok servislerini kaydet
    /// </summary>
    public static IServiceCollection AddInventoryServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Stok servislerini kaydet
        services.AddScoped<IInventoryService, InventoryService>();

        return services;
    }
}
