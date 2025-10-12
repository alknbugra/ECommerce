using FluentValidation;

namespace ECommerce.Application.Features.RolePermissions.Commands.AssignPermissionToRole;

/// <summary>
/// Role yetki atama komut validatörü
/// </summary>
public class AssignPermissionToRoleCommandValidator : AbstractValidator<AssignPermissionToRoleCommand>
{
    public AssignPermissionToRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("Rol ID boş olamaz.");

        RuleFor(x => x.PermissionId)
            .NotEmpty()
            .WithMessage("Yetki ID boş olamaz.");

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.ExpiresAt.HasValue)
            .WithMessage("Yetki sona erme tarihi gelecekte olmalıdır.");
    }
}
