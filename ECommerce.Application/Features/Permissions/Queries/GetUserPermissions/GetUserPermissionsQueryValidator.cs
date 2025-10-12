using FluentValidation;

namespace ECommerce.Application.Features.Permissions.Queries.GetUserPermissions;

/// <summary>
/// Kullanıcı yetkilerini getirme sorgu validatörü
/// </summary>
public class GetUserPermissionsQueryValidator : AbstractValidator<GetUserPermissionsQuery>
{
    public GetUserPermissionsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID boş olamaz.");
    }
}
