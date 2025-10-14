using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure.Services.PaymentGateway;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace ECommerce.Infrastructure.Configuration;

/// <summary>
/// Payment Gateway yapılandırması
/// </summary>
public static class PaymentGatewayConfiguration
{
    /// <summary>
    /// Payment Gateway servislerini kaydet
    /// </summary>
    public static IServiceCollection AddPaymentGatewayServices(this IServiceCollection services, IConfiguration configuration)
    {
        // HttpClient'ı kaydet
        services.AddHttpClient<IPaymentGatewayService, IyzicoPaymentGatewayService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Iyzico Payment Gateway servisini kaydet
        services.AddScoped<IPaymentGatewayService, IyzicoPaymentGatewayService>();

        return services;
    }
}
