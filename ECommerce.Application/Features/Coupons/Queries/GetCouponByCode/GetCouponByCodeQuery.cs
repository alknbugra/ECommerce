using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace ECommerce.Application.Features.Coupons.Queries.GetCouponByCode;

/// <summary>
/// Kupon kodu ile kupon getirme sorgusu
/// </summary>
public class GetCouponByCodeQuery : IQuery<CouponDto>
{
    /// <summary>
    /// Kupon kodu
    /// </summary>
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Kupon kodu ile kupon getirme sorgu işleyicisi
/// </summary>
public class GetCouponByCodeQueryHandler : IQueryHandler<GetCouponByCodeQuery, CouponDto>
{
    private readonly ICouponService _couponService;
    private readonly ILogger<GetCouponByCodeQueryHandler> _logger;

    public GetCouponByCodeQueryHandler(
        ICouponService couponService,
        ILogger<GetCouponByCodeQueryHandler> logger)
    {
        _couponService = couponService;
        _logger = logger;
    }

    public async Task<Result<CouponDto>> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kupon kodu ile kupon getirme sorgusu işleniyor. Code: {Code}", request.Code);

            var result = await _couponService.GetCouponByCodeAsync(request.Code, cancellationToken);

            if (result == null)
            {
                _logger.LogWarning("Kupon bulunamadı. Code: {Code}", request.Code);
                return Result.Failure<CouponDto>(Error.NotFound("Coupon.NotFound", $"Kupon bulunamadı. Kod: {request.Code}"));
            }

            _logger.LogInformation("Kupon başarıyla getirildi. Code: {Code}, Id: {Id}", request.Code, result.Id);
            return Result.Success<CouponDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon kodu ile kupon getirme sırasında hata oluştu. Code: {Code}", request.Code);
            return Result.Failure<CouponDto>(Error.Problem("Coupon.GetCouponByCodeError", "Kupon kodu ile kupon getirme sırasında bir hata oluştu."));
        }
    }
}

/// <summary>
/// Kupon kodu ile kupon getirme sorgu validator'ı
/// </summary>
public class GetCouponByCodeQueryValidator : AbstractValidator<GetCouponByCodeQuery>
{
    public GetCouponByCodeQueryValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Kupon kodu boş olamaz")
            .MaximumLength(50)
            .WithMessage("Kupon kodu en fazla 50 karakter olabilir");
    }
}
