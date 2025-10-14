using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Features.RolePermissions.Commands.AssignPermissionToRole;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.RolePermissions.Commands.AssignPermissionToRole;

/// <summary>
/// Role yetki atama komut handler'ı
/// </summary>
public class AssignPermissionToRoleCommandHandler : ICommandHandler<AssignPermissionToRoleCommand, bool>
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AssignPermissionToRoleCommandHandler> _logger;

    public AssignPermissionToRoleCommandHandler(
        IRolePermissionRepository rolePermissionRepository,
        IUnitOfWork unitOfWork,
        ILogger<AssignPermissionToRoleCommandHandler> logger)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(AssignPermissionToRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Role {RoleId} yetki {PermissionId} atanıyor.", request.RoleId, request.PermissionId);

            var rolePermission = await _rolePermissionRepository.AssignPermissionToRoleAsync(
                request.RoleId, 
                request.PermissionId, 
                request.AssignedBy);

            if (request.ExpiresAt.HasValue)
            {
                rolePermission.ExpiresAt = request.ExpiresAt.Value;
                rolePermission.UpdatedAt = DateTime.UtcNow;
                rolePermission.UpdatedBy = request.AssignedBy;
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Role {RoleId} yetki {PermissionId} başarıyla atandı.", request.RoleId, request.PermissionId);

            return Result.Success<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Role {RoleId} yetki {PermissionId} atanırken hata oluştu.", request.RoleId, request.PermissionId);
            return Result.Failure<bool>(Error.Problem("RolePermission.AssignError", "Role yetki atama sırasında bir hata oluştu."));
        }
    }
}
