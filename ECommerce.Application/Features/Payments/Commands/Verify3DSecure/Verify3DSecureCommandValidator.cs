using FluentValidation;

namespace ECommerce.Application.Features.Payments.Commands.Verify3DSecure;

/// <summary>
/// 3D Secure doğrulama komut validator'ı
/// </summary>
public class Verify3DSecureCommandValidator : AbstractValidator<Verify3DSecureCommand>
{
    public Verify3DSecureCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty().WithMessage("Ödeme ID'si gereklidir");

        RuleFor(x => x.ThreeDSecureResponse)
            .NotEmpty().WithMessage("3D Secure yanıtı gereklidir");
    }
}
