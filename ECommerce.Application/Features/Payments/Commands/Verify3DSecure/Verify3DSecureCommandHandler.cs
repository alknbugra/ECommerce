using AutoMapper;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Payments.Commands.Verify3DSecure;

/// <summary>
/// 3D Secure doğrulama komut handler'ı
/// </summary>
public class Verify3DSecureCommandHandler : ICommandHandler<Verify3DSecureCommand, PaymentResultDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IMapper _mapper;
    private readonly ILogger<Verify3DSecureCommandHandler> _logger;

    public Verify3DSecureCommandHandler(
        IUnitOfWork unitOfWork,
        IPaymentGatewayService paymentGatewayService,
        IMapper mapper,
        ILogger<Verify3DSecureCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _paymentGatewayService = paymentGatewayService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PaymentResultDto>> Handle(Verify3DSecureCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("3D Secure doğrulaması başlatıldı. PaymentId: {PaymentId}", request.PaymentId);

            // Ödemeyi kontrol et
            var payment = await _unitOfWork.Payments.GetByIdAsync(request.PaymentId);
            if (payment == null)
            {
                return Result.Failure<PaymentResultDto>(Error.NotFound("Payment.NotFound", $"Ödeme bulunamadı: {request.PaymentId}"));
            }

            // Ödeme durumunu kontrol et
            if (payment.Status != "WaitingFor3DSecure")
            {
                return Result.Failure<PaymentResultDto>(Error.Problem("Payment.NotWaitingFor3DSecure", "Bu ödeme 3D Secure doğrulaması için uygun değil."));
            }

            // Siparişi kontrol et
            var order = await _unitOfWork.Orders.GetByIdAsync(payment.OrderId);
            if (order == null)
            {
                return Result.Failure<PaymentResultDto>(Error.NotFound("Payment.OrderNotFound", $"Sipariş bulunamadı: {payment.OrderId}"));
            }

        try
        {
            // Payment Gateway'e 3D Secure doğrulama isteği gönder
            var gatewayResult = await _paymentGatewayService.Verify3DSecureAsync(
                request.PaymentId, 
                request.ThreeDSecureResponse, 
                cancellationToken);

            // Payment Gateway yanıtını güncelle
            payment.Status = gatewayResult.Status;
            payment.GatewayPaymentId = gatewayResult.GatewayPaymentId;
            payment.GatewayTransactionId = gatewayResult.GatewayTransactionId;
            payment.ErrorMessage = gatewayResult.ErrorMessage;
            payment.UpdatedAt = DateTime.UtcNow;

            if (gatewayResult.IsSuccess)
            {
                payment.PaidAt = DateTime.UtcNow;
                
                // Sipariş durumunu güncelle
                order.Status = "Paid";
                order.PaymentStatus = "Paid";
                order.PaymentId = gatewayResult.GatewayPaymentId;
                order.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                // Sipariş durumunu güncelle
                order.Status = "PaymentFailed";
                order.PaymentStatus = "Failed";
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
                ErrorMessage = payment.ErrorMessage,
                PaidAt = payment.PaidAt,
                RedirectUrl = gatewayResult.RedirectUrl
            };

            _logger.LogInformation("3D Secure doğrulaması tamamlandı. PaymentId: {PaymentId}, Status: {Status}", 
                payment.Id, payment.Status);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "3D Secure doğrulaması sırasında hata oluştu. PaymentId: {PaymentId}", payment.Id);

            // Hata durumunda ödeme ve sipariş durumunu güncelle
            payment.Status = "Failed";
            payment.ErrorMessage = ex.Message;
            payment.UpdatedAt = DateTime.UtcNow;

            order.Status = "PaymentFailed";
            order.PaymentStatus = "Failed";
            order.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

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
            _logger.LogError(ex, "3D Secure doğrulaması sırasında hata oluştu. PaymentId: {PaymentId}", request.PaymentId);
            return Result.Failure<PaymentResultDto>(Error.Problem("Payment.Verify3DSecureError", "3D Secure doğrulaması sırasında bir hata oluştu."));
        }
    }
}
