using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// ID'ye göre kullanıcı getirme sorgu handler'ı
/// </summary>
public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
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

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kullanıcı getiriliyor: {Id}", request.Id);

            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
            {
                _logger.LogWarning("Kullanıcı bulunamadı: {Id}", request.Id);
                return Result.Failure<UserDto>(Error.NotFound("User", request.Id.ToString()));
            }

            // Güvenlik kontrolü: Kullanıcı sadece kendi bilgilerini görebilir (Admin hariç)
            if (request.RequestingUserId.HasValue && request.RequestingUserId.Value != user.Id)
            {
                // TODO: Admin kontrolü eklenebilir
                _logger.LogWarning("Kullanıcı erişim yetkisi yok: {Id}, RequestingUserId: {RequestingUserId}", request.Id, request.RequestingUserId);
                return Result.Failure<UserDto>(Error.Forbidden("User", "Bu kullanıcı bilgilerine erişim yetkiniz yok."));
            }

            // Kullanıcı rollerini al
            var userRoles = await _unitOfWork.UserRoles.GetUserRolesAsync(user.Id);
            var roleNames = userRoles.Select(ur => ur.Role.Name).ToList();

            _logger.LogInformation("Kullanıcı başarıyla getirildi: {Email} (ID: {Id})", user.Email, request.Id);

            var userDto = new UserDto
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

            return Result.Success(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı getirilirken hata oluştu: {Id}", request.Id);
            return Result.Failure<UserDto>(Error.Failure("GetUserById.Failed", $"Kullanıcı getirilirken hata oluştu: {ex.Message}"));
        }
    }
}
