using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Features.Wishlists.Commands.CreateWishlist;
using ECommerce.Application.Features.Wishlists.Commands.AddToWishlist;
using ECommerce.Application.Features.Wishlists.Commands.RemoveFromWishlist;
using ECommerce.Application.Features.Wishlists.Queries.GetUserWishlists;
using ECommerce.Application.Features.Wishlists.Queries.GetWishlistStats;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Behaviors;
using ECommerce.Application.DTOs;
using ECommerce.Infrastructure.Services.Wishlist;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FluentValidation;

namespace ECommerce.Infrastructure.Configuration;

/// <summary>
/// Favori liste servisleri yapılandırması
/// </summary>
public static class WishlistConfiguration
{
    /// <summary>
    /// Favori liste servislerini ekle
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddWishlistServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Favori liste servisi
        services.AddScoped<IWishlistService, WishlistService>();

        // Scrutor ile otomatik handler kayıtları (sadece Wishlist namespace'i için)
        services.Scan(scan => scan
            .FromAssemblyOf<CreateWishlistCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<CreateWishlistCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Validator'ları kaydet
        services.AddScoped<IValidator<CreateWishlistCommand>, CreateWishlistCommandValidator>();
        services.AddScoped<IValidator<AddToWishlistCommand>, AddToWishlistCommandValidator>();
        services.AddScoped<IValidator<RemoveFromWishlistCommand>, RemoveFromWishlistCommandValidator>();
        services.AddScoped<IValidator<GetUserWishlistsQuery>, GetUserWishlistsQueryValidator>();
        services.AddScoped<IValidator<GetWishlistStatsQuery>, GetWishlistStatsQueryValidator>();

        return services;
    }
}
