namespace ECommerce.API.Common.Configuration;

/// <summary>
/// Serilog yapılandırma sınıfı
/// </summary>
public class SerilogConfiguration
{
    /// <summary>
    /// Serilog bölümü adı
    /// </summary>
    public const string SectionName = "Serilog";

    /// <summary>
    /// Minimum log level
    /// </summary>
    public string MinimumLevel { get; set; } = "Information";

    /// <summary>
    /// Console sink etkin mi?
    /// </summary>
    public bool EnableConsoleSink { get; set; } = true;

    /// <summary>
    /// File sink etkin mi?
    /// </summary>
    public bool EnableFileSink { get; set; } = true;

    /// <summary>
    /// Log dosyası yolu
    /// </summary>
    public string LogFilePath { get; set; } = "logs/ecommerce-.log";

    /// <summary>
    /// Log dosyası boyutu limiti (MB)
    /// </summary>
    public int FileSizeLimitMB { get; set; } = 100;

    /// <summary>
    /// Maksimum log dosyası sayısı
    /// </summary>
    public int RetainedFileCountLimit { get; set; } = 10;

    /// <summary>
    /// Rolling interval (Day, Hour, Minute)
    /// </summary>
    public string RollingInterval { get; set; } = "Day";

    /// <summary>
    /// Output template
    /// </summary>
    public string OutputTemplate { get; set; } = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// JSON formatında log yaz mı?
    /// </summary>
    public bool UseJsonFormat { get; set; } = true;

    /// <summary>
    /// Structured logging etkin mi?
    /// </summary>
    public bool EnableStructuredLogging { get; set; } = true;

    /// <summary>
    /// Request logging etkin mi?
    /// </summary>
    public bool EnableRequestLogging { get; set; } = true;

    /// <summary>
    /// Performance logging etkin mi?
    /// </summary>
    public bool EnablePerformanceLogging { get; set; } = true;

    /// <summary>
    /// Sensitive data masking etkin mi?
    /// </summary>
    public bool EnableSensitiveDataMasking { get; set; } = true;

    /// <summary>
    /// Mask edilecek alanlar
    /// </summary>
    public string[] SensitiveFields { get; set; } = new[]
    {
        "password", "token", "secret", "key", "authorization",
        "creditcard", "ssn", "email", "phone"
    };
}
