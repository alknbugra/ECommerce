using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Kupon ürün entity'si
/// </summary>
public class CouponProduct : BaseEntity
{
    /// <summary>
    /// Kupon ID'si
    /// </summary>
    public Guid CouponId { get; set; }

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Kupon
    /// </summary>
    public virtual Coupon Coupon { get; set; } = null!;

    /// <summary>
    /// Ürün
    /// </summary>
    public virtual Product Product { get; set; } = null!;
}
