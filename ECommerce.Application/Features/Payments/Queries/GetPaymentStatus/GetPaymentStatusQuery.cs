using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Payments.Queries.GetPaymentStatus;

/// <summary>
/// Ödeme durumu sorgu komutu
/// </summary>
public class GetPaymentStatusQuery : IQuery<PaymentDto>
{
    /// <summary>
    /// Ödeme ID'si
    /// </summary>
    public Guid PaymentId { get; set; }
}
