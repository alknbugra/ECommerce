using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Permissions.Queries.GetPermissions;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Permissions.Queries.GetPermissions;

/// <summary>
/// Tüm yetkileri getirme sorgu handler'ı
/// </summary>
public class GetPermissionsQueryHandler : IQueryHandler<GetPermissionsQuery, List<PermissionDto>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly ILogger<GetPermissionsQueryHandler> _logger;

    public GetPermissionsQueryHandler(
        IPermissionRepository permissionRepository,
        ILogger<GetPermissionsQueryHandler> logger)
    {
        _permissionRepository = permissionRepository;
        _logger = logger;
    }

    public async Task<Result<List<PermissionDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Yetkiler getiriliyor. Kategori: {Category}, Sadece Aktif: {OnlyActive}", 
                request.Category, request.OnlyActive);

            IEnumerable<Domain.Entities.Permission> permissions;

            if (!string.IsNullOrEmpty(request.Category))
            {
                permissions = await _permissionRepository.GetByCategoryAsync(request.Category);
            }
            else if (request.OnlyActive)
            {
                permissions = await _permissionRepository.GetActivePermissionsAsync();
            }
            else
            {
                permissions = await _permissionRepository.GetAllAsync();
            }

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

            _logger.LogInformation("{Count} yetki getirildi.", permissionDtos.Count());

            return Result.Success<List<PermissionDto>>(permissionDtos.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Yetkiler getirme sırasında hata oluştu");
            return Result.Failure<List<PermissionDto>>(Error.Problem("Permission.GetPermissionsError", "Yetkiler getirme sırasında bir hata oluştu."));
        }
    }
}
