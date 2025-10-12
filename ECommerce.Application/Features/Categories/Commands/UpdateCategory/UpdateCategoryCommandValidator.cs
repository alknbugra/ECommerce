using FluentValidation;

namespace ECommerce.Application.Features.Categories.Commands.UpdateCategory;

/// <summary>
/// UpdateCategory komutu için FluentValidation kuralları
/// </summary>
public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Kategori ID'si gereklidir.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Kategori adı gereklidir.")
            .MinimumLength(2)
            .WithMessage("Kategori adı en az 2 karakter olmalıdır.")
            .MaximumLength(100)
            .WithMessage("Kategori adı en fazla 100 karakter olabilir.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ0-9\s\-_]+$")
            .WithMessage("Kategori adı sadece harf, rakam, boşluk, tire ve alt çizgi içerebilir.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Kategori açıklaması en fazla 500 karakter olabilir.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.ParentCategoryId)
            .NotEqual(x => x.Id)
            .WithMessage("Kategori kendisinin alt kategorisi olamaz.")
            .When(x => x.ParentCategoryId.HasValue);

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Sıralama düzeni 0 veya pozitif bir sayı olmalıdır.");
    }
}
