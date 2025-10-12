using FluentValidation;

namespace ECommerce.Application.Features.Permissions.Queries.GetPermissions;

/// <summary>
/// Tüm yetkileri getirme sorgu validatörü
/// </summary>
public class GetPermissionsQueryValidator : AbstractValidator<GetPermissionsQuery>
{
    public GetPermissionsQueryValidator()
    {
        RuleFor(x => x.Category)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Category))
            .WithMessage("Kategori adı en fazla 50 karakter olabilir.");
    }
}
