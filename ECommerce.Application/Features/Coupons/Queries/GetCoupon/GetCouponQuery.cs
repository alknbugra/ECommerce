using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace ECommerce.Application.Features.Coupons.Queries.GetCoupon;

/// <summary>
/// Kupon getirme sorgusu
/// </summary>
public class GetCouponQuery : IQuery<CouponDto>
{
    /// <summary>
    /// Kupon ID'si
    /// </summary>
    public Guid CouponId { get; set; }
}

/// <summary>
/// Kupon getirme sorgu işleyicisi
/// </summary>
public class GetCouponQueryHandler : IQueryHandler<GetCouponQuery, CouponDto>
{
    private readonly ICouponService _couponService;
    private readonly ILogger<GetCouponQueryHandler> _logger;

    public GetCouponQueryHandler(
        ICouponService couponService,
        ILogger<GetCouponQueryHandler> logger)
    {
        _couponService = couponService;
        _logger = logger;
    }

    public async Task<Result<CouponDto>> Handle(GetCouponQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kupon getirme sorgusu işleniyor. CouponId: {CouponId}", request.CouponId);

            var result = await _couponService.GetCouponAsync(request.CouponId, cancellationToken);

            if (result == null)
            {
                _logger.LogWarning("Kupon bulunamadı. CouponId: {CouponId}", request.CouponId);
                return Result.Failure<CouponDto>(Error.NotFound("Coupon.NotFound", $"Kupon bulunamadı. ID: {request.CouponId}"));
            }

            _logger.LogInformation("Kupon başarıyla getirildi. CouponId: {CouponId}, Code: {Code}", request.CouponId, result.Code);
            return Result.Success<CouponDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon getirme sırasında hata oluştu. CouponId: {CouponId}", request.CouponId);
            return Result.Failure<CouponDto>(Error.Problem("Coupon.GetCouponError", "Kupon getirme sırasında bir hata oluştu."));
        }
    }
}

/// <summary>
/// Kupon getirme sorgu validator'ı
/// </summary>
public class GetCouponQueryValidator : AbstractValidator<GetCouponQuery>
{
    public GetCouponQueryValidator()
    {
        RuleFor(x => x.CouponId)
            .NotEmpty()
            .WithMessage("Kupon ID'si boş olamaz");
    }
}
