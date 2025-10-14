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

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kullanıcı şifresi değiştiriliyor: {UserId}", request.UserId);

            // Kullanıcıyı bul
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure<bool>(Error.NotFound("User", request.UserId.ToString()));
            }

            if (!user.IsActive)
            {
                return Result.Failure<bool>(Error.Validation("User", "Kullanıcı aktif değil."));
            }

            // Mevcut şifreyi doğrula
            if (!_passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                return Result.Failure<bool>(Error.Validation("CurrentPassword", "Mevcut şifre yanlış."));
            }

            // Yeni şifre güçlülük kontrolü
            if (!_passwordService.IsPasswordStrong(request.NewPassword))
            {
                return Result.Failure<bool>(Error.Validation("NewPassword", "Yeni şifre güçlülük gereksinimlerini karşılamıyor."));
            }

            // Yeni şifreyi hash'le ve kaydet
            user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Kullanıcı şifresi başarıyla değiştirildi: {Email}", user.Email);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı şifresi değiştirilirken hata oluştu: {UserId}", request.UserId);
            return Result.Failure<bool>(Error.Failure("ChangePassword.Failed", $"Şifre değiştirilirken hata oluştu: {ex.Message}"));
        }
    }
}
