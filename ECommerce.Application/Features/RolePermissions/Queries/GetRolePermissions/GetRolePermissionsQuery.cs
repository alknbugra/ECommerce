using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.RolePermissions.Queries.GetRolePermissions;

/// <summary>
/// Rol yetkilerini getirme sorgusu
/// </summary>
public class GetRolePermissionsQuery : IQuery<IEnumerable<RolePermissionDto>>
{
    /// <summary>
    /// Rol ID (opsiyonel - belirtilmezse tüm rol yetkileri getirilir)
    /// </summary>
    public Guid? RoleId { get; set; }

    /// <summary>
    /// Sadece aktif yetkileri getir
    /// </summary>
    public bool OnlyActive { get; set; } = true;
}
