using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Email şablonu entity'si
/// </summary>
public class EmailTemplate : BaseEntity
{
    /// <summary>
    /// Şablon adı
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Şablon kodu (unique)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Email konusu
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Email içeriği (HTML)
    /// </summary>
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Email içeriği (Plain Text)
    /// </summary>
    public string? PlainTextContent { get; set; }

    /// <summary>
    /// Şablon kategorisi
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Şablon aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Şablon değişkenleri (JSON format)
    /// </summary>
    [MaxLength(1000)]
    public string? Variables { get; set; }

    /// <summary>
    /// Şablon açıklaması
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }
}
