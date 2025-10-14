using FluentValidation;

namespace ECommerce.Application.Features.Cart.Queries.GetCart;

/// <summary>
/// GetCart sorgusu için FluentValidation kuralları
/// </summary>
public class GetCartQueryValidator : AbstractValidator<GetCartQuery>
{
    public GetCartQueryValidator()
    {
        RuleFor(x => x)
            .Must(HaveValidUserOrSession)
            .WithMessage("Kullanıcı ID'si veya Session ID'si gereklidir.");
    }

    private static bool HaveValidUserOrSession(GetCartQuery query)
    {
        return query.UserId.HasValue || !string.IsNullOrEmpty(query.SessionId);
    }
}
