using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Kupon entity'si
/// </summary>
public class Coupon : BaseEntity
{
    /// <summary>
    /// Kupon kodu
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Kupon adı
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kupon açıklaması
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// İndirim türü
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string DiscountType { get; set; } = string.Empty;

    /// <summary>
    /// İndirim değeri
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountValue { get; set; }

    /// <summary>
    /// Minimum sipariş tutarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal MinimumOrderAmount { get; set; }

    /// <summary>
    /// Maksimum indirim tutarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MaximumDiscountAmount { get; set; }

    /// <summary>
    /// Kullanım limiti (toplam)
    /// </summary>
    public int? UsageLimit { get; set; }

    /// <summary>
    /// Kullanıcı başına kullanım limiti
    /// </summary>
    public int? UsageLimitPerUser { get; set; }

    /// <summary>
    /// Kullanım sayısı
    /// </summary>
    public int UsageCount { get; set; } = 0;

    /// <summary>
    /// Başlangıç tarihi
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Bitiş tarihi
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Kupon aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Sadece yeni kullanıcılar için mi?
    /// </summary>
    public bool IsForNewUsersOnly { get; set; } = false;

    /// <summary>
    /// Sadece belirli kategoriler için mi?
    /// </summary>
    public bool IsForSpecificCategories { get; set; } = false;

    /// <summary>
    /// Sadece belirli ürünler için mi?
    /// </summary>
    public bool IsForSpecificProducts { get; set; } = false;

    /// <summary>
    /// Sadece belirli kullanıcılar için mi?
    /// </summary>
    public bool IsForSpecificUsers { get; set; } = false;

    /// <summary>
    /// Kupon oluşturan kullanıcı ID'si
    /// </summary>
    public Guid? CreatedByUserId { get; set; }

    /// <summary>
    /// Kupon oluşturan kullanıcı
    /// </summary>
    public virtual User? CreatedByUser { get; set; }

    /// <summary>
    /// Kupon kullanımları
    /// </summary>
    public virtual ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();

    /// <summary>
    /// Kupon kategorileri
    /// </summary>
    public virtual ICollection<CouponCategory> CouponCategories { get; set; } = new List<CouponCategory>();

    /// <summary>
    /// Kupon ürünleri
    /// </summary>
    public virtual ICollection<CouponProduct> CouponProducts { get; set; } = new List<CouponProduct>();

    /// <summary>
    /// Kupon kullanıcıları
    /// </summary>
    public virtual ICollection<CouponUser> CouponUsers { get; set; } = new List<CouponUser>();

    /// <summary>
    /// Kupon geçerli mi?
    /// </summary>
    [NotMapped]
    public bool IsValid => IsActive && 
                          DateTime.UtcNow >= StartDate && 
                          DateTime.UtcNow <= EndDate && 
                          (UsageLimit == null || UsageCount < UsageLimit);

    /// <summary>
    /// Kupon süresi dolmuş mu?
    /// </summary>
    [NotMapped]
    public bool IsExpired => DateTime.UtcNow > EndDate;

    /// <summary>
    /// Kupon kullanım limiti dolmuş mu?
    /// </summary>
    [NotMapped]
    public bool IsUsageLimitReached => UsageLimit.HasValue && UsageCount >= UsageLimit.Value;
}
