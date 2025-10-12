using FluentValidation;

namespace ECommerce.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// ID'ye göre ürün getirme sorgusu validator'ı
/// </summary>
public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Ürün ID'si boş olamaz")
            .NotEqual(Guid.Empty).WithMessage("Geçerli bir ürün ID'si giriniz");
    }
}
