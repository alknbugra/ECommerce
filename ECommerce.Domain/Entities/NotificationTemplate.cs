using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Bildirim şablonu entity'si
/// </summary>
public class NotificationTemplate : BaseEntity
{
    /// <summary>
    /// Şablon adı
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Şablon kodu (benzersiz)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Bildirim türü
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Şablon başlığı
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string TitleTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Şablon içeriği
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string ContentTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Varsayılan öncelik
    /// </summary>
    public NotificationPriority DefaultPriority { get; set; } = NotificationPriority.Normal;

    /// <summary>
    /// Aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Açıklama
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Şablon değişkenleri (JSON formatında)
    /// </summary>
    [MaxLength(1000)]
    public string? Variables { get; set; }
}
