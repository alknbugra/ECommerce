using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

/// <summary>
/// RolePermission repository interface'i
/// </summary>
public interface IRolePermissionRepository : IRepository<RolePermission>
{
    /// <summary>
    /// Rol ID'ye göre rol-yetki ilişkilerini getirir
    /// </summary>
    /// <param name="roleId">Rol ID</param>
    /// <returns>Rol-yetki ilişki listesi</returns>
    Task<IEnumerable<RolePermission>> GetByRoleIdAsync(Guid roleId);

    /// <summary>
    /// Yetki ID'ye göre rol-yetki ilişkilerini getirir
    /// </summary>
    /// <param name="permissionId">Yetki ID</param>
    /// <returns>Rol-yetki ilişki listesi</returns>
    Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(Guid permissionId);

    /// <summary>
    /// Rol ve yetki ID'ye göre ilişki getirir
    /// </summary>
    /// <param name="roleId">Rol ID</param>
    /// <param name="permissionId">Yetki ID</param>
    /// <returns>Rol-yetki ilişkisi</returns>
    Task<RolePermission?> GetByRoleAndPermissionIdAsync(Guid roleId, Guid permissionId);

    /// <summary>
    /// Aktif rol-yetki ilişkilerini getirir
    /// </summary>
    /// <returns>Aktif rol-yetki ilişki listesi</returns>
    Task<IEnumerable<RolePermission>> GetActiveRolePermissionsAsync();

    /// <summary>
    /// Kullanıcı ID'ye göre rol-yetki ilişkilerini getirir
    /// </summary>
    /// <param name="userId">Kullanıcı ID</param>
    /// <returns>Rol-yetki ilişki listesi</returns>
    Task<IEnumerable<RolePermission>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Rol'e yetki atar
    /// </summary>
    /// <param name="roleId">Rol ID</param>
    /// <param name="permissionId">Yetki ID</param>
    /// <param name="assignedBy">Atayan kullanıcı ID</param>
    /// <returns>Oluşturulan rol-yetki ilişkisi</returns>
    Task<RolePermission> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, Guid? assignedBy = null);

    /// <summary>
    /// Rol'den yetki kaldırır
    /// </summary>
    /// <param name="roleId">Rol ID</param>
    /// <param name="permissionId">Yetki ID</param>
    /// <returns>İşlem başarılı mı?</returns>
    Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);
}
