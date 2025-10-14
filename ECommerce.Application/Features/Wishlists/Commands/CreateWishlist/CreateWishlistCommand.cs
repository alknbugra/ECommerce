using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Results;
using FluentValidation;

namespace ECommerce.Application.Features.Wishlists.Commands.CreateWishlist;

/// <summary>
/// Favori liste oluşturma command'ı
/// </summary>
public class CreateWishlistCommand : ICommand<WishlistDto>
{
    /// <summary>
    /// Favori liste oluşturma DTO'su
    /// </summary>
    public CreateWishlistDto CreateWishlistDto { get; set; } = null!;

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// Favori liste oluşturma command handler'ı
/// </summary>
public class CreateWishlistCommandHandler : ICommandHandler<CreateWishlistCommand, WishlistDto>
{
    private readonly IWishlistService _wishlistService;

    public CreateWishlistCommandHandler(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    public async Task<Result<WishlistDto>> Handle(CreateWishlistCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var wishlist = await _wishlistService.CreateWishlistAsync(
                request.CreateWishlistDto, 
                request.UserId, 
                cancellationToken);

            if (wishlist == null)
            {
                return Result.Failure<WishlistDto>(Error.Problem("Wishlist.CreateFailed", "Favori liste oluşturulamadı"));
            }

            return Result.Success<WishlistDto>(wishlist);
        }
        catch (Exception ex)
        {
            return Result.Failure<WishlistDto>(Error.Problem("Wishlist.CreateError", ex.Message));
        }
    }
}

/// <summary>
/// Favori liste oluşturma command validator'ı
/// </summary>
public class CreateWishlistCommandValidator : AbstractValidator<CreateWishlistCommand>
{
    public CreateWishlistCommandValidator()
    {
        RuleFor(x => x.CreateWishlistDto)
            .NotNull()
            .WithMessage("Favori liste bilgileri boş olamaz");

        RuleFor(x => x.CreateWishlistDto.Name)
            .NotEmpty()
            .WithMessage("Favori liste adı boş olamaz")
            .MaximumLength(100)
            .WithMessage("Favori liste adı en fazla 100 karakter olabilir");

        RuleFor(x => x.CreateWishlistDto.Description)
            .MaximumLength(500)
            .WithMessage("Favori liste açıklaması en fazla 500 karakter olabilir");

        RuleFor(x => x.CreateWishlistDto.ListType)
            .NotEmpty()
            .WithMessage("Favori liste türü boş olamaz")
            .MaximumLength(20)
            .WithMessage("Favori liste türü en fazla 20 karakter olabilir");

        RuleFor(x => x.CreateWishlistDto.Color)
            .Matches(@"^#[0-9A-Fa-f]{6}$")
            .WithMessage("Geçersiz renk formatı (örn: #FF0000)")
            .When(x => !string.IsNullOrEmpty(x.CreateWishlistDto.Color));

        RuleFor(x => x.CreateWishlistDto.Icon)
            .MaximumLength(50)
            .WithMessage("İkon adı en fazla 50 karakter olabilir");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID'si boş olamaz");
    }
}
