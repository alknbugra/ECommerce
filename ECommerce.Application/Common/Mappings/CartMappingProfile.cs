using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Mappings;

/// <summary>
/// Sepet mapping profili
/// </summary>
public class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        // Cart -> CartDto
        CreateMap<Cart, CartDto>()
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.IsEmpty, opt => opt.MapFrom(src => src.IsEmpty));

        // CartItem -> CartItemDto
        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ProductSku, opt => opt.MapFrom(src => src.Product.Sku))
            .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.MainImageUrl))
            .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(src => src.Product.InStock))
            .ForMember(dest => dest.AvailableStock, opt => opt.MapFrom(src => src.Product.StockQuantity));
    }
}
