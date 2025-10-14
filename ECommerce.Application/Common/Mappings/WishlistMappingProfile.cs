using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Mappings;

/// <summary>
/// Favori liste AutoMapper profili
/// </summary>
public class WishlistMappingProfile : Profile
{
    public WishlistMappingProfile()
    {
        // Wishlist entity'den DTO'ya mapping
        CreateMap<Wishlist, WishlistDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => !src.IsDeleted))
            .ForMember(dest => dest.IsShared, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ShareCode)))
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.WishlistItems.Count(wi => !wi.IsDeleted)));

        // CreateWishlistDto'dan Wishlist entity'ye mapping
        CreateMap<CreateWishlistDto, Wishlist>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.WishlistItems, opt => opt.Ignore())
            .ForMember(dest => dest.WishlistShares, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        // WishlistItem entity'den DTO'ya mapping
        CreateMap<WishlistItem, WishlistItemDto>()
            .ForMember(dest => dest.IsProductActive, opt => opt.MapFrom(src => src.Product != null && src.Product.IsActive))
            .ForMember(dest => dest.HasPriceDropped, opt => opt.MapFrom(src => src.Product != null && src.Product.Price < src.PriceAtTime))
            .ForMember(dest => dest.HasReachedTargetPrice, opt => opt.MapFrom(src => src.TargetPrice.HasValue && src.Product != null && src.Product.Price <= src.TargetPrice.Value))
            .ForMember(dest => dest.HasStockStatusChanged, opt => opt.MapFrom(src => src.Product != null && src.Product.StockQuantity > 0 != src.WasInStock));

        // AddToWishlistDto'dan WishlistItem entity'ye mapping
        CreateMap<AddToWishlistDto, WishlistItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.WishlistId, opt => opt.Ignore())
            .ForMember(dest => dest.Wishlist, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.PriceAtTime, opt => opt.Ignore())
            .ForMember(dest => dest.DiscountedPriceAtTime, opt => opt.Ignore())
            .ForMember(dest => dest.WasInStock, opt => opt.Ignore())
            .ForMember(dest => dest.LastPriceNotificationAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastStockNotificationAt, opt => opt.Ignore())
            .ForMember(dest => dest.PriceHistory, opt => opt.Ignore())
            .ForMember(dest => dest.StockHistory, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        // WishlistShare entity'den DTO'ya mapping
        CreateMap<WishlistShare, WishlistShareDto>()
            .ForMember(dest => dest.SharedWithUserName, opt => opt.MapFrom(src => src.SharedWithUser != null ? src.SharedWithUser.FirstName + " " + src.SharedWithUser.LastName : null))
            .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.ExpiresAt.HasValue && src.ExpiresAt.Value < DateTime.UtcNow))
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => src.IsActive && !src.IsDeleted && (!src.ExpiresAt.HasValue || src.ExpiresAt.Value >= DateTime.UtcNow)));

        // WishlistItemPriceHistory entity'den DTO'ya mapping
        CreateMap<WishlistItemPriceHistory, WishlistItemPriceHistoryDto>()
            .ForMember(dest => dest.IsPriceIncrease, opt => opt.MapFrom(src => src.ChangeType == "Increase"))
            .ForMember(dest => dest.IsPriceDecrease, opt => opt.MapFrom(src => src.ChangeType == "Decrease"))
            .ForMember(dest => dest.PriceChangeAmount, opt => opt.MapFrom(src => src.NewPrice - src.OldPrice));

        // WishlistItemStockHistory entity'den DTO'ya mapping
        CreateMap<WishlistItemStockHistory, WishlistItemStockHistoryDto>()
            .ForMember(dest => dest.HasStockStatusChanged, opt => opt.MapFrom(src => src.OldStockStatus != src.NewStockStatus))
            .ForMember(dest => dest.HasQuantityChanged, opt => opt.MapFrom(src => src.OldStockQuantity != src.NewStockQuantity))
            .ForMember(dest => dest.IsStockIncrease, opt => opt.MapFrom(src => src.NewStockQuantity > src.OldStockQuantity))
            .ForMember(dest => dest.IsStockDecrease, opt => opt.MapFrom(src => src.NewStockQuantity < src.OldStockQuantity));
    }
}
