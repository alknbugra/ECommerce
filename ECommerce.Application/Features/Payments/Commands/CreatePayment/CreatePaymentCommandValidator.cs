using FluentValidation;

namespace ECommerce.Application.Features.Payments.Commands.CreatePayment;

/// <summary>
/// Ödeme oluşturma komut validator'ı
/// </summary>
public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Sipariş ID'si gereklidir");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Ödeme yöntemi gereklidir")
            .MaximumLength(50).WithMessage("Ödeme yöntemi en fazla 50 karakter olabilir");

        When(x => x.PaymentMethod == "CreditCard" || x.PaymentMethod == "DebitCard", () =>
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Kart numarası gereklidir")
                .Matches(@"^\d{16}$").WithMessage("Kart numarası 16 haneli olmalıdır");

            RuleFor(x => x.CardHolderName)
                .NotEmpty().WithMessage("Kart sahibi adı gereklidir")
                .MaximumLength(100).WithMessage("Kart sahibi adı en fazla 100 karakter olabilir");

            RuleFor(x => x.ExpiryMonth)
                .NotEmpty().WithMessage("Son kullanma ayı gereklidir")
                .InclusiveBetween(1, 12).WithMessage("Son kullanma ayı 1-12 arasında olmalıdır");

            RuleFor(x => x.ExpiryYear)
                .NotEmpty().WithMessage("Son kullanma yılı gereklidir")
                .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("Son kullanma yılı geçerli bir yıl olmalıdır");

            RuleFor(x => x.Cvv)
                .NotEmpty().WithMessage("CVV kodu gereklidir")
                .Matches(@"^\d{3,4}$").WithMessage("CVV kodu 3 veya 4 haneli olmalıdır");
        });

        RuleFor(x => x.InstallmentCount)
            .GreaterThan(0).WithMessage("Taksit sayısı 0'dan büyük olmalıdır")
            .LessThanOrEqualTo(12).WithMessage("Taksit sayısı en fazla 12 olabilir");
    }
}
