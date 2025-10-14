using ECommerce.Application.DTOs;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Payment Gateway servis interface'i
/// </summary>
public interface IPaymentGatewayService
{
    /// <summary>
    /// Ödeme işlemini başlat
    /// </summary>
    /// <param name="paymentRequest">Ödeme isteği</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Ödeme sonucu</returns>
    Task<PaymentResultDto> ProcessPaymentAsync(CreatePaymentDto paymentRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// 3D Secure doğrulaması
    /// </summary>
    /// <param name="paymentId">Ödeme ID'si</param>
    /// <param name="threeDSecureResponse">3D Secure yanıtı</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Ödeme sonucu</returns>
    Task<PaymentResultDto> Verify3DSecureAsync(Guid paymentId, string threeDSecureResponse, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ödeme durumunu sorgula
    /// </summary>
    /// <param name="gatewayPaymentId">Gateway ödeme ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Ödeme durumu</returns>
    Task<PaymentResultDto> GetPaymentStatusAsync(string gatewayPaymentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ödeme iptal et
    /// </summary>
    /// <param name="gatewayPaymentId">Gateway ödeme ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İptal sonucu</returns>
    Task<PaymentResultDto> CancelPaymentAsync(string gatewayPaymentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ödeme iade et
    /// </summary>
    /// <param name="gatewayPaymentId">Gateway ödeme ID'si</param>
    /// <param name="refundAmount">İade tutarı</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İade sonucu</returns>
    Task<PaymentResultDto> RefundPaymentAsync(string gatewayPaymentId, decimal refundAmount, CancellationToken cancellationToken = default);

    /// <summary>
    /// Webhook doğrulaması
    /// </summary>
    /// <param name="webhookData">Webhook verisi</param>
    /// <param name="signature">İmza</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Doğrulama sonucu</returns>
    Task<bool> VerifyWebhookAsync(string webhookData, string signature, CancellationToken cancellationToken = default);

    /// <summary>
    /// Webhook işleme
    /// </summary>
    /// <param name="webhookData">Webhook verisi</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İşlem sonucu</returns>
    Task<PaymentResultDto> ProcessWebhookAsync(string webhookData, CancellationToken cancellationToken = default);
}
