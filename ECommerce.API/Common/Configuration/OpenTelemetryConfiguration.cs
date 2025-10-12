namespace ECommerce.API.Common.Configuration;

/// <summary>
/// OpenTelemetry yapılandırma sınıfı
/// </summary>
public class OpenTelemetryConfiguration
{
    /// <summary>
    /// OpenTelemetry bölümü adı
    /// </summary>
    public const string SectionName = "OpenTelemetry";

    /// <summary>
    /// OpenTelemetry etkin mi?
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Service name
    /// </summary>
    public string ServiceName { get; set; } = "ECommerce.API";

    /// <summary>
    /// Service version
    /// </summary>
    public string ServiceVersion { get; set; } = "1.0.0";

    /// <summary>
    /// Service namespace
    /// </summary>
    public string ServiceNamespace { get; set; } = "ECommerce";

    /// <summary>
    /// OTLP endpoint (Jaeger, Zipkin, etc.)
    /// </summary>
    public string? OtlpEndpoint { get; set; }

    /// <summary>
    /// Console exporter etkin mi?
    /// </summary>
    public bool EnableConsoleExporter { get; set; } = true;

    /// <summary>
    /// ASP.NET Core instrumentation etkin mi?
    /// </summary>
    public bool EnableAspNetCoreInstrumentation { get; set; } = true;

    /// <summary>
    /// Entity Framework Core instrumentation etkin mi?
    /// </summary>
    public bool EnableEntityFrameworkCoreInstrumentation { get; set; } = true;

    /// <summary>
    /// HTTP client instrumentation etkin mi?
    /// </summary>
    public bool EnableHttpClientInstrumentation { get; set; } = true;

    /// <summary>
    /// SQL client instrumentation etkin mi?
    /// </summary>
    public bool EnableSqlClientInstrumentation { get; set; } = true;

    /// <summary>
    /// Sampling ratio (0.0 - 1.0)
    /// </summary>
    public double SamplingRatio { get; set; } = 1.0;

    /// <summary>
    /// Maximum activities per second
    /// </summary>
    public int MaxActivitiesPerSecond { get; set; } = 1000;

    /// <summary>
    /// Maximum events per activity
    /// </summary>
    public int MaxEventsPerActivity { get; set; } = 100;

    /// <summary>
    /// Maximum links per activity
    /// </summary>
    public int MaxLinksPerActivity { get; set; } = 100;

    /// <summary>
    /// Maximum attributes per activity
    /// </summary>
    public int MaxAttributesPerActivity { get; set; } = 1000;
}
