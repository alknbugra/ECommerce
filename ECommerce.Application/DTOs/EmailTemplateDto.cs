namespace ECommerce.Application.DTOs;

/// <summary>
/// Email şablonu DTO'su
/// </summary>
public class EmailTemplateDto
{
    /// <summary>
    /// Şablon ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Şablon adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Şablon kodu
    /// </summary>
    public string Code { get; set; } = string.Empty;

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
    /// Şablon kategorisi
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Şablon aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Şablon değişkenleri
    /// </summary>
    public string? Variables { get; set; }

    /// <summary>
    /// Şablon açıklaması
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
