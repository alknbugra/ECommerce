using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs;

/// <summary>
/// Bildirim şablonu DTO'su
/// </summary>
public class NotificationTemplateDto
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
    /// Bildirim türü
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Şablon başlığı
    /// </summary>
    public string TitleTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Şablon içeriği
    /// </summary>
    public string ContentTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Varsayılan öncelik
    /// </summary>
    public NotificationPriority DefaultPriority { get; set; }

    /// <summary>
    /// Aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Açıklama
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Şablon değişkenleri
    /// </summary>
    public string? Variables { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
