using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Features.Permissions.Queries.GetPermissions;

/// <summary>
/// Tüm yetkileri getirme sorgusu
/// </summary>
public class GetPermissionsQuery : IQuery<IEnumerable<PermissionDto>>
{
    /// <summary>
    /// Kategori filtresi (opsiyonel)
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Sadece aktif yetkileri getir
    /// </summary>
    public bool OnlyActive { get; set; } = true;
}
