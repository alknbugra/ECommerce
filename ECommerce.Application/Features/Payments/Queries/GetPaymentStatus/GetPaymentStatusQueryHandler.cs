using AutoMapper;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Payments.Queries.GetPaymentStatus;

/// <summary>
/// Ödeme durumu sorgu handler'ı
/// </summary>
public class GetPaymentStatusQueryHandler : IQueryHandler<GetPaymentStatusQuery, PaymentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPaymentStatusQueryHandler> _logger;

    public GetPaymentStatusQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetPaymentStatusQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PaymentDto>> Handle(GetPaymentStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ödeme durumu sorgulanıyor. PaymentId: {PaymentId}", request.PaymentId);

            var payment = await _unitOfWork.Payments.GetByIdAsync(request.PaymentId);
            if (payment == null)
            {
                return Result.Failure<PaymentDto>(Error.NotFound("Payment.NotFound", $"Ödeme bulunamadı: {request.PaymentId}"));
            }

            var result = _mapper.Map<PaymentDto>(payment);

            _logger.LogInformation("Ödeme durumu alındı. PaymentId: {PaymentId}, Status: {Status}", 
                request.PaymentId, result.Status);

            return Result.Success<PaymentDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ödeme durumu sorgulama sırasında hata oluştu. PaymentId: {PaymentId}", request.PaymentId);
            return Result.Failure<PaymentDto>(Error.Problem("Payment.GetStatusError", "Ödeme durumu sorgulama sırasında bir hata oluştu."));
        }
    }
}
