using FluentValidation;

namespace ECommerce.Application.Features.Cargo.Commands.CreateCargo;

/// <summary>
/// Kargo oluşturma komut doğrulayıcısı
/// </summary>
public class CreateCargoCommandValidator : AbstractValidator<CreateCargoCommand>
{
    public CreateCargoCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Sipariş ID'si gereklidir.");

        RuleFor(x => x.CargoCompanyId)
            .NotEmpty()
            .WithMessage("Kargo şirketi ID'si gereklidir.");

        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .WithMessage("Kargo ağırlığı 0'dan büyük olmalıdır.");

        RuleFor(x => x.ShippingCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Kargo ücreti 0'dan küçük olamaz.");

        RuleFor(x => x.CustomerShippingCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Müşteri kargo ücreti 0'dan küçük olamaz.");

        RuleFor(x => x.Sender.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Gönderen adı gereklidir ve 100 karakterden fazla olamaz.");

        RuleFor(x => x.Sender.Address)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Gönderen adresi gereklidir ve 500 karakterden fazla olamaz.");

        RuleFor(x => x.Receiver.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Alıcı adı gereklidir ve 100 karakterden fazla olamaz.");

        RuleFor(x => x.Receiver.Address)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Alıcı adresi gereklidir ve 500 karakterden fazla olamaz.");

        RuleFor(x => x.Sender.Phone)
            .MaximumLength(20)
            .WithMessage("Gönderen telefon numarası 20 karakterden fazla olamaz.");

        RuleFor(x => x.Receiver.Phone)
            .MaximumLength(20)
            .WithMessage("Alıcı telefon numarası 20 karakterden fazla olamaz.");

        RuleFor(x => x.ContentDescription)
            .MaximumLength(500)
            .WithMessage("Kargo içeriği açıklaması 500 karakterden fazla olamaz.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Kargo notları 1000 karakterden fazla olamaz.");

        RuleFor(x => x.CompanyReferenceNumber)
            .MaximumLength(100)
            .WithMessage("Şirket referans numarası 100 karakterden fazla olamaz.");

        RuleFor(x => x.SpecialInstructions)
            .MaximumLength(500)
            .WithMessage("Özel talimatlar 500 karakterden fazla olamaz.");

        RuleFor(x => x.DeclaredValue)
            .GreaterThan(0)
            .When(x => x.DeclaredValue.HasValue)
            .WithMessage("Kargo değeri 0'dan büyük olmalıdır.");

        RuleFor(x => x.InsuranceAmount)
            .GreaterThan(0)
            .When(x => x.InsuranceAmount.HasValue)
            .WithMessage("Sigorta tutarı 0'dan büyük olmalıdır.");

        RuleFor(x => x.EstimatedDeliveryDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.EstimatedDeliveryDate.HasValue)
            .WithMessage("Tahmini teslim tarihi gelecekte olmalıdır.");
    }
}
