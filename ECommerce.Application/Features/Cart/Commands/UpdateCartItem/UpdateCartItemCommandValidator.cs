using FluentValidation;

namespace ECommerce.Application.Features.Cart.Commands.UpdateCartItem;

/// <summary>
/// UpdateCartItem komutu için FluentValidation kuralları
/// </summary>
public class UpdateCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand>
{
    public UpdateCartItemCommandValidator()
    {
        RuleFor(x => x.CartItemId)
            .NotEmpty()
            .WithMessage("Sepet ürünü ID'si gereklidir.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Miktar 0'dan büyük olmalıdır.")
            .LessThanOrEqualTo(100)
            .WithMessage("Miktar 100'den küçük veya eşit olmalıdır.");

        RuleFor(x => x)
            .Must(HaveValidUserOrSession)
            .WithMessage("Kullanıcı ID'si veya Session ID'si gereklidir.");
    }

    private static bool HaveValidUserOrSession(UpdateCartItemCommand command)
    {
        return command.UserId.HasValue || !string.IsNullOrEmpty(command.SessionId);
    }
}
