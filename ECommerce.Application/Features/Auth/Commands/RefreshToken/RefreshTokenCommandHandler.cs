using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// Refresh token komut handler'ı
/// </summary>
public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Refresh token işlemi başlatıldı");

            // Refresh token'ı doğrula (gerçek uygulamada veritabanında saklanacak)
            // Şimdilik basit bir kontrol yapıyoruz
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return Result.Failure<AuthResponseDto>(Error.Problem("Auth.InvalidRefreshToken", "Geçersiz refresh token."));
            }

            // Token'dan kullanıcı bilgilerini çıkar
            // Not: Gerçek uygulamada refresh token'ı veritabanında saklayıp doğrulayacağız
            // Şimdilik access token'dan kullanıcı bilgilerini alıyoruz
            
            // Bu örnekte refresh token'ı access token olarak kabul ediyoruz
            // Gerçek uygulamada ayrı bir refresh token tablosu olacak
            var userId = _jwtService.GetUserIdFromToken(request.RefreshToken);
            if (userId == null)
            {
                return Result.Failure<AuthResponseDto>(Error.Problem("Auth.InvalidRefreshToken", "Geçersiz refresh token."));
            }

            // Kullanıcıyı bul
            var user = await _unitOfWork.Users.GetByIdAsync(userId.Value);
            if (user == null || !user.IsActive)
            {
                return Result.Failure<AuthResponseDto>(Error.Problem("Auth.UserNotFound", "Kullanıcı bulunamadı veya aktif değil."));
            }

            // Kullanıcı rollerini al
            var userRoles = await _unitOfWork.UserRoles.GetUserRolesAsync(user.Id);
            var roleNames = userRoles.Select(ur => ur.Role.Name).ToList();

            // Yeni token'ları oluştur
            var newAccessToken = _jwtService.GenerateAccessToken(user, roleNames);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            _logger.LogInformation("Refresh token işlemi başarılı: {Email}", user.Email);

            var authResponse = new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                TokenType = "Bearer",
                ExpiresIn = 3600, // 1 saat
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                }
            };

            return Result.Success<AuthResponseDto>(authResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Refresh token işlemi sırasında hata oluştu");
            return Result.Failure<AuthResponseDto>(Error.Problem("Auth.RefreshTokenError", "Refresh token işlemi sırasında bir hata oluştu."));
        }
    }
}
