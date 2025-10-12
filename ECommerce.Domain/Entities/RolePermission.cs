using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Rol-Yetki (Role-Permission) ilişki entity'si
/// </summary>
public class RolePermission : BaseEntity
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
    /// Yetki atama tarihi
    /// </summary>
    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Yetki atayan kullanıcı ID
    /// </summary>
    public Guid? AssignedBy { get; set; }

    /// <summary>
    /// Yetki aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Yetki sona erme tarihi (opsiyonel)
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// İlişkili rol
    /// </summary>
    public virtual Role Role { get; set; } = null!;

    /// <summary>
    /// İlişkili yetki
    /// </summary>
    public virtual Permission Permission { get; set; } = null!;
}
