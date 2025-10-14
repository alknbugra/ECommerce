using AutoMapper;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Application.Features.Payments.Commands.ProcessWebhook;

/// <summary>
/// Webhook işleme komut handler'ı
/// </summary>
public class ProcessWebhookCommandHandler : ICommandHandler<ProcessWebhookCommand, PaymentResultDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProcessWebhookCommandHandler> _logger;

    public ProcessWebhookCommandHandler(
        IUnitOfWork unitOfWork,
        IPaymentGatewayService paymentGatewayService,
        IMapper mapper,
        ILogger<ProcessWebhookCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _paymentGatewayService = paymentGatewayService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PaymentResultDto>> Handle(ProcessWebhookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Webhook işleniyor");

            // Webhook imzasını doğrula
            var isValid = await _paymentGatewayService.VerifyWebhookAsync(
                request.WebhookData, 
                request.Signature, 
                cancellationToken);

            if (!isValid)
            {
                _logger.LogWarning("Webhook imza doğrulaması başarısız");
                return Result.Failure<PaymentResultDto>(Error.Problem("Payment.InvalidWebhookSignature", "Geçersiz webhook imzası"));
            }

            // Webhook verisini işle
            var webhookResult = await _paymentGatewayService.ProcessWebhookAsync(
                request.WebhookData, 
                cancellationToken);

            if (!webhookResult.IsSuccess)
            {
                _logger.LogWarning("Webhook işleme başarısız: {ErrorMessage}", webhookResult.ErrorMessage);
                return webhookResult;
            }

            // Webhook verisinden ödeme bilgilerini çıkar
            var webhookPayload = JsonSerializer.Deserialize<Dictionary<string, object>>(request.WebhookData);
            var gatewayPaymentId = webhookPayload?.GetValueOrDefault("paymentId")?.ToString();
            var status = webhookPayload?.GetValueOrDefault("status")?.ToString();

            if (string.IsNullOrEmpty(gatewayPaymentId))
            {
                _logger.LogWarning("Webhook'ta ödeme ID'si bulunamadı");
                throw new BadRequestException("Webhook'ta ödeme ID'si bulunamadı");
            }

            // Ödemeyi bul
            var payment = await _unitOfWork.Payments.FirstOrDefaultAsync(
                p => p.GatewayPaymentId == gatewayPaymentId);

            if (payment == null)
            {
                _logger.LogWarning("Webhook için ödeme bulunamadı. GatewayPaymentId: {GatewayPaymentId}", gatewayPaymentId);
                throw new NotFoundException("Payment", gatewayPaymentId);
            }

            // Siparişi bul
            var order = await _unitOfWork.Orders.GetByIdAsync(payment.OrderId);
            if (order == null)
            {
                throw new NotFoundException("Order", payment.OrderId);
            }

            // Ödeme durumunu güncelle
            var oldStatus = payment.Status;
            payment.Status = status ?? "Unknown";
            payment.GatewayResponse = request.WebhookData;
            payment.UpdatedAt = DateTime.UtcNow;

            // Sipariş durumunu güncelle
            if (status == "success")
            {
                payment.PaidAt = DateTime.UtcNow;
                order.Status = "Paid";
                order.PaymentStatus = "Paid";
            }
            else if (status == "failed")
            {
                order.Status = "PaymentFailed";
                order.PaymentStatus = "Failed";
            }

            order.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Webhook işlendi. PaymentId: {PaymentId}, OldStatus: {OldStatus}, NewStatus: {NewStatus}", 
                payment.Id, oldStatus, payment.Status);

            return new PaymentResultDto
            {
                IsSuccess = true,
                PaymentId = payment.Id,
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                Status = payment.Status,
                Amount = payment.Amount,
                Currency = payment.Currency,
                GatewayPaymentId = payment.GatewayPaymentId,
                GatewayTransactionId = payment.GatewayTransactionId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Webhook işleme sırasında hata oluştu");
            
            var failedResult = new PaymentResultDto
            {
                IsSuccess = false,
                Status = "Failed",
                ErrorMessage = ex.Message
            };

            return Result.Success(failedResult);
        }
    }
}
