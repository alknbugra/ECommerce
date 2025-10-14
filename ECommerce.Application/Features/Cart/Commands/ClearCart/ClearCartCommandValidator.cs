using FluentValidation;

namespace ECommerce.Application.Features.Cart.Commands.ClearCart;

/// <summary>
/// ClearCart komutu için FluentValidation kuralları
/// </summary>
public class ClearCartCommandValidator : AbstractValidator<ClearCartCommand>
{
    public ClearCartCommandValidator()
    {
        RuleFor(x => x)
            .Must(HaveValidUserOrSession)
            .WithMessage("Kullanıcı ID'si veya Session ID'si gereklidir.");
    }

    private static bool HaveValidUserOrSession(ClearCartCommand command)
    {
        return command.UserId.HasValue || !string.IsNullOrEmpty(command.SessionId);
    }
}
