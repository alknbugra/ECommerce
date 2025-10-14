using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Ürün değerlendirmesi entity'si
/// </summary>
public class ProductReview : BaseEntity
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Sipariş ID'si (değerlendirme doğrulama için)
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// Sipariş detayı ID'si
    /// </summary>
    public Guid? OrderItemId { get; set; }

    /// <summary>
    /// Puan (1-5 arası)
    /// </summary>
    [Range(1, 5)]
    public int Rating { get; set; }

    /// <summary>
    /// Değerlendirme başlığı
    /// </summary>
    [MaxLength(200)]
    public string? Title { get; set; }

    /// <summary>
    /// Değerlendirme içeriği
    /// </summary>
    [MaxLength(2000)]
    public string? Content { get; set; }

    /// <summary>
    /// Değerlendirme onaylandı mı?
    /// </summary>
    public bool IsApproved { get; set; } = false;

    /// <summary>
    /// Değerlendirme reddedildi mi?
    /// </summary>
    public bool IsRejected { get; set; } = false;

    /// <summary>
    /// Red sebebi
    /// </summary>
    [MaxLength(500)]
    public string? RejectionReason { get; set; }

    /// <summary>
    /// Onaylayan kullanıcı ID'si
    /// </summary>
    public Guid? ApprovedByUserId { get; set; }

    /// <summary>
    /// Onay tarihi
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// Yararlı bulma sayısı
    /// </summary>
    public int HelpfulCount { get; set; } = 0;

    /// <summary>
    /// Yararsız bulma sayısı
    /// </summary>
    public int NotHelpfulCount { get; set; } = 0;

    /// <summary>
    /// Değerlendirme türü
    /// </summary>
    [MaxLength(20)]
    public string ReviewType { get; set; } = "Verified"; // Verified, Unverified, Guest

    /// <summary>
    /// Kullanıcı IP adresi
    /// </summary>
    [MaxLength(45)]
    public string? UserIpAddress { get; set; }

    /// <summary>
    /// Kullanıcı agent bilgisi
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Ürün
    /// </summary>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Kullanıcı
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Sipariş
    /// </summary>
    public virtual Order? Order { get; set; }

    /// <summary>
    /// Sipariş detayı
    /// </summary>
    public virtual OrderItem? OrderItem { get; set; }

    /// <summary>
    /// Onaylayan kullanıcı
    /// </summary>
    public virtual User? ApprovedByUser { get; set; }

    /// <summary>
    /// Değerlendirme yanıtları
    /// </summary>
    public virtual ICollection<ReviewResponse> ReviewResponses { get; set; } = new List<ReviewResponse>();

    /// <summary>
    /// Değerlendirme oyları
    /// </summary>
    public virtual ICollection<ReviewVote> ReviewVotes { get; set; } = new List<ReviewVote>();

    /// <summary>
    /// Değerlendirme resimleri
    /// </summary>
    public virtual ICollection<ReviewImage> ReviewImages { get; set; } = new List<ReviewImage>();

    /// <summary>
    /// Değerlendirme aktif mi?
    /// </summary>
    [NotMapped]
    public bool IsActive => IsApproved && !IsRejected && !IsDeleted;

    /// <summary>
    /// Değerlendirme bekliyor mu?
    /// </summary>
    [NotMapped]
    public bool IsPending => !IsApproved && !IsRejected;

    /// <summary>
    /// Net yararlılık skoru
    /// </summary>
    [NotMapped]
    public int NetHelpfulScore => HelpfulCount - NotHelpfulCount;
}
