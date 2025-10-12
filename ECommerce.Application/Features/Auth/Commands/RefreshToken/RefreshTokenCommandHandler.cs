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

    public async Task<AuthResponseDto> HandleAsync(RefreshTokenCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Refresh token işlemi başlatıldı");

        // Refresh token'ı doğrula (gerçek uygulamada veritabanında saklanacak)
        // Şimdilik basit bir kontrol yapıyoruz
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw new UnauthorizedException("Geçersiz refresh token.");
        }

        // Token'dan kullanıcı bilgilerini çıkar
        // Not: Gerçek uygulamada refresh token'ı veritabanında saklayıp doğrulayacağız
        // Şimdilik access token'dan kullanıcı bilgilerini alıyoruz
        
        // Bu örnekte refresh token'ı access token olarak kabul ediyoruz
        // Gerçek uygulamada ayrı bir refresh token tablosu olacak
        var userId = _jwtService.GetUserIdFromToken(request.RefreshToken);
        if (userId == null)
        {
            throw new UnauthorizedException("Geçersiz refresh token.");
        }

        // Kullanıcıyı bul
        var user = await _unitOfWork.Users.GetByIdAsync(userId.Value);
        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedException("Kullanıcı bulunamadı veya aktif değil.");
        }

        // Kullanıcı rollerini al
        var userRoles = await _unitOfWork.UserRoles.GetUserRolesAsync(user.Id);
        var roleNames = userRoles.Select(ur => ur.Role.Name).ToList();

        // Yeni token'ları oluştur
        var newAccessToken = _jwtService.GenerateAccessToken(user, roleNames);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        _logger.LogInformation("Refresh token işlemi başarılı: {Email}", user.Email);

        return new AuthResponseDto
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
    }
}
