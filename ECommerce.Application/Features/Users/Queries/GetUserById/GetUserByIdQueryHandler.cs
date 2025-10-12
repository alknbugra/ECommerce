using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// ID'ye göre kullanıcı getirme sorgu handler'ı
/// </summary>
public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetUserByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UserDto?> HandleAsync(GetUserByIdQuery request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Kullanıcı getiriliyor: {Id}", request.Id);

        var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
        if (user == null)
        {
            _logger.LogWarning("Kullanıcı bulunamadı: {Id}", request.Id);
            return null;
        }

        // Güvenlik kontrolü: Kullanıcı sadece kendi bilgilerini görebilir (Admin hariç)
        if (request.RequestingUserId.HasValue && request.RequestingUserId.Value != user.Id)
        {
            // TODO: Admin kontrolü eklenebilir
            _logger.LogWarning("Kullanıcı erişim yetkisi yok: {Id}, RequestingUserId: {RequestingUserId}", request.Id, request.RequestingUserId);
            throw new ForbiddenException("Bu kullanıcı bilgilerine erişim yetkiniz yok.");
        }

        // Kullanıcı rollerini al
        var userRoles = await _unitOfWork.UserRoles.GetUserRolesAsync(user.Id);
        var roleNames = userRoles.Select(ur => ur.Role.Name).ToList();

        _logger.LogInformation("Kullanıcı başarıyla getirildi: {Email} (ID: {Id})", user.Email, request.Id);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            IsLocked = user.IsLocked,
            IsActive = user.IsActive,
            FullName = user.FullName,
            Roles = roleNames,
            CreatedAt = user.CreatedAt
        };
    }
}
