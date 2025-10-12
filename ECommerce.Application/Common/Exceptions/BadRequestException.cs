namespace ECommerce.Application.Common.Exceptions;

/// <summary>
/// Geçersiz istek için exception sınıfı
/// </summary>
public class BadRequestException : ApplicationException
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    public BadRequestException(string message) : base(message, "BAD_REQUEST")
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="innerException">İç exception</param>
    public BadRequestException(string message, Exception innerException) 
        : base(message, "BAD_REQUEST", innerException)
    {
    }
}
