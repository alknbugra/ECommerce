using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Results;
using FluentValidation;

namespace ECommerce.Application.Features.Wishlists.Commands.RemoveFromWishlist;

/// <summary>
/// Favorilerden çıkarma command'ı
/// </summary>
public class RemoveFromWishlistCommand : ICommand<bool>
{
    /// <summary>
    /// Favori ürün ID'si
    /// </summary>
    public Guid WishlistItemId { get; set; }

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// Favorilerden çıkarma command handler'ı
/// </summary>
public class RemoveFromWishlistCommandHandler : ICommandHandler<RemoveFromWishlistCommand, bool>
{
    private readonly IWishlistService _wishlistService;

    public RemoveFromWishlistCommandHandler(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    public async Task<Result<bool>> Handle(RemoveFromWishlistCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var removed = await _wishlistService.RemoveFromWishlistAsync(
                request.WishlistItemId, 
                request.UserId, 
                cancellationToken);

            if (!removed)
            {
                return Result.Failure<bool>(Error.Problem("Wishlist.RemoveFailed", "Ürün favorilerden çıkarılamadı"));
            }

            return Result.Success<bool>(removed);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>(Error.Problem("Wishlist.RemoveError", ex.Message));
        }
    }
}

/// <summary>
/// Favorilerden çıkarma command validator'ı
/// </summary>
public class RemoveFromWishlistCommandValidator : AbstractValidator<RemoveFromWishlistCommand>
{
    public RemoveFromWishlistCommandValidator()
    {
        RuleFor(x => x.WishlistItemId)
            .NotEmpty()
            .WithMessage("Favori ürün ID'si boş olamaz");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID'si boş olamaz");
    }
}
