using FluentValidation;

namespace ECommerce.Application.Features.Cart.Commands.RemoveFromCart;

/// <summary>
/// RemoveFromCart komutu için FluentValidation kuralları
/// </summary>
public class RemoveFromCartCommandValidator : AbstractValidator<RemoveFromCartCommand>
{
    public RemoveFromCartCommandValidator()
    {
        RuleFor(x => x.CartItemId)
            .NotEmpty()
            .WithMessage("Sepet ürünü ID'si gereklidir.");

        RuleFor(x => x)
            .Must(HaveValidUserOrSession)
            .WithMessage("Kullanıcı ID'si veya Session ID'si gereklidir.");
    }

    private static bool HaveValidUserOrSession(RemoveFromCartCommand command)
    {
        return command.UserId.HasValue || !string.IsNullOrEmpty(command.SessionId);
    }
}
