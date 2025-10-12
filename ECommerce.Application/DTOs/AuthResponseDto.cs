namespace ECommerce.Application.DTOs;

/// <summary>
/// Authentication response DTO
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// Access token
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Token süresi (saniye)
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Token türü
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Kullanıcı bilgileri
    /// </summary>
    public UserDto User { get; set; } = null!;
}
