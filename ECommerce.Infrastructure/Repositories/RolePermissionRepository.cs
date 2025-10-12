using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

/// <summary>
/// RolePermission repository implementasyonu
/// </summary>
public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
{
    public RolePermissionRepository(ECommerceDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Rol ID'ye göre rol-yetki ilişkilerini getirir
    /// </summary>
    public async Task<IEnumerable<RolePermission>> GetByRoleIdAsync(Guid roleId)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Permission)
            .Where(rp => rp.RoleId == roleId && !rp.IsDeleted)
            .OrderBy(rp => rp.Permission.Category)
            .ThenBy(rp => rp.Permission.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Yetki ID'ye göre rol-yetki ilişkilerini getirir
    /// </summary>
    public async Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(Guid permissionId)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Where(rp => rp.PermissionId == permissionId && !rp.IsDeleted)
            .OrderBy(rp => rp.Role.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Rol ve yetki ID'ye göre ilişki getirir
    /// </summary>
    public async Task<RolePermission?> GetByRoleAndPermissionIdAsync(Guid roleId, Guid permissionId)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId && !rp.IsDeleted);
    }

    /// <summary>
    /// Aktif rol-yetki ilişkilerini getirir
    /// </summary>
    public async Task<IEnumerable<RolePermission>> GetActiveRolePermissionsAsync()
    {
        return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .Where(rp => rp.IsActive && !rp.IsDeleted)
            .OrderBy(rp => rp.Role.Name)
            .ThenBy(rp => rp.Permission.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Kullanıcı ID'ye göre rol-yetki ilişkilerini getirir
    /// </summary>
    public async Task<IEnumerable<RolePermission>> GetByUserIdAsync(Guid userId)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .Where(rp => rp.Role.UserRoles.Any(ur => ur.UserId == userId && ur.IsActive && !ur.IsDeleted) && !rp.IsDeleted)
            .OrderBy(rp => rp.Permission.Category)
            .ThenBy(rp => rp.Permission.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Rol'e yetki atar
    /// </summary>
    public async Task<RolePermission> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, Guid? assignedBy = null)
    {
        // Önce mevcut ilişki var mı kontrol et
        var existingRolePermission = await GetByRoleAndPermissionIdAsync(roleId, permissionId);
        
        if (existingRolePermission != null)
        {
            // Eğer soft delete edilmişse, geri aktif et
            if (existingRolePermission.IsDeleted)
            {
                existingRolePermission.IsDeleted = false;
                existingRolePermission.IsActive = true;
                existingRolePermission.AssignedDate = DateTime.UtcNow;
                existingRolePermission.AssignedBy = assignedBy;
                existingRolePermission.UpdatedAt = DateTime.UtcNow;
                existingRolePermission.UpdatedBy = assignedBy;
                
                await _context.SaveChangesAsync();
                return existingRolePermission;
            }
            
            // Zaten aktif ilişki varsa, mevcut olanı döndür
            return existingRolePermission;
        }

        // Yeni ilişki oluştur
        var rolePermission = new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId,
            AssignedDate = DateTime.UtcNow,
            AssignedBy = assignedBy,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = assignedBy
        };

        await _context.RolePermissions.AddAsync(rolePermission);
        await _context.SaveChangesAsync();

        return rolePermission;
    }

    /// <summary>
    /// Rol'den yetki kaldırır
    /// </summary>
    public async Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
    {
        var rolePermission = await GetByRoleAndPermissionIdAsync(roleId, permissionId);
        
        if (rolePermission == null)
            return false;

        // Soft delete yap
        rolePermission.IsDeleted = true;
        rolePermission.IsActive = false;
        rolePermission.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}
