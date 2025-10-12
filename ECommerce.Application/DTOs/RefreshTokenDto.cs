namespace ECommerce.Application.DTOs;

/// <summary>
/// Refresh token request DTO
/// </summary>
public class RefreshTokenDto
{
    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}
