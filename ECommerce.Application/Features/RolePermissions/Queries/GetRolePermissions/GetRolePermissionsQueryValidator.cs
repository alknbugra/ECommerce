using FluentValidation;

namespace ECommerce.Application.Features.RolePermissions.Queries.GetRolePermissions;

/// <summary>
/// Rol yetkilerini getirme sorgu validatörü
/// </summary>
public class GetRolePermissionsQueryValidator : AbstractValidator<GetRolePermissionsQuery>
{
    public GetRolePermissionsQueryValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .When(x => x.RoleId.HasValue)
            .WithMessage("Rol ID geçerli bir GUID olmalıdır.");
    }
}
