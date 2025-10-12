using ECommerce.Application.Common.Decorators;
using ECommerce.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace ECommerce.Application.Common.Extensions;

/// <summary>
/// Service collection extension metodları
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Handler'ları otomatik olarak kaydet ve decorator'ları uygula
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddHandlersWithDecorators(this IServiceCollection services)
    {
        // Handler'ları otomatik olarak kaydet
        services.Scan(scan => scan
            .FromAssemblyOf<IHandler<object, object>>()
            .AddClasses(classes => classes
                .Where(type => type.IsClass && 
                              !type.IsAbstract && 
                              type.GetInterfaces().Any(i => 
                                  i.IsGenericType && 
                                  (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                                   i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) ||
                                   i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                                   i.GetGenericTypeDefinition() == typeof(IHandler<,>) ||
                                   i.GetGenericTypeDefinition() == typeof(IHandler<>)))))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
