using AutoMapper;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Payments.Commands.CreatePayment;

/// <summary>
/// Ödeme oluşturma komut handler'ı
/// </summary>
public class CreatePaymentCommandHandler : ICommandHandler<CreatePaymentCommand, PaymentResultDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IMapper _mapper;
    private readonly ILogger<CreatePaymentCommandHandler> _logger;

    public CreatePaymentCommandHandler(
        IUnitOfWork unitOfWork,
        IPaymentGatewayService paymentGatewayService,
        IMapper mapper,
        ILogger<CreatePaymentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _paymentGatewayService = paymentGatewayService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PaymentResultDto>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ödeme işlemi başlatıldı. OrderId: {OrderId}, PaymentMethod: {PaymentMethod}", 
                request.OrderId, request.PaymentMethod);

            // Siparişi kontrol et
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                return Result.Failure<PaymentResultDto>(Error.NotFound("Payment.OrderNotFound", $"Sipariş bulunamadı: {request.OrderId}"));
            }

            // Sipariş durumunu kontrol et
            if (order.Status != "Pending")
            {
                return Result.Failure<PaymentResultDto>(Error.Problem("Payment.OrderNotPending", "Bu sipariş ödeme için uygun değil."));
            }

            // Ödeme tutarını kontrol et
            if (order.TotalAmount <= 0)
            {
                return Result.Failure<PaymentResultDto>(Error.Problem("Payment.InvalidAmount", "Ödeme tutarı geçersiz."));
            }

            // Ödeme entity'si oluştur
            var payment = new Payment
            {
                OrderId = request.OrderId,
                Amount = order.TotalAmount,
                Currency = "TRY",
                Status = "Pending",
                PaymentMethod = request.PaymentMethod,
                Is3DSecure = request.Use3DSecure,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            // Payment Gateway'e ödeme isteği gönder
            var paymentRequest = new CreatePaymentDto
            {
                OrderId = request.OrderId,
                PaymentMethod = request.PaymentMethod,
                CardNumber = request.CardNumber,
                CardHolderName = request.CardHolderName,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                Cvv = request.Cvv,
                InstallmentCount = request.InstallmentCount,
                Use3DSecure = request.Use3DSecure
            };

            var gatewayResult = await _paymentGatewayService.ProcessPaymentAsync(paymentRequest, cancellationToken);

            // Payment Gateway yanıtını güncelle
            payment.Status = gatewayResult.Status;
            payment.GatewayPaymentId = gatewayResult.GatewayPaymentId;
            payment.GatewayTransactionId = gatewayResult.GatewayTransactionId;
            payment.ThreeDSecureHtml = gatewayResult.ThreeDSecureHtml;
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
                ThreeDSecureHtml = payment.ThreeDSecureHtml,
                ThreeDSecureUrl = gatewayResult.ThreeDSecureUrl,
                ErrorMessage = payment.ErrorMessage,
                PaidAt = payment.PaidAt,
                RedirectUrl = gatewayResult.RedirectUrl
            };

            _logger.LogInformation("Ödeme işlemi tamamlandı. PaymentId: {PaymentId}, Status: {Status}", 
                payment.Id, payment.Status);

            return Result.Success<PaymentResultDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ödeme işlemi sırasında hata oluştu. OrderId: {OrderId}", request.OrderId);
            return Result.Failure<PaymentResultDto>(Error.Problem("Payment.CreateError", "Ödeme işlemi sırasında bir hata oluştu."));
        }
    }
}
