using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Kupon kullanıcı entity'si
/// </summary>
public class CouponUser : BaseEntity
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
    /// Kupon
    /// </summary>
    public virtual Coupon Coupon { get; set; } = null!;

    /// <summary>
    /// Kullanıcı
    /// </summary>
    public virtual User User { get; set; } = null!;
}
