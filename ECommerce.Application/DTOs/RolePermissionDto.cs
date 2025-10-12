namespace ECommerce.Application.DTOs;

/// <summary>
/// Rol-Yetki (Role-Permission) DTO'su
/// </summary>
public class RolePermissionDto
{
    /// <summary>
    /// Rol-yetki ilişki ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Rol ID
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Rol adı
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// Yetki ID
    /// </summary>
    public Guid PermissionId { get; set; }

    /// <summary>
    /// Yetki adı
    /// </summary>
    public string PermissionName { get; set; } = string.Empty;

    /// <summary>
    /// Yetki kategorisi
    /// </summary>
    public string PermissionCategory { get; set; } = string.Empty;

    /// <summary>
    /// Yetki eylemi
    /// </summary>
    public string PermissionAction { get; set; } = string.Empty;

    /// <summary>
    /// Yetki kaynağı
    /// </summary>
    public string PermissionResource { get; set; } = string.Empty;

    /// <summary>
    /// Yetki atama tarihi
    /// </summary>
    public DateTime AssignedDate { get; set; }

    /// <summary>
    /// Yetki atayan kullanıcı ID
    /// </summary>
    public Guid? AssignedBy { get; set; }

    /// <summary>
    /// Yetki aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Yetki sona erme tarihi
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}
