using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Payments.Commands.RefundPayment;

/// <summary>
/// Ödeme iade komutu
/// </summary>
public class RefundPaymentCommand : ICommand<PaymentResultDto>
{
    /// <summary>
    /// Ödeme ID'si
    /// </summary>
    public Guid PaymentId { get; set; }

    /// <summary>
    /// İade tutarı
    /// </summary>
    public decimal RefundAmount { get; set; }

    /// <summary>
    /// İade nedeni
    /// </summary>
    public string? Reason { get; set; }
}
