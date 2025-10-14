using AspNetCoreRateLimit;
using ECommerce.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RateLimitRule = AspNetCoreRateLimit.RateLimitRule;

namespace ECommerce.API.Common.Extensions;

/// <summary>
/// Rate limiting extension metodları
/// </summary>
public static class RateLimitingExtensions
{
    /// <summary>
    /// Rate limiting servislerini ekler
    /// </summary>
    public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var rateLimitConfig = configuration.GetSection(RateLimitingConfiguration.SectionName).Get<RateLimitingConfiguration>();
        
        if (rateLimitConfig?.EnableRateLimiting != true)
        {
            return services;
        }

        // Memory cache ekle (rate limiting için gerekli)
        services.AddMemoryCache();

        // Rate limiting servislerini ekle
        services.Configure<IpRateLimitOptions>(options =>
        {
            // Genel kurallar
            options.GeneralRules = rateLimitConfig.GeneralRules.Select(rule => new RateLimitRule
            {
                Endpoint = rule.Endpoint,
                Period = rule.Period,
                Limit = rule.Limit
            }).ToList();

            // Kimlik doğrulama kuralları
            var authRules = rateLimitConfig.AuthRules.Select(rule => new RateLimitRule
            {
                Endpoint = rule.Endpoint,
                Period = rule.Period,
                Limit = rule.Limit
            }).ToList();

            // API kuralları
            var apiRules = rateLimitConfig.ApiRules.Select(rule => new RateLimitRule
            {
                Endpoint = rule.Endpoint,
                Period = rule.Period,
                Limit = rule.Limit
            }).ToList();

            // Tüm kuralları birleştir
            options.GeneralRules.AddRange(authRules);
            options.GeneralRules.AddRange(apiRules);

            // Rate limiting ayarları
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = 429; // Too Many Requests
            options.RealIpHeader = "X-Real-IP";
            options.ClientIdHeader = "X-ClientId";
        });

        // Client rate limiting (opsiyonel)
        services.Configure<ClientRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = 429;
        });

        // Rate limiting servislerini kaydet
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        return services;
    }

    /// <summary>
    /// Rate limiting middleware'ini ekler
    /// </summary>
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder app)
    {
        var rateLimitConfig = app.ApplicationServices.GetRequiredService<IConfiguration>()
            .GetSection(RateLimitingConfiguration.SectionName).Get<RateLimitingConfiguration>();

        if (rateLimitConfig?.EnableRateLimiting != true)
        {
            return app;
        }

        // IP rate limiting middleware
        app.UseIpRateLimiting();

        // Client rate limiting middleware (opsiyonel)
        // app.UseClientRateLimiting();

        return app;
    }
}
