using AutoMapper;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Payments.Commands.CancelPayment;

/// <summary>
/// Ödeme iptal komut handler'ı
/// </summary>
public class CancelPaymentCommandHandler : ICommandHandler<CancelPaymentCommand, PaymentResultDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelPaymentCommandHandler> _logger;

    public CancelPaymentCommandHandler(
        IUnitOfWork unitOfWork,
        IPaymentGatewayService paymentGatewayService,
        IMapper mapper,
        ILogger<CancelPaymentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _paymentGatewayService = paymentGatewayService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PaymentResultDto>> Handle(CancelPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ödeme iptali başlatıldı. PaymentId: {PaymentId}", request.PaymentId);

            // Ödemeyi kontrol et
            var payment = await _unitOfWork.Payments.GetByIdAsync(request.PaymentId);
            if (payment == null)
            {
                return Result.Failure<PaymentResultDto>(Error.NotFound("Payment.NotFound", $"Ödeme bulunamadı: {request.PaymentId}"));
            }

            // Ödeme durumunu kontrol et
            if (payment.Status == "Success" || payment.Status == "Cancelled")
            {
                return Result.Failure<PaymentResultDto>(Error.Problem("Payment.CannotCancel", "Bu ödeme iptal edilemez."));
            }

            // Siparişi kontrol et
            var order = await _unitOfWork.Orders.GetByIdAsync(payment.OrderId);
            if (order == null)
            {
                return Result.Failure<PaymentResultDto>(Error.NotFound("Payment.OrderNotFound", $"Sipariş bulunamadı: {payment.OrderId}"));
            }

        try
        {
            PaymentResultDto gatewayResult;

            // Eğer Payment Gateway'e gönderilmişse, oradan iptal et
            if (!string.IsNullOrEmpty(payment.GatewayPaymentId))
            {
                gatewayResult = await _paymentGatewayService.CancelPaymentAsync(
                    payment.GatewayPaymentId, 
                    cancellationToken);
            }
            else
            {
                // Sadece yerel olarak iptal et
                gatewayResult = new PaymentResultDto
                {
                    IsSuccess = true,
                    Status = "Cancelled"
                };
            }

            // Ödeme durumunu güncelle
            payment.Status = "Cancelled";
            payment.CancelledAt = DateTime.UtcNow;
            payment.UpdatedAt = DateTime.UtcNow;

            // Sipariş durumunu güncelle
            order.Status = "Cancelled";
            order.PaymentStatus = "Cancelled";
            order.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            // Sonucu hazırla
            var result = new PaymentResultDto
            {
                IsSuccess = true,
                PaymentId = payment.Id,
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                Status = "Cancelled",
                Amount = payment.Amount,
                Currency = payment.Currency,
                GatewayPaymentId = payment.GatewayPaymentId,
                GatewayTransactionId = payment.GatewayTransactionId
            };

            _logger.LogInformation("Ödeme iptali tamamlandı. PaymentId: {PaymentId}", payment.Id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ödeme iptali sırasında hata oluştu. PaymentId: {PaymentId}", payment.Id);

            var failedResult = new PaymentResultDto
            {
                IsSuccess = false,
                PaymentId = payment.Id,
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                Status = "Failed",
                Amount = payment.Amount,
                Currency = payment.Currency,
                ErrorMessage = ex.Message
            };

            return Result.Success<PaymentResultDto>(failedResult);
        }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ödeme iptali sırasında hata oluştu. PaymentId: {PaymentId}", request.PaymentId);
            return Result.Failure<PaymentResultDto>(Error.Problem("Payment.CancelError", "Ödeme iptali sırasında bir hata oluştu."));
        }
    }
}
