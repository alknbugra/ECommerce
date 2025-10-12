using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

/// <summary>
/// Permission repository interface'i
/// </summary>
public interface IPermissionRepository : IRepository<Permission>
{
    /// <summary>
    /// Kategoriye göre yetkileri getirir
    /// </summary>
    /// <param name="category">Yetki kategorisi</param>
    /// <returns>Yetki listesi</returns>
    Task<IEnumerable<Permission>> GetByCategoryAsync(string category);

    /// <summary>
    /// Aktif yetkileri getirir
    /// </summary>
    /// <returns>Aktif yetki listesi</returns>
    Task<IEnumerable<Permission>> GetActivePermissionsAsync();

    /// <summary>
    /// Rol ID'ye göre yetkileri getirir
    /// </summary>
    /// <param name="roleId">Rol ID</param>
    /// <returns>Yetki listesi</returns>
    Task<IEnumerable<Permission>> GetByRoleIdAsync(Guid roleId);

    /// <summary>
    /// Yetki adına göre yetki getirir
    /// </summary>
    /// <param name="name">Yetki adı</param>
    /// <returns>Yetki</returns>
    Task<Permission?> GetByNameAsync(string name);

    /// <summary>
    /// Kullanıcı ID'ye göre yetkileri getirir
    /// </summary>
    /// <param name="userId">Kullanıcı ID</param>
    /// <returns>Yetki listesi</returns>
    Task<IEnumerable<Permission>> GetByUserIdAsync(Guid userId);
}
