using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Payments.Commands.ProcessWebhook;

/// <summary>
/// Webhook işleme komutu
/// </summary>
public class ProcessWebhookCommand : ICommand<PaymentResultDto>
{
    /// <summary>
    /// Webhook verisi
    /// </summary>
    public string WebhookData { get; set; } = string.Empty;

    /// <summary>
    /// İmza
    /// </summary>
    public string Signature { get; set; } = string.Empty;
}
