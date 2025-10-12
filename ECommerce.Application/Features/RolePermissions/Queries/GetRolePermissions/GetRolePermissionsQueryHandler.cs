using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.RolePermissions.Queries.GetRolePermissions;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.RolePermissions.Queries.GetRolePermissions;

/// <summary>
/// Rol yetkilerini getirme sorgu handler'ı
/// </summary>
public class GetRolePermissionsQueryHandler : IQueryHandler<GetRolePermissionsQuery, IEnumerable<RolePermissionDto>>
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly ILogger<GetRolePermissionsQueryHandler> _logger;

    public GetRolePermissionsQueryHandler(
        IRolePermissionRepository rolePermissionRepository,
        ILogger<GetRolePermissionsQueryHandler> logger)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<RolePermissionDto>> HandleAsync(GetRolePermissionsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Rol yetkileri getiriliyor. Rol ID: {RoleId}, Sadece Aktif: {OnlyActive}", 
            request.RoleId, request.OnlyActive);

        IEnumerable<Domain.Entities.RolePermission> rolePermissions;

        if (request.RoleId.HasValue)
        {
            rolePermissions = await _rolePermissionRepository.GetByRoleIdAsync(request.RoleId.Value);
        }
        else if (request.OnlyActive)
        {
            rolePermissions = await _rolePermissionRepository.GetActiveRolePermissionsAsync();
        }
        else
        {
            rolePermissions = await _rolePermissionRepository.GetAllAsync();
        }

        var rolePermissionDtos = rolePermissions.Select(rp => new RolePermissionDto
        {
            Id = rp.Id,
            RoleId = rp.RoleId,
            RoleName = rp.Role?.Name ?? string.Empty,
            PermissionId = rp.PermissionId,
            PermissionName = rp.Permission?.Name ?? string.Empty,
            PermissionCategory = rp.Permission?.Category ?? string.Empty,
            PermissionAction = rp.Permission?.Action ?? string.Empty,
            PermissionResource = rp.Permission?.Resource ?? string.Empty,
            AssignedDate = rp.AssignedDate,
            AssignedBy = rp.AssignedBy,
            IsActive = rp.IsActive,
            ExpiresAt = rp.ExpiresAt
        });

        _logger.LogInformation("{Count} rol yetki ilişkisi getirildi.", rolePermissionDtos.Count());

        return rolePermissionDtos;
    }
}
