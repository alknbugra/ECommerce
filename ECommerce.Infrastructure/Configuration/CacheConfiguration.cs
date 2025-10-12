namespace ECommerce.Infrastructure.Configuration;

/// <summary>
/// Cache yapılandırma sınıfı
/// </summary>
public class CacheConfiguration
{
    /// <summary>
    /// Cache bölümü adı
    /// </summary>
    public const string SectionName = "Cache";

    /// <summary>
    /// Cache etkin mi?
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Varsayılan cache süresi (dakika)
    /// </summary>
    public int DefaultExpirationMinutes { get; set; } = 30;

    /// <summary>
    /// Maksimum cache boyutu (MB)
    /// </summary>
    public int MaxSizeMB { get; set; } = 100;

    /// <summary>
    /// Cache temizleme sıklığı (dakika)
    /// </summary>
    public int CleanupIntervalMinutes { get; set; } = 5;

    /// <summary>
    /// Product cache süresi (dakika)
    /// </summary>
    public int ProductCacheMinutes { get; set; } = 15;

    /// <summary>
    /// Category cache süresi (dakika)
    /// </summary>
    public int CategoryCacheMinutes { get; set; } = 30;

    /// <summary>
    /// User cache süresi (dakika)
    /// </summary>
    public int UserCacheMinutes { get; set; } = 10;

    /// <summary>
    /// Order cache süresi (dakika)
    /// </summary>
    public int OrderCacheMinutes { get; set; } = 5;
}
