using FluentValidation;

namespace ECommerce.Application.Features.Inventory.Commands.StockIn;

/// <summary>
/// Stok girişi komut validator'ı
/// </summary>
public class StockInCommandValidator : AbstractValidator<StockInCommand>
{
    public StockInCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Ürün ID'si gereklidir");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Neden gereklidir")
            .MaximumLength(200).WithMessage("Neden en fazla 200 karakter olabilir");
    }
}
