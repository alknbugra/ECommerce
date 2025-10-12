using FluentValidation;

namespace ECommerce.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// RefreshToken komutu için FluentValidation kuralları
/// </summary>
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token gereklidir.")
            .MaximumLength(500)
            .WithMessage("Refresh token en fazla 500 karakter olabilir.");
    }
}
