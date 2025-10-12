namespace ECommerce.Domain.Exceptions;

/// <summary>
/// Domain katmanı için özel exception sınıfı
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Hata kodu
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    public DomainException(string message) : base(message)
    {
        ErrorCode = "DOMAIN_ERROR";
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="errorCode">Hata kodu</param>
    public DomainException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="innerException">İç exception</param>
    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = "DOMAIN_ERROR";
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="errorCode">Hata kodu</param>
    /// <param name="innerException">İç exception</param>
    public DomainException(string message, string errorCode, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
