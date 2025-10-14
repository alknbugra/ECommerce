using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Kupon kategori entity'si
/// </summary>
public class CouponCategory : BaseEntity
{
    /// <summary>
    /// Kupon ID'si
    /// </summary>
    public Guid CouponId { get; set; }

    /// <summary>
    /// Kategori ID'si
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Kupon
    /// </summary>
    public virtual Coupon Coupon { get; set; } = null!;

    /// <summary>
    /// Kategori
    /// </summary>
    public virtual Category Category { get; set; } = null!;
}
