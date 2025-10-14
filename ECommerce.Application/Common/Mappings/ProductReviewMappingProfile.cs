using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Mappings;

/// <summary>
/// Ürün değerlendirme AutoMapper profili
/// </summary>
public class ProductReviewMappingProfile : Profile
{
    public ProductReviewMappingProfile()
    {
        // ProductReview entity'den ProductReviewDto'ya mapping
        CreateMap<Domain.Entities.ProductReview, ProductReviewDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.Order != null ? src.Order.OrderNumber : null))
            .ForMember(dest => dest.ApprovedByUserName, opt => opt.MapFrom(src => src.ApprovedByUser != null ? src.ApprovedByUser.FirstName + " " + src.ApprovedByUser.LastName : null))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.IsPending, opt => opt.MapFrom(src => src.IsPending))
            .ForMember(dest => dest.NetHelpfulScore, opt => opt.MapFrom(src => src.NetHelpfulScore));

        // CreateProductReviewDto'dan ProductReview entity'ye mapping
        CreateMap<CreateProductReviewDto, Domain.Entities.ProductReview>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.IsApproved, opt => opt.Ignore())
            .ForMember(dest => dest.IsRejected, opt => opt.Ignore())
            .ForMember(dest => dest.RejectionReason, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedAt, opt => opt.Ignore())
            .ForMember(dest => dest.HelpfulCount, opt => opt.Ignore())
            .ForMember(dest => dest.NotHelpfulCount, opt => opt.Ignore())
            .ForMember(dest => dest.UserIpAddress, opt => opt.Ignore())
            .ForMember(dest => dest.UserAgent, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Order, opt => opt.Ignore())
            .ForMember(dest => dest.OrderItem, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewResponses, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewVotes, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewImages, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        // ReviewResponse entity'den ReviewResponseDto'ya mapping
        CreateMap<ReviewResponse, ReviewResponseDto>()
            .ForMember(dest => dest.RespondedByUserName, opt => opt.MapFrom(src => src.RespondedByUser.FirstName + " " + src.RespondedByUser.LastName))
            .ForMember(dest => dest.ApprovedByUserName, opt => opt.MapFrom(src => src.ApprovedByUser != null ? src.ApprovedByUser.FirstName + " " + src.ApprovedByUser.LastName : null));

        // ReviewVote entity'den ReviewVoteDto'ya mapping
        CreateMap<ReviewVote, ReviewVoteDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));

        // ReviewImage entity'den ReviewImageDto'ya mapping
        CreateMap<ReviewImage, ReviewImageDto>();
    }
}
