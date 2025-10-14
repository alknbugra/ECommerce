namespace ECommerce.Application.DTOs;

/// <summary>
/// Email gönderme DTO'su
/// </summary>
public class SendEmailDto
{
    /// <summary>
    /// Alıcı email adresi
    /// </summary>
    public string ToEmail { get; set; } = string.Empty;

    /// <summary>
    /// Alıcı adı
    /// </summary>
    public string? ToName { get; set; }

    /// <summary>
    /// Email türü
    /// </summary>
    public string EmailType { get; set; } = string.Empty;

    /// <summary>
    /// İlişkili entity ID'si
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// İlişkili entity türü
    /// </summary>
    public string? RelatedEntityType { get; set; }

    /// <summary>
    /// Şablon değişkenleri
    /// </summary>
    public Dictionary<string, object>? Variables { get; set; }
}
