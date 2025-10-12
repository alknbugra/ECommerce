using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Mappings;

/// <summary>
/// AutoMapper profili
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.Images, opt => opt.Ignore());

        CreateMap<ProductDto, Product>();

        // ProductImage mappings
        CreateMap<ProductImage, ProductImageDto>();
        CreateMap<ProductImageDto, ProductImage>();

        // Category mappings
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null))
            .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
            .ForMember(dest => dest.ProductCount, opt => opt.Ignore());

        CreateMap<CategoryDto, Category>();

        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

        CreateMap<UserDto, User>();

        // Address mappings
        CreateMap<Address, AddressDto>();
        CreateMap<AddressDto, Address>();

        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty))
            .ForMember(dest => dest.ShippingAddress, opt => opt.Ignore())
            .ForMember(dest => dest.BillingAddress, opt => opt.Ignore())
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
            .ForMember(dest => dest.StatusHistory, opt => opt.Ignore());

        CreateMap<OrderDto, Order>();

        // OrderItem mappings
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<OrderItemDto, OrderItem>();

        // OrderStatusHistory mappings
        CreateMap<OrderStatusHistory, OrderStatusHistoryDto>();
        CreateMap<OrderStatusHistoryDto, OrderStatusHistory>();

        // Role mappings
        CreateMap<Role, string>().ConvertUsing(src => src.Name);
        CreateMap<string, Role>().ConvertUsing(src => new Role { Name = src });

        // UserRole mappings
        CreateMap<UserRole, string>().ConvertUsing(src => src.Role != null ? src.Role.Name : string.Empty);
    }
}
