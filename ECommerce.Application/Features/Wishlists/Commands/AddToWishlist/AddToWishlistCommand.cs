using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Results;
using FluentValidation;

namespace ECommerce.Application.Features.Wishlists.Commands.AddToWishlist;

/// <summary>
/// Favorilere ekleme command'ı
/// </summary>
public class AddToWishlistCommand : ICommand<WishlistItemDto>
{
    /// <summary>
    /// Favorilere ekleme DTO'su
    /// </summary>
    public AddToWishlistDto AddToWishlistDto { get; set; } = null!;

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// Favorilere ekleme command handler'ı
/// </summary>
public class AddToWishlistCommandHandler : ICommandHandler<AddToWishlistCommand, WishlistItemDto>
{
    private readonly IWishlistService _wishlistService;

    public AddToWishlistCommandHandler(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    public async Task<Result<WishlistItemDto>> Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var wishlistItem = await _wishlistService.AddToWishlistAsync(
                request.AddToWishlistDto, 
                request.UserId, 
                cancellationToken);

            if (wishlistItem == null)
            {
                return Result.Failure<WishlistItemDto>(Error.Problem("Wishlist.AddFailed", "Ürün favorilere eklenemedi"));
            }

            return Result.Success<WishlistItemDto>(wishlistItem);
        }
        catch (Exception ex)
        {
            return Result.Failure<WishlistItemDto>(Error.Problem("Wishlist.AddError", ex.Message));
        }
    }
}

/// <summary>
/// Favorilere ekleme command validator'ı
/// </summary>
public class AddToWishlistCommandValidator : AbstractValidator<AddToWishlistCommand>
{
    public AddToWishlistCommandValidator()
    {
        RuleFor(x => x.AddToWishlistDto)
            .NotNull()
            .WithMessage("Favorilere ekleme bilgileri boş olamaz");

        RuleFor(x => x.AddToWishlistDto.ProductId)
            .NotEmpty()
            .WithMessage("Ürün ID'si boş olamaz");

        RuleFor(x => x.AddToWishlistDto.Notes)
            .MaximumLength(500)
            .WithMessage("Notlar en fazla 500 karakter olabilir");

        RuleFor(x => x.AddToWishlistDto.Priority)
            .InclusiveBetween(0, 3)
            .WithMessage("Öncelik seviyesi 0-3 arasında olmalıdır");

        RuleFor(x => x.AddToWishlistDto.TargetPrice)
            .GreaterThan(0)
            .WithMessage("Hedef fiyat 0'dan büyük olmalıdır")
            .When(x => x.AddToWishlistDto.TargetPrice.HasValue);

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID'si boş olamaz");
    }
}
