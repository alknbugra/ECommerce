using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Users.Commands.ChangePassword;

/// <summary>
/// Şifre değiştirme komut handler'ı
/// </summary>
public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<ChangePasswordCommandHandler> _logger;

    public ChangePasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        ILogger<ChangePasswordCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _logger = logger;
    }

    public async Task<bool> HandleAsync(ChangePasswordCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Kullanıcı şifresi değiştiriliyor: {UserId}", request.UserId);

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

        // Mevcut şifreyi doğrula
        if (!_passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
        {
            throw new BadRequestException("Mevcut şifre yanlış.");
        }

        // Yeni şifre güçlülük kontrolü
        if (!_passwordService.IsPasswordStrong(request.NewPassword))
        {
            throw new BadRequestException("Yeni şifre güçlülük gereksinimlerini karşılamıyor.");
        }

        // Yeni şifreyi hash'le ve kaydet
        user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Kullanıcı şifresi başarıyla değiştirildi: {Email}", user.Email);

        return true;
    }
}
