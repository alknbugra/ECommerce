namespace ECommerce.API.Common.ProblemDetails;

/// <summary>
/// Hata yanıt modeli
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Hata kodu
    /// </summary>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>
    /// Hata mesajı
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Hata detayları
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();

    /// <summary>
    /// Validation hataları
    /// </summary>
    public List<ValidationErrorDetail> ValidationErrors { get; set; } = new();

    /// <summary>
    /// Hata zamanı
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Request ID (trace için)
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Hata tipi
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Hata başlığı
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// HTTP durum kodu
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Hata örneği URI'si
    /// </summary>
    public string? Instance { get; set; }
}

/// <summary>
/// Validation hata modeli
/// </summary>
public class ValidationErrorDetail
{
    /// <summary>
    /// Özellik adı
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Hata mesajı
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Denenen değer
    /// </summary>
    public object? AttemptedValue { get; set; }
}
