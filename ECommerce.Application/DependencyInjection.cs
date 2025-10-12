using ECommerce.Application.Common.Mappings;
using ECommerce.Application.Common.Extensions;
using ECommerce.Application.Common.Interfaces;
using FluentValidation;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Products.Commands.CreateProduct;
using ECommerce.Application.Features.Products.Queries.GetProductById;
using ECommerce.Application.Features.Products.Queries.GetProducts;
using ECommerce.Application.Features.Auth.Commands.Login;
using ECommerce.Application.Features.Auth.Commands.Register;
using ECommerce.Application.Features.Auth.Commands.RefreshToken;
using ECommerce.Application.Features.Orders.Commands.CreateOrder;
using ECommerce.Application.Features.Orders.Commands.UpdateOrderStatus;
using ECommerce.Application.Features.Orders.Queries.GetOrderById;
using ECommerce.Application.Features.Orders.Queries.GetOrders;
using ECommerce.Application.Features.Users.Commands.UpdateUserProfile;
using ECommerce.Application.Features.Users.Commands.ChangePassword;
using ECommerce.Application.Features.Users.Commands.UpdateUserStatus;
using ECommerce.Application.Features.Users.Queries.GetUserById;
using ECommerce.Application.Features.Users.Queries.GetUsers;
using ECommerce.Application.Features.Products.Commands.UploadProductImage;
using ECommerce.Application.Features.Products.Commands.DeleteProductImage;
using ECommerce.Application.Features.Categories.Commands.CreateCategory;
using ECommerce.Application.Features.Categories.Commands.UpdateCategory;
using ECommerce.Application.Features.Categories.Commands.DeleteCategory;
using ECommerce.Application.Features.Categories.Queries.GetCategoryById;
using ECommerce.Application.Features.Categories.Queries.GetCategories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.Application;

/// <summary>
/// Application katmanı için dependency injection yapılandırması
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Application servislerini ekle
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper ekle
        services.AddAutoMapper(cfg => {
            cfg.AddProfile<MappingProfile>();
        });

        // FluentValidation ekle
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Cache servisini ekle
        services.AddMemoryCache();
        // ICacheService implementasyonu Infrastructure katmanında olacak

        // Handler'ları manuel olarak kaydet
        services.AddScoped<ICommandHandler<CreateProductCommand, ProductDto>, CreateProductCommandHandler>();
        services.AddScoped<IQueryHandler<GetProductByIdQuery, ProductDto?>, GetProductByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetProductsQuery, List<ProductDto>>, GetProductsQueryHandler>();
        
        // Auth handler'ları
        services.AddScoped<ICommandHandler<LoginCommand, AuthResponseDto>, LoginCommandHandler>();
        services.AddScoped<ICommandHandler<RegisterCommand, AuthResponseDto>, RegisterCommandHandler>();
        services.AddScoped<ICommandHandler<RefreshTokenCommand, AuthResponseDto>, RefreshTokenCommandHandler>();

        // Order handler'ları
        services.AddScoped<ICommandHandler<CreateOrderCommand, OrderDto>, CreateOrderCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateOrderStatusCommand, OrderDto>, UpdateOrderStatusCommandHandler>();
        services.AddScoped<IQueryHandler<GetOrderByIdQuery, OrderDto?>, GetOrderByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetOrdersQuery, List<OrderDto>>, GetOrdersQueryHandler>();

        // User handler'ları
        services.AddScoped<ICommandHandler<UpdateUserProfileCommand, UserDto>, UpdateUserProfileCommandHandler>();
        services.AddScoped<ICommandHandler<ChangePasswordCommand, bool>, ChangePasswordCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserStatusCommand, UserDto>, UpdateUserStatusCommandHandler>();
        services.AddScoped<IQueryHandler<GetUserByIdQuery, UserDto?>, GetUserByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetUsersQuery, List<UserDto>>, GetUsersQueryHandler>();

        // File Upload handler'ları
        services.AddScoped<ICommandHandler<UploadProductImageCommand, ProductImageDto>, UploadProductImageCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteProductImageCommand, bool>, DeleteProductImageCommandHandler>();
        
        // Category handler'ları
        services.AddScoped<ICommandHandler<CreateCategoryCommand, CategoryDto>, CreateCategoryCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateCategoryCommand, CategoryDto>, UpdateCategoryCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteCategoryCommand, bool>, DeleteCategoryCommandHandler>();
        services.AddScoped<IQueryHandler<GetCategoryByIdQuery, CategoryDto?>, GetCategoryByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetCategoriesQuery, List<CategoryDto>>, GetCategoriesQueryHandler>();

        return services;
    }
}
