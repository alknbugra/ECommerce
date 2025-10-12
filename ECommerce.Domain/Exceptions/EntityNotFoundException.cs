namespace ECommerce.Domain.Exceptions;

/// <summary>
/// Entity bulunamadığında fırlatılan exception sınıfı
/// </summary>
public class EntityNotFoundException : DomainException
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="entityName">Entity adı</param>
    /// <param name="id">Entity ID'si</param>
    public EntityNotFoundException(string entityName, Guid id) 
        : base($"{entityName} with ID {id} was not found.", "ENTITY_NOT_FOUND")
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="entityName">Entity adı</param>
    /// <param name="id">Entity ID'si</param>
    /// <param name="innerException">İç exception</param>
    public EntityNotFoundException(string entityName, Guid id, Exception innerException) 
        : base($"{entityName} with ID {id} was not found.", "ENTITY_NOT_FOUND", innerException)
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    public EntityNotFoundException(string message) : base(message, "ENTITY_NOT_FOUND")
    {
    }
}
