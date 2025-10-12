using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Sipariş entity'si
/// </summary>
public class Order : BaseEntity
{
    /// <summary>
    /// Sipariş numarası
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Teslimat adresi ID'si
    /// </summary>
    public Guid ShippingAddressId { get; set; }

    /// <summary>
    /// Fatura adresi ID'si
    /// </summary>
    public Guid? BillingAddressId { get; set; }

    /// <summary>
    /// Sipariş durumu
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Ödeme durumu
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string PaymentStatus { get; set; } = "Pending";

    /// <summary>
    /// Ödeme yöntemi
    /// </summary>
    [MaxLength(50)]
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// Ödeme ID'si (ödeme sağlayıcısından)
    /// </summary>
    [MaxLength(100)]
    public string? PaymentId { get; set; }

    /// <summary>
    /// Alt toplam (ürün fiyatları)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }

    /// <summary>
    /// Kargo ücreti
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingCost { get; set; } = 0;

    /// <summary>
    /// Vergi tutarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; } = 0;

    /// <summary>
    /// İndirim tutarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; } = 0;

    /// <summary>
    /// Toplam tutar
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Kargo takip numarası
    /// </summary>
    [MaxLength(100)]
    public string? TrackingNumber { get; set; }

    /// <summary>
    /// Kargo şirketi
    /// </summary>
    [MaxLength(100)]
    public string? ShippingCompany { get; set; }

    /// <summary>
    /// Sipariş notları
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Sipariş tarihi
    /// </summary>
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gönderim tarihi
    /// </summary>
    public DateTime? ShippedDate { get; set; }

    /// <summary>
    /// Teslim tarihi
    /// </summary>
    public DateTime? DeliveredDate { get; set; }

    /// <summary>
    /// Kullanıcı
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Teslimat adresi
    /// </summary>
    public virtual Address ShippingAddress { get; set; } = null!;

    /// <summary>
    /// Fatura adresi
    /// </summary>
    public virtual Address? BillingAddress { get; set; }

    /// <summary>
    /// Sipariş detayları
    /// </summary>
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    /// <summary>
    /// Sipariş durumu geçmişi
    /// </summary>
    public virtual ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
}
