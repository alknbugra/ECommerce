using ECommerce.Domain.Entities;
using System.Security.Claims;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// JWT token servisi interface'i
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Kullanıcı için access token oluştur
    /// </summary>
    /// <param name="user">Kullanıcı bilgileri</param>
    /// <param name="roles">Kullanıcı rolleri</param>
    /// <returns>Access token</returns>
    string GenerateAccessToken(User user, IEnumerable<string> roles);

    /// <summary>
    /// Refresh token oluştur
    /// </summary>
    /// <returns>Refresh token</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Token'dan kullanıcı ID'sini çıkar
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Kullanıcı ID'si</returns>
    Guid? GetUserIdFromToken(string token);

    /// <summary>
    /// Token'dan kullanıcı email'ini çıkar
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Kullanıcı email'i</returns>
    string? GetUserEmailFromToken(string token);

    /// <summary>
    /// Token'dan kullanıcı rollerini çıkar
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Kullanıcı rolleri</returns>
    IEnumerable<string> GetUserRolesFromToken(string token);

    /// <summary>
    /// Token'ın geçerli olup olmadığını kontrol et
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Geçerli mi?</returns>
    bool ValidateToken(string token);

    /// <summary>
    /// Token'dan claims'leri çıkar
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Claims listesi</returns>
    IEnumerable<Claim> GetClaimsFromToken(string token);

    /// <summary>
    /// Token'ın süresinin dolup dolmadığını kontrol et
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Süresi dolmuş mu?</returns>
    bool IsTokenExpired(string token);
}
