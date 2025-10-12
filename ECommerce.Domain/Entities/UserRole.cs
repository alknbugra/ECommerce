namespace ECommerce.Domain.Entities;

/// <summary>
/// Kullanıcı-Rol ilişki entity'si
/// </summary>
public class UserRole : BaseEntity
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Rol ID'si
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Rol atama tarihi
    /// </summary>
    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Rol aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Kullanıcı
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Rol
    /// </summary>
    public virtual Role Role { get; set; } = null!;
}
