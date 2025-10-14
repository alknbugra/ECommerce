using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Kupon kullanımı entity'si
/// </summary>
public class CouponUsage : BaseEntity
{
    /// <summary>
    /// Kupon ID'si
    /// </summary>
    public Guid CouponId { get; set; }

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// İndirim tutarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Sipariş tutarı (indirim öncesi)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal OrderAmount { get; set; }

    /// <summary>
    /// Kullanım tarihi
    /// </summary>
    public DateTime UsedAt { get; set; } = DateTime.UtcNow;

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
    /// Kupon
    /// </summary>
    public virtual Coupon Coupon { get; set; } = null!;

    /// <summary>
    /// Kullanıcı
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Sipariş
    /// </summary>
    public virtual Order Order { get; set; } = null!;
}
