namespace ECommerce.Application.Common.Exceptions;

/// <summary>
/// Application katmanı için özel exception sınıfı
/// </summary>
public class ApplicationException : Exception
{
    /// <summary>
    /// Hata kodu
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Hata detayları
    /// </summary>
    public Dictionary<string, object> Details { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    public ApplicationException(string message) : base(message)
    {
        ErrorCode = "APPLICATION_ERROR";
        Details = new Dictionary<string, object>();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="errorCode">Hata kodu</param>
    public ApplicationException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
        Details = new Dictionary<string, object>();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="innerException">İç exception</param>
    public ApplicationException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = "APPLICATION_ERROR";
        Details = new Dictionary<string, object>();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="errorCode">Hata kodu</param>
    /// <param name="innerException">İç exception</param>
    public ApplicationException(string message, string errorCode, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
        Details = new Dictionary<string, object>();
    }

    /// <summary>
    /// Hata detayı ekle
    /// </summary>
    /// <param name="key">Anahtar</param>
    /// <param name="value">Değer</param>
    /// <returns>Exception instance</returns>
    public ApplicationException WithDetail(string key, object value)
    {
        Details[key] = value;
        return this;
    }
}
