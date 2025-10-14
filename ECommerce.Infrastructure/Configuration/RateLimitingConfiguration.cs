namespace ECommerce.Infrastructure.Configuration;

/// <summary>
/// Rate limiting yapılandırma sınıfı
/// </summary>
public class RateLimitingConfiguration
{
    /// <summary>
    /// Rate limiting bölümü adı
    /// </summary>
    public const string SectionName = "RateLimiting";

    /// <summary>
    /// Rate limiting etkin mi?
    /// </summary>
    public bool EnableRateLimiting { get; set; } = true;

    /// <summary>
    /// Genel kurallar
    /// </summary>
    public List<RateLimitRule> GeneralRules { get; set; } = new();

    /// <summary>
    /// Kimlik doğrulama kuralları
    /// </summary>
    public List<RateLimitRule> AuthRules { get; set; } = new();

    /// <summary>
    /// API kuralları
    /// </summary>
    public List<RateLimitRule> ApiRules { get; set; } = new();
}

/// <summary>
/// Rate limit kuralı
/// </summary>
public class RateLimitRule
{
    /// <summary>
    /// Endpoint pattern
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Zaman periyodu (örn: 1m, 1h, 1d)
    /// </summary>
    public string Period { get; set; } = "1m";

    /// <summary>
    /// Maksimum istek sayısı
    /// </summary>
    public int Limit { get; set; } = 100;
}
