using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Application.Features.RolePermissions.Commands.AssignPermissionToRole;

/// <summary>
/// Role yetki atama komutu
/// </summary>
public class AssignPermissionToRoleCommand : ICommand<bool>
{
    /// <summary>
    /// Rol ID
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Yetki ID
    /// </summary>
    public Guid PermissionId { get; set; }

    /// <summary>
    /// Yetki atayan kullanıcı ID
    /// </summary>
    public Guid? AssignedBy { get; set; }

    /// <summary>
    /// Yetki sona erme tarihi (opsiyonel)
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}
