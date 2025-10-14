using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Users.Commands.UpdateUserStatus;

/// <summary>
/// Kullanıcı durumu güncelleme komut handler'ı
/// </summary>
public class UpdateUserStatusCommandHandler : ICommandHandler<UpdateUserStatusCommand, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateUserStatusCommandHandler> _logger;

    public UpdateUserStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateUserStatusCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kullanıcı durumu güncelleniyor: {UserId}", request.UserId);

            // Kullanıcıyı bul
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure<UserDto>(Error.NotFound("User", request.UserId.ToString()));
            }

            // Durum bilgilerini güncelle
            user.IsActive = request.IsActive;
            user.IsLocked = request.IsLocked;
            user.EmailConfirmed = request.EmailConfirmed;
            user.PhoneNumberConfirmed = request.PhoneNumberConfirmed;
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Kullanıcı rollerini al
            var userRoles = await _unitOfWork.UserRoles.GetUserRolesAsync(user.Id);
            var roleNames = userRoles.Select(ur => ur.Role.Name).ToList();

            _logger.LogInformation("Kullanıcı durumu başarıyla güncellendi: {Email} - Active: {IsActive}, Locked: {IsLocked}", 
                user.Email, user.IsActive, user.IsLocked);

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
            _logger.LogError(ex, "Kullanıcı durumu güncellenirken hata oluştu: {UserId}", request.UserId);
            return Result.Failure<UserDto>(Error.Failure("UpdateUserStatus.Failed", $"Kullanıcı durumu güncellenirken hata oluştu: {ex.Message}"));
        }
    }
}
