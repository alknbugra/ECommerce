using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Sipariş detayı entity'si
/// </summary>
public class OrderItem : BaseEntity
{
    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Ürün adı (sipariş anındaki)
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Ürün SKU'su (sipariş anındaki)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ProductSku { get; set; } = string.Empty;

    /// <summary>
    /// Miktar
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Birim fiyat (sipariş anındaki)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Toplam fiyat
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// İndirim tutarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; } = 0;

    /// <summary>
    /// Ürün resmi URL'si (sipariş anındaki)
    /// </summary>
    [MaxLength(500)]
    public string? ProductImageUrl { get; set; }

    /// <summary>
    /// Sipariş
    /// </summary>
    public virtual Order Order { get; set; } = null!;

    /// <summary>
    /// Ürün
    /// </summary>
    public virtual Product Product { get; set; } = null!;
}
