namespace ECommerce.Application.DTOs;

/// <summary>
/// Ödeme sonucu DTO'su
/// </summary>
public class PaymentResultDto
{
    /// <summary>
    /// Ödeme başarılı mı?
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Ödeme ID'si
    /// </summary>
    public Guid PaymentId { get; set; }

    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Sipariş numarası
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme durumu
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme tutarı
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Para birimi
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Payment Gateway ödeme ID'si
    /// </summary>
    public string? GatewayPaymentId { get; set; }

    /// <summary>
    /// Payment Gateway transaction ID'si
    /// </summary>
    public string? GatewayTransactionId { get; set; }

    /// <summary>
    /// 3D Secure HTML içeriği (3D Secure gerekliyse)
    /// </summary>
    public string? ThreeDSecureHtml { get; set; }

    /// <summary>
    /// 3D Secure URL'i (3D Secure gerekliyse)
    /// </summary>
    public string? ThreeDSecureUrl { get; set; }

    /// <summary>
    /// Hata mesajı
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Ödeme tarihi
    /// </summary>
    public DateTime? PaidAt { get; set; }

    /// <summary>
    /// Redirect URL'i (başarılı ödeme sonrası)
    /// </summary>
    public string? RedirectUrl { get; set; }

    /// <summary>
    /// İade tutarı
    /// </summary>
    public decimal RefundAmount { get; set; }
}
