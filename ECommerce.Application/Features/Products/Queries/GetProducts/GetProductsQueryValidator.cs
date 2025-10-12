using FluentValidation;

namespace ECommerce.Application.Features.Products.Queries.GetProducts;

/// <summary>
/// Ürünleri getirme sorgusu validator'ı
/// </summary>
public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Sayfa numarası 0'dan büyük olmalıdır")
            .LessThanOrEqualTo(1000).WithMessage("Sayfa numarası 1000'den küçük veya eşit olmalıdır");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Sayfa boyutu 0'dan büyük olmalıdır")
            .LessThanOrEqualTo(100).WithMessage("Sayfa boyutu 100'den küçük veya eşit olmalıdır");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(100).WithMessage("Arama terimi en fazla 100 karakter olabilir")
            .When(x => !string.IsNullOrEmpty(x.SearchTerm));

        RuleFor(x => x.SortBy)
            .Must(BeValidSortField).WithMessage("Geçersiz sıralama alanı")
            .When(x => !string.IsNullOrEmpty(x.SortBy));

        RuleFor(x => x.SortDirection)
            .Must(BeValidSortDirection).WithMessage("Sıralama yönü 'asc' veya 'desc' olmalıdır")
            .When(x => !string.IsNullOrEmpty(x.SortDirection));

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty).WithMessage("Geçerli bir kategori ID'si giriniz")
            .When(x => x.CategoryId.HasValue);
    }

    private static bool BeValidSortField(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return true;

        var validFields = new[] { "name", "price", "createdat", "stockquantity" };
        return validFields.Contains(sortBy.ToLower());
    }

    private static bool BeValidSortDirection(string? sortDirection)
    {
        if (string.IsNullOrEmpty(sortDirection))
            return true;

        var validDirections = new[] { "asc", "desc" };
        return validDirections.Contains(sortDirection.ToLower());
    }
}
