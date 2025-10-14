namespace ECommerce.Application.DTOs;

/// <summary>
/// Ürün değerlendirmesi DTO'su
/// </summary>
public class ProductReviewDto
{
    /// <summary>
    /// Değerlendirme ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Ürün adı
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Kullanıcı adı
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// Sipariş numarası
    /// </summary>
    public string? OrderNumber { get; set; }

    /// <summary>
    /// Puan (1-5 arası)
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Değerlendirme başlığı
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Değerlendirme içeriği
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Değerlendirme onaylandı mı?
    /// </summary>
    public bool IsApproved { get; set; }

    /// <summary>
    /// Değerlendirme reddedildi mi?
    /// </summary>
    public bool IsRejected { get; set; }

    /// <summary>
    /// Red sebebi
    /// </summary>
    public string? RejectionReason { get; set; }

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
    /// Yararlı bulma sayısı
    /// </summary>
    public int HelpfulCount { get; set; }

    /// <summary>
    /// Yararsız bulma sayısı
    /// </summary>
    public int NotHelpfulCount { get; set; }

    /// <summary>
    /// Net yararlılık skoru
    /// </summary>
    public int NetHelpfulScore { get; set; }

    /// <summary>
    /// Değerlendirme türü
    /// </summary>
    public string ReviewType { get; set; } = string.Empty;

    /// <summary>
    /// Değerlendirme aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Değerlendirme bekliyor mu?
    /// </summary>
    public bool IsPending { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Değerlendirme yanıtları
    /// </summary>
    public List<ReviewResponseDto>? ReviewResponses { get; set; }

    /// <summary>
    /// Değerlendirme resimleri
    /// </summary>
    public List<ReviewImageDto>? ReviewImages { get; set; }
}
