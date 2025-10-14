using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Payments.Commands.Verify3DSecure;

/// <summary>
/// 3D Secure doğrulama komutu
/// </summary>
public class Verify3DSecureCommand : ICommand<PaymentResultDto>
{
    /// <summary>
    /// Ödeme ID'si
    /// </summary>
    public Guid PaymentId { get; set; }

    /// <summary>
    /// 3D Secure yanıtı
    /// </summary>
    public string ThreeDSecureResponse { get; set; } = string.Empty;
}
