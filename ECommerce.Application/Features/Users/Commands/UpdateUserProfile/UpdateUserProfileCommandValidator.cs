using FluentValidation;

namespace ECommerce.Application.Features.Users.Commands.UpdateUserProfile;

/// <summary>
/// UpdateUserProfile komutu için FluentValidation kuralları
/// </summary>
public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID'si gereklidir.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Ad gereklidir.")
            .MinimumLength(2)
            .WithMessage("Ad en az 2 karakter olmalıdır.")
            .MaximumLength(50)
            .WithMessage("Ad en fazla 50 karakter olabilir.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$")
            .WithMessage("Ad sadece harf ve boşluk içerebilir.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Soyad gereklidir.")
            .MinimumLength(2)
            .WithMessage("Soyad en az 2 karakter olmalıdır.")
            .MaximumLength(50)
            .WithMessage("Soyad en fazla 50 karakter olabilir.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$")
            .WithMessage("Soyad sadece harf ve boşluk içerebilir.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(\+90|0)?[5][0-9]{9}$")
            .WithMessage("Geçerli bir Türkiye telefon numarası giriniz.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}
