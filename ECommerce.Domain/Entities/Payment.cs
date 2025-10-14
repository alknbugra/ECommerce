using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Ödeme entity'si
/// </summary>
public class Payment : BaseEntity
{
    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Ödeme tutarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Para birimi
    /// </summary>
    [Required]
    [MaxLength(3)]
    public string Currency { get; set; } = "TRY";

    /// <summary>
    /// Ödeme durumu
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Ödeme yöntemi
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Payment Gateway'den gelen ödeme ID'si
    /// </summary>
    [MaxLength(100)]
    public string? GatewayPaymentId { get; set; }

    /// <summary>
    /// Payment Gateway'den gelen transaction ID'si
    /// </summary>
    [MaxLength(100)]
    public string? GatewayTransactionId { get; set; }

    /// <summary>
    /// 3D Secure durumu
    /// </summary>
    public bool Is3DSecure { get; set; } = false;

    /// <summary>
    /// 3D Secure HTML içeriği
    /// </summary>
    [Column(TypeName = "ntext")]
    public string? ThreeDSecureHtml { get; set; }

    /// <summary>
    /// Ödeme sağlayıcısından gelen yanıt
    /// </summary>
    [Column(TypeName = "ntext")]
    public string? GatewayResponse { get; set; }

    /// <summary>
    /// Hata mesajı
    /// </summary>
    [MaxLength(500)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Ödeme tarihi
    /// </summary>
    public DateTime? PaidAt { get; set; }

    /// <summary>
    /// İptal tarihi
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    /// <summary>
    /// İade tarihi
    /// </summary>
    public DateTime? RefundedAt { get; set; }

    /// <summary>
    /// İade tutarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal RefundAmount { get; set; } = 0;

    /// <summary>
    /// Sipariş
    /// </summary>
    public virtual Order Order { get; set; } = null!;
}
