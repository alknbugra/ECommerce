namespace ECommerce.API.Common.Configuration;

/// <summary>
/// OpenSearch yapılandırma sınıfı
/// </summary>
public class OpenSearchConfiguration
{
    /// <summary>
    /// OpenSearch bölümü adı
    /// </summary>
    public const string SectionName = "OpenSearch";

    /// <summary>
    /// OpenSearch etkin mi?
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// OpenSearch node URL'leri
    /// </summary>
    public string[] NodeUris { get; set; } = new[] { "http://localhost:9200" };

    /// <summary>
    /// Index adı
    /// </summary>
    public string IndexFormat { get; set; } = "ecommerce-logs-{0:yyyy.MM.dd}";

    /// <summary>
    /// Index template adı
    /// </summary>
    public string IndexTemplateName { get; set; } = "ecommerce-logs-template";

    /// <summary>
    /// Authentication kullanıcı adı
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Authentication şifre
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// API key authentication
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Certificate fingerprint (HTTPS için)
    /// </summary>
    public string? CertificateFingerprint { get; set; }

    /// <summary>
    /// SSL doğrulaması etkin mi?
    /// </summary>
    public bool VerifySsl { get; set; } = true;

    /// <summary>
    /// Connection timeout (saniye)
    /// </summary>
    public int ConnectionTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Request timeout (saniye)
    /// </summary>
    public int RequestTimeoutSeconds { get; set; } = 60;

    /// <summary>
    /// Batch size
    /// </summary>
    public int BatchSize { get; set; } = 1000;

    /// <summary>
    /// Batch posting interval (saniye)
    /// </summary>
    public int BatchPostingIntervalSeconds { get; set; } = 2;

    /// <summary>
    /// Queue size limit
    /// </summary>
    public int QueueSizeLimit { get; set; } = 10000;

    /// <summary>
    /// Auto register template
    /// </summary>
    public bool AutoRegisterTemplate { get; set; } = true;

    /// <summary>
    /// Template lifetime (gün)
    /// </summary>
    public int TemplateLifetimeDays { get; set; } = 30;

    /// <summary>
    /// Number of shards
    /// </summary>
    public int NumberOfShards { get; set; } = 1;

    /// <summary>
    /// Number of replicas
    /// </summary>
    public int NumberOfReplicas { get; set; } = 0;

    /// <summary>
    /// Index refresh interval
    /// </summary>
    public string IndexRefreshInterval { get; set; } = "5s";

    /// <summary>
    /// Index lifecycle management etkin mi?
    /// </summary>
    public bool EnableIndexLifecycleManagement { get; set; } = true;

    /// <summary>
    /// Index retention period (gün)
    /// </summary>
    public int IndexRetentionDays { get; set; } = 30;

    /// <summary>
    /// Buffer size (MB)
    /// </summary>
    public int BufferSizeMB { get; set; } = 10;

    /// <summary>
    /// Flush interval (saniye)
    /// </summary>
    public int FlushIntervalSeconds { get; set; } = 5;

    /// <summary>
    /// Retry count
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Retry delay (saniye)
    /// </summary>
    public int RetryDelaySeconds { get; set; } = 1;

    /// <summary>
    /// Dead letter queue etkin mi?
    /// </summary>
    public bool EnableDeadLetterQueue { get; set; } = true;

    /// <summary>
    /// Dead letter queue path
    /// </summary>
    public string DeadLetterQueuePath { get; set; } = "logs/dead-letter-queue";

    /// <summary>
    /// Custom fields
    /// </summary>
    public Dictionary<string, object> CustomFields { get; set; } = new()
    {
        ["Application"] = "ECommerce.API",
        ["Environment"] = "Development",
        ["Version"] = "1.0.0"
    };
}
