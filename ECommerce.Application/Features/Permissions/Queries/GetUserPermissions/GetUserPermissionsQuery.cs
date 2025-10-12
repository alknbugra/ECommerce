using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Permissions.Queries.GetUserPermissions;

/// <summary>
/// Kullanıcı yetkilerini getirme sorgusu
/// </summary>
public class GetUserPermissionsQuery : IQuery<IEnumerable<PermissionDto>>
{
    /// <summary>
    /// Kullanıcı ID
    /// </summary>
    public Guid UserId { get; set; }
}
