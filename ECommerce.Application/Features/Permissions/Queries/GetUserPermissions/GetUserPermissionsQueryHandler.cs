using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Permissions.Queries.GetUserPermissions;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Permissions.Queries.GetUserPermissions;

/// <summary>
/// Kullanıcı yetkilerini getirme sorgu handler'ı
/// </summary>
public class GetUserPermissionsQueryHandler : IQueryHandler<GetUserPermissionsQuery, List<PermissionDto>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly ILogger<GetUserPermissionsQueryHandler> _logger;

    public GetUserPermissionsQueryHandler(
        IPermissionRepository permissionRepository,
        ILogger<GetUserPermissionsQueryHandler> logger)
    {
        _permissionRepository = permissionRepository;
        _logger = logger;
    }

    public async Task<Result<List<PermissionDto>>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kullanıcı {UserId} yetkileri getiriliyor.", request.UserId);

            var permissions = await _permissionRepository.GetByUserIdAsync(request.UserId);

            var permissionDtos = permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Category = p.Category,
                Action = p.Action,
                Resource = p.Resource,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            });

            _logger.LogInformation("Kullanıcı {UserId} için {Count} yetki getirildi.", request.UserId, permissionDtos.Count());

            return Result.Success<List<PermissionDto>>(permissionDtos.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı yetkileri getirme sırasında hata oluştu. UserId: {UserId}", request.UserId);
            return Result.Failure<List<PermissionDto>>(Error.Problem("Permission.GetUserPermissionsError", "Kullanıcı yetkileri getirme sırasında bir hata oluştu."));
        }
    }
}
