namespace ECommerce.Application.DTOs;

/// <summary>
/// Email DTO'su
/// </summary>
public class EmailDto
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
    /// Gönderen email adresi
    /// </summary>
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gönderen adı
    /// </summary>
    public string? FromName { get; set; }

    /// <summary>
    /// Email konusu
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Email içeriği (HTML)
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Email içeriği (Plain Text)
    /// </summary>
    public string? PlainTextContent { get; set; }

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
