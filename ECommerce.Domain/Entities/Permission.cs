using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Yetki (Permission) entity'si
/// </summary>
public class Permission : BaseEntity
{
    /// <summary>
    /// Yetki adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Yetki açıklaması
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Yetki kategorisi (örn: User, Product, Order, Category)
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Yetki eylemi (örn: Create, Read, Update, Delete, Manage)
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Yetki kaynağı (örn: Users, Products, Orders)
    /// </summary>
    public string Resource { get; set; } = string.Empty;

    /// <summary>
    /// Yetki aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Bu yetkiye sahip roller
    /// </summary>
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
