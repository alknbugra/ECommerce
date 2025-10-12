namespace ECommerce.Application.Common.Exceptions;

/// <summary>
/// Yasaklı erişim için exception sınıfı
/// </summary>
public class ForbiddenException : ApplicationException
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    public ForbiddenException(string message) : base(message, "FORBIDDEN")
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="innerException">İç exception</param>
    public ForbiddenException(string message, Exception innerException) 
        : base(message, "FORBIDDEN", innerException)
    {
    }
}
