namespace ECommerce.Application.Common.Exceptions;

/// <summary>
/// Yetkisiz erişim için exception sınıfı
/// </summary>
public class UnauthorizedException : ApplicationException
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    public UnauthorizedException(string message) : base(message, "UNAUTHORIZED")
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="innerException">İç exception</param>
    public UnauthorizedException(string message, Exception innerException) 
        : base(message, "UNAUTHORIZED", innerException)
    {
    }
}
