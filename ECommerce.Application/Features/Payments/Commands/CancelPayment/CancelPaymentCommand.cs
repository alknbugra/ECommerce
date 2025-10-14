using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Payments.Commands.CancelPayment;

/// <summary>
/// Ödeme iptal komutu
/// </summary>
public class CancelPaymentCommand : ICommand<PaymentResultDto>
{
    /// <summary>
    /// Ödeme ID'si
    /// </summary>
    public Guid PaymentId { get; set; }
}
