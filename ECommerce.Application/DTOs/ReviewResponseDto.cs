namespace ECommerce.Application.DTOs;

/// <summary>
/// Değerlendirme yanıtı DTO'su
/// </summary>
public class ReviewResponseDto
{
    /// <summary>
    /// Yanıt ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Değerlendirme ID'si
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Yanıtlayan kullanıcı ID'si
    /// </summary>
    public Guid RespondedByUserId { get; set; }

    /// <summary>
    /// Yanıtlayan kullanıcı adı
    /// </summary>
    public string RespondedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// Yanıt içeriği
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Yanıt türü
    /// </summary>
    public string ResponseType { get; set; } = string.Empty;

    /// <summary>
    /// Yanıt onaylandı mı?
    /// </summary>
    public bool IsApproved { get; set; }

    /// <summary>
    /// Onaylayan kullanıcı ID'si
    /// </summary>
    public Guid? ApprovedByUserId { get; set; }

    /// <summary>
    /// Onaylayan kullanıcı adı
    /// </summary>
    public string? ApprovedByUserName { get; set; }

    /// <summary>
    /// Onay tarihi
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
