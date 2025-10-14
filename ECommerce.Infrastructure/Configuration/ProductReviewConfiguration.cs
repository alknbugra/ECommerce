using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure.Services.ProductReview;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Configuration;

/// <summary>
/// Ürün değerlendirme servisleri konfigürasyonu
/// </summary>
public static class ProductReviewConfiguration
{
    /// <summary>
    /// Ürün değerlendirme servislerini DI container'a ekle
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddProductReviewServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Ürün değerlendirme servisi
        services.AddScoped<IProductReviewService, ProductReviewService>();

        return services;
    }
}
