using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Mappings;

/// <summary>
/// Kupon AutoMapper profili
/// </summary>
public class CouponMappingProfile : Profile
{
    public CouponMappingProfile()
    {
        // Coupon entity'den CouponDto'ya mapping
        CreateMap<Domain.Entities.Coupon, CouponDto>()
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => src.IsValid))
            .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired))
            .ForMember(dest => dest.IsUsageLimitReached, opt => opt.MapFrom(src => src.IsUsageLimitReached));

        // CreateCouponDto'dan Coupon entity'ye mapping
        CreateMap<CreateCouponDto, Domain.Entities.Coupon>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code.ToUpper()))
            .ForMember(dest => dest.UsageCount, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.CouponUsages, opt => opt.Ignore())
            .ForMember(dest => dest.CouponCategories, opt => opt.Ignore())
            .ForMember(dest => dest.CouponProducts, opt => opt.Ignore())
            .ForMember(dest => dest.CouponUsers, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        // CouponUsage entity'den CouponUsageDto'ya mapping
        CreateMap<CouponUsage, CouponUsageDto>()
            .ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.Coupon.Code))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.Order.OrderNumber));
    }
}
