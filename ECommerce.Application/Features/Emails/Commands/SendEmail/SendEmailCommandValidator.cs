using FluentValidation;

namespace ECommerce.Application.Features.Emails.Commands.SendEmail;

/// <summary>
/// Email gönderme komut validator'ı
/// </summary>
public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
{
    public SendEmailCommandValidator()
    {
        RuleFor(x => x.ToEmail)
            .NotEmpty().WithMessage("Alıcı email adresi gereklidir")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz");

        RuleFor(x => x.EmailType)
            .NotEmpty().WithMessage("Email türü gereklidir")
            .MaximumLength(50).WithMessage("Email türü en fazla 50 karakter olabilir");

        RuleFor(x => x.ToName)
            .MaximumLength(100).WithMessage("Alıcı adı en fazla 100 karakter olabilir");

        RuleFor(x => x.RelatedEntityType)
            .MaximumLength(50).WithMessage("İlişkili entity türü en fazla 50 karakter olabilir");
    }
}
