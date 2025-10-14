namespace ECommerce.Application.DTOs;

/// <summary>
/// Favori liste paylaşımı DTO'su
/// </summary>
public class WishlistShareDto
{
    /// <summary>
    /// Paylaşım ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Favori liste ID'si
    /// </summary>
    public Guid WishlistId { get; set; }

    /// <summary>
    /// Paylaşılan kullanıcı ID'si
    /// </summary>
    public Guid? SharedWithUserId { get; set; }

    /// <summary>
    /// Paylaşılan kullanıcı adı
    /// </summary>
    public string? SharedWithUserName { get; set; }

    /// <summary>
    /// Paylaşım türü
    /// </summary>
    public string ShareType { get; set; } = string.Empty;

    /// <summary>
    /// Paylaşım kodu
    /// </summary>
    public string ShareCode { get; set; } = string.Empty;

    /// <summary>
    /// E-posta adresi
    /// </summary>
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Paylaşım mesajı
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Paylaşım süresi (gün)
    /// </summary>
    public int? ExpirationDays { get; set; }

    /// <summary>
    /// Paylaşım sona erme tarihi
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Paylaşım aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Görüntülenme sayısı
    /// </summary>
    public int ViewCount { get; set; }

    /// <summary>
    /// Son görüntülenme tarihi
    /// </summary>
    public DateTime? LastViewedAt { get; set; }

    /// <summary>
    /// Paylaşım süresi doldu mu?
    /// </summary>
    public bool IsExpired { get; set; }

    /// <summary>
    /// Paylaşım geçerli mi?
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
