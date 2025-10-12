namespace ECommerce.Infrastructure.Configuration;

/// <summary>
/// JWT yapılandırma ayarları
/// </summary>
public class JwtConfiguration
{
    /// <summary>
    /// JWT secret key
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// JWT issuer
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// JWT audience
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Access token süresi (dakika)
    /// </summary>
    public int AccessTokenExpirationMinutes { get; set; } = 60;

    /// <summary>
    /// Refresh token süresi (gün)
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;

    /// <summary>
    /// Clock skew toleransı (dakika)
    /// </summary>
    public int ClockSkewMinutes { get; set; } = 5;
}
