namespace ECommerce.Domain.Exceptions;

/// <summary>
/// İş kuralları ihlali için exception sınıfı
/// </summary>
public class BusinessRuleException : DomainException
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    public BusinessRuleException(string message) : base(message, "BUSINESS_RULE_VIOLATION")
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="innerException">İç exception</param>
    public BusinessRuleException(string message, Exception innerException) : base(message, "BUSINESS_RULE_VIOLATION", innerException)
    {
    }
}
