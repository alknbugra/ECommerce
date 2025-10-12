using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Users.Commands.UpdateUserProfile;

/// <summary>
/// Kullanıcı profil güncelleme komut handler'ı
/// </summary>
public class UpdateUserProfileCommandHandler : ICommandHandler<UpdateUserProfileCommand, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

    public UpdateUserProfileCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateUserProfileCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UserDto> HandleAsync(UpdateUserProfileCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Kullanıcı profili güncelleniyor: {UserId}", request.UserId);

        // Kullanıcıyı bul
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException("Kullanıcı bulunamadı.");
        }

        if (!user.IsActive)
        {
            throw new BadRequestException("Kullanıcı aktif değil.");
        }

        // Profil bilgilerini güncelle
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Kullanıcı rollerini al
        var userRoles = await _unitOfWork.UserRoles.GetUserRolesAsync(user.Id);
        var roleNames = userRoles.Select(ur => ur.Role.Name).ToList();

        _logger.LogInformation("Kullanıcı profili başarıyla güncellendi: {Email}", user.Email);

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
