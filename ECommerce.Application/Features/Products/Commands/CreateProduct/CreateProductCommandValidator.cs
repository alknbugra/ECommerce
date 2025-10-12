using FluentValidation;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// CreateProduct komutu için FluentValidation kuralları
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Ürün adı gereklidir.")
            .MinimumLength(2)
            .WithMessage("Ürün adı en az 2 karakter olmalıdır.")
            .MaximumLength(200)
            .WithMessage("Ürün adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Ürün açıklaması gereklidir.")
            .MinimumLength(10)
            .WithMessage("Ürün açıklaması en az 10 karakter olmalıdır.")
            .MaximumLength(2000)
            .WithMessage("Ürün açıklaması en fazla 2000 karakter olabilir.");

        RuleFor(x => x.Sku)
            .NotEmpty()
            .WithMessage("Ürün SKU'su gereklidir.")
            .MaximumLength(50)
            .WithMessage("Ürün SKU'su en fazla 50 karakter olabilir.")
            .Matches(@"^[A-Z0-9\-_]+$")
            .WithMessage("Ürün SKU'su sadece büyük harf, rakam, tire ve alt çizgi içerebilir.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.")
            .LessThan(1000000)
            .WithMessage("Ürün fiyatı çok yüksek.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stok miktarı negatif olamaz.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Kategori ID'si gereklidir.");

        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .WithMessage("Ürün ağırlığı 0'dan büyük olmalıdır.")
            .When(x => x.Weight.HasValue);

        // Dimensions ve Tags property'leri CreateProductCommand'ta yok, bu yüzden bu kuralları kaldırıyoruz
    }
}