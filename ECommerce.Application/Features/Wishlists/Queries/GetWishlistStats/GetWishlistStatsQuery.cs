using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Results;
using FluentValidation;

namespace ECommerce.Application.Features.Wishlists.Queries.GetWishlistStats;

/// <summary>
/// Favori liste istatistiklerini getirme query'si
/// </summary>
public class GetWishlistStatsQuery : IQuery<WishlistStatsDto>
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// Favori liste istatistiklerini getirme query handler'ı
/// </summary>
public class GetWishlistStatsQueryHandler : IQueryHandler<GetWishlistStatsQuery, WishlistStatsDto>
{
    private readonly IWishlistService _wishlistService;

    public GetWishlistStatsQueryHandler(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    public async Task<Result<WishlistStatsDto>> Handle(GetWishlistStatsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var stats = await _wishlistService.GetWishlistStatsAsync(request.UserId, cancellationToken);
            return Result.Success<WishlistStatsDto>(stats);
        }
        catch (Exception ex)
        {
            return Result.Failure<WishlistStatsDto>(Error.Problem("Wishlist.GetStatsError", ex.Message));
        }
    }
}

/// <summary>
/// Favori liste istatistiklerini getirme query validator'ı
/// </summary>
public class GetWishlistStatsQueryValidator : AbstractValidator<GetWishlistStatsQuery>
{
    public GetWishlistStatsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID'si boş olamaz");
    }
}
