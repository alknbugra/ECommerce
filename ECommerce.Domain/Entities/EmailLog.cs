using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Email log entity'si
/// </summary>
public class EmailLog : BaseEntity
{
    /// <summary>
    /// Alıcı email adresi
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string ToEmail { get; set; } = string.Empty;

    /// <summary>
    /// Alıcı adı
    /// </summary>
    [MaxLength(100)]
    public string? ToName { get; set; }

    /// <summary>
    /// Gönderen email adresi
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gönderen adı
    /// </summary>
    [MaxLength(100)]
    public string? FromName { get; set; }

    /// <summary>
    /// Email konusu
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Email içeriği
    /// </summary>
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Email durumu
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Email türü
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string EmailType { get; set; } = string.Empty;

    /// <summary>
    /// İlişkili entity ID'si (Order, User vb.)
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// İlişkili entity türü
    /// </summary>
    [MaxLength(50)]
    public string? RelatedEntityType { get; set; }

    /// <summary>
    /// Gönderim tarihi
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// Hata mesajı
    /// </summary>
    [MaxLength(1000)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Deneme sayısı
    /// </summary>
    public int RetryCount { get; set; } = 0;

    /// <summary>
    /// Maksimum deneme sayısı
    /// </summary>
    public int MaxRetryCount { get; set; } = 3;

    /// <summary>
    /// Sonraki deneme tarihi
    /// </summary>
    public DateTime? NextRetryAt { get; set; }

    /// <summary>
    /// Email sağlayıcısı yanıtı
    /// </summary>
    [MaxLength(1000)]
    public string? ProviderResponse { get; set; }

    /// <summary>
    /// Email ID'si (sağlayıcıdan)
    /// </summary>
    [MaxLength(100)]
    public string? ProviderEmailId { get; set; }
}
