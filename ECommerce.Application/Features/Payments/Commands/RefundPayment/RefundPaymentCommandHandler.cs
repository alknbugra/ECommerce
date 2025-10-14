using AutoMapper;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Payments.Commands.RefundPayment;

/// <summary>
/// Ödeme iade komut handler'ı
/// </summary>
public class RefundPaymentCommandHandler : ICommandHandler<RefundPaymentCommand, PaymentResultDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IMapper _mapper;
    private readonly ILogger<RefundPaymentCommandHandler> _logger;

    public RefundPaymentCommandHandler(
        IUnitOfWork unitOfWork,
        IPaymentGatewayService paymentGatewayService,
        IMapper mapper,
        ILogger<RefundPaymentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _paymentGatewayService = paymentGatewayService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PaymentResultDto>> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ödeme iadesi başlatıldı. PaymentId: {PaymentId}, RefundAmount: {RefundAmount}", 
                request.PaymentId, request.RefundAmount);

            // Ödemeyi kontrol et
            var payment = await _unitOfWork.Payments.GetByIdAsync(request.PaymentId);
            if (payment == null)
            {
                return Result.Failure<PaymentResultDto>(Error.NotFound("Payment.NotFound", $"Ödeme bulunamadı: {request.PaymentId}"));
            }

            // Ödeme durumunu kontrol et
            if (payment.Status != "Success")
            {
                return Result.Failure<PaymentResultDto>(Error.Problem("Payment.CannotRefund", "Sadece başarılı ödemeler iade edilebilir."));
            }

            // İade tutarını kontrol et
            if (request.RefundAmount <= 0 || request.RefundAmount > payment.Amount - payment.RefundAmount)
            {
                return Result.Failure<PaymentResultDto>(Error.Problem("Payment.InvalidRefundAmount", "İade tutarı geçersiz."));
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

            // Eğer Payment Gateway'e gönderilmişse, oradan iade et
            if (!string.IsNullOrEmpty(payment.GatewayPaymentId))
            {
                gatewayResult = await _paymentGatewayService.RefundPaymentAsync(
                    payment.GatewayPaymentId, 
                    request.RefundAmount, 
                    cancellationToken);
            }
            else
            {
                // Sadece yerel olarak iade et
                gatewayResult = new PaymentResultDto
                {
                    IsSuccess = true,
                    Status = "Refunded"
                };
            }

            if (gatewayResult.IsSuccess)
            {
                // Ödeme durumunu güncelle
                payment.RefundAmount += request.RefundAmount;
                payment.UpdatedAt = DateTime.UtcNow;

                // Tam iade mi kontrol et
                if (payment.RefundAmount >= payment.Amount)
                {
                    payment.Status = "Refunded";
                    payment.RefundedAt = DateTime.UtcNow;
                    
                    // Sipariş durumunu güncelle
                    order.Status = "Refunded";
                    order.PaymentStatus = "Refunded";
                }
                else
                {
                    payment.Status = "PartiallyRefunded";
                    
                    // Sipariş durumunu güncelle
                    order.Status = "PartiallyRefunded";
                    order.PaymentStatus = "PartiallyRefunded";
                }

                order.UpdatedAt = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync();

            // Sonucu hazırla
            var result = new PaymentResultDto
            {
                IsSuccess = gatewayResult.IsSuccess,
                PaymentId = payment.Id,
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                Status = payment.Status,
                Amount = payment.Amount,
                Currency = payment.Currency,
                GatewayPaymentId = payment.GatewayPaymentId,
                GatewayTransactionId = payment.GatewayTransactionId,
                RefundAmount = payment.RefundAmount,
                ErrorMessage = gatewayResult.ErrorMessage
            };

            _logger.LogInformation("Ödeme iadesi tamamlandı. PaymentId: {PaymentId}, RefundAmount: {RefundAmount}", 
                payment.Id, request.RefundAmount);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ödeme iadesi sırasında hata oluştu. PaymentId: {PaymentId}", payment.Id);

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
            _logger.LogError(ex, "Ödeme iadesi sırasında hata oluştu. PaymentId: {PaymentId}", request.PaymentId);
            return Result.Failure<PaymentResultDto>(Error.Problem("Payment.RefundError", "Ödeme iadesi sırasında bir hata oluştu."));
        }
    }
}
