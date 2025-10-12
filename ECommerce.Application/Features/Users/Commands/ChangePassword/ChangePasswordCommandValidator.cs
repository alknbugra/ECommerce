using FluentValidation;

namespace ECommerce.Application.Features.Users.Commands.ChangePassword;

/// <summary>
/// ChangePassword komutu için FluentValidation kuralları
/// </summary>
public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID'si gereklidir.");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Mevcut şifre gereklidir.")
            .MinimumLength(6)
            .WithMessage("Mevcut şifre en az 6 karakter olmalıdır.")
            .MaximumLength(100)
            .WithMessage("Mevcut şifre en fazla 100 karakter olabilir.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("Yeni şifre gereklidir.")
            .MinimumLength(8)
            .WithMessage("Yeni şifre en az 8 karakter olmalıdır.")
            .MaximumLength(100)
            .WithMessage("Yeni şifre en fazla 100 karakter olabilir.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("Yeni şifre en az bir küçük harf, bir büyük harf, bir rakam ve bir özel karakter içermelidir.")
            .NotEqual(x => x.CurrentPassword)
            .WithMessage("Yeni şifre mevcut şifre ile aynı olamaz.");
    }
}
