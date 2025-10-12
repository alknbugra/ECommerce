using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

/// <summary>
/// Permission repository implementasyonu
/// </summary>
public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    public PermissionRepository(ECommerceDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Kategoriye göre yetkileri getirir
    /// </summary>
    public async Task<IEnumerable<Permission>> GetByCategoryAsync(string category)
    {
        return await _context.Permissions
            .Where(p => p.Category == category && !p.IsDeleted)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Aktif yetkileri getirir
    /// </summary>
    public async Task<IEnumerable<Permission>> GetActivePermissionsAsync()
    {
        return await _context.Permissions
            .Where(p => p.IsActive && !p.IsDeleted)
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Rol ID'ye göre yetkileri getirir
    /// </summary>
    public async Task<IEnumerable<Permission>> GetByRoleIdAsync(Guid roleId)
    {
        return await _context.Permissions
            .Where(p => p.RolePermissions.Any(rp => rp.RoleId == roleId && rp.IsActive && !rp.IsDeleted) && !p.IsDeleted)
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Yetki adına göre yetki getirir
    /// </summary>
    public async Task<Permission?> GetByNameAsync(string name)
    {
        return await _context.Permissions
            .FirstOrDefaultAsync(p => p.Name == name && !p.IsDeleted);
    }

    /// <summary>
    /// Kullanıcı ID'ye göre yetkileri getirir
    /// </summary>
    public async Task<IEnumerable<Permission>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Permissions
            .Where(p => p.RolePermissions.Any(rp => 
                rp.Role.UserRoles.Any(ur => ur.UserId == userId && ur.IsActive && !ur.IsDeleted) &&
                rp.IsActive && !rp.IsDeleted) && !p.IsDeleted)
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }
}
