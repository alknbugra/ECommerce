using FluentValidation;

namespace ECommerce.Application.Features.Cart.Commands.AddToCart;

/// <summary>
/// AddToCart komutu için FluentValidation kuralları
/// </summary>
public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Ürün ID'si gereklidir.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Miktar 0'dan büyük olmalıdır.")
            .LessThanOrEqualTo(100)
            .WithMessage("Miktar 100'den küçük veya eşit olmalıdır.");

        RuleFor(x => x)
            .Must(HaveValidUserOrSession)
            .WithMessage("Kullanıcı ID'si veya Session ID'si gereklidir.");
    }

    private static bool HaveValidUserOrSession(AddToCartCommand command)
    {
        return command.UserId.HasValue || !string.IsNullOrEmpty(command.SessionId);
    }
}
