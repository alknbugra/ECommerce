namespace ECommerce.Application.DTOs;

/// <summary>
/// Ödeme DTO'su
/// </summary>
public class PaymentDto
{
    /// <summary>
    /// Ödeme ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Sipariş numarası
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme tutarı
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Para birimi
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme durumu
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme yöntemi
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Payment Gateway ödeme ID'si
    /// </summary>
    public string? GatewayPaymentId { get; set; }

    /// <summary>
    /// Payment Gateway transaction ID'si
    /// </summary>
    public string? GatewayTransactionId { get; set; }

    /// <summary>
    /// 3D Secure durumu
    /// </summary>
    public bool Is3DSecure { get; set; }

    /// <summary>
    /// 3D Secure HTML içeriği
    /// </summary>
    public string? ThreeDSecureHtml { get; set; }

    /// <summary>
    /// Hata mesajı
    /// </summary>
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
    public decimal RefundAmount { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Güncellenme tarihi
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
