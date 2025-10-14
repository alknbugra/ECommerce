using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Results;
using FluentValidation;

namespace ECommerce.Application.Features.Wishlists.Queries.GetUserWishlists;

/// <summary>
/// Kullanıcı favori listelerini getirme query'si
/// </summary>
public class GetUserWishlistsQuery : IQuery<List<WishlistDto>>
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// Kullanıcı favori listelerini getirme query handler'ı
/// </summary>
public class GetUserWishlistsQueryHandler : IQueryHandler<GetUserWishlistsQuery, List<WishlistDto>>
{
    private readonly IWishlistService _wishlistService;

    public GetUserWishlistsQueryHandler(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    public async Task<Result<List<WishlistDto>>> Handle(GetUserWishlistsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var wishlists = await _wishlistService.GetUserWishlistsAsync(request.UserId, cancellationToken);
            return Result.Success<List<WishlistDto>>(wishlists);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<WishlistDto>>(Error.Problem("Wishlist.GetUserWishlistsError", ex.Message));
        }
    }
}

/// <summary>
/// Kullanıcı favori listelerini getirme query validator'ı
/// </summary>
public class GetUserWishlistsQueryValidator : AbstractValidator<GetUserWishlistsQuery>
{
    public GetUserWishlistsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID'si boş olamaz");
    }
}
