namespace ECommerce.Application.Common.Exceptions;

/// <summary>
/// Kaynak bulunamadığında fırlatılan exception sınıfı
/// </summary>
public class NotFoundException : ApplicationException
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="resourceName">Kaynak adı</param>
    /// <param name="id">Kaynak ID'si</param>
    public NotFoundException(string resourceName, object id) 
        : base($"{resourceName} with ID '{id}' was not found.", "NOT_FOUND")
    {
        WithDetail("ResourceName", resourceName)
            .WithDetail("Id", id);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    public NotFoundException(string message) : base(message, "NOT_FOUND")
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="innerException">İç exception</param>
    public NotFoundException(string message, Exception innerException) 
        : base(message, "NOT_FOUND", innerException)
    {
    }
}
