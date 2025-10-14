using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Coupons.Queries.GetAllCoupons;

/// <summary>
/// Tüm kuponları getirme sorgusu
/// </summary>
public class GetAllCouponsQuery : IQuery<List<CouponDto>>
{
    /// <summary>
    /// Aktif kuponlar filtresi (opsiyonel)
    /// </summary>
    public bool? IsActive { get; set; }
}

/// <summary>
/// Tüm kuponları getirme sorgu işleyicisi
/// </summary>
public class GetAllCouponsQueryHandler : IQueryHandler<GetAllCouponsQuery, List<CouponDto>>
{
    private readonly ICouponService _couponService;
    private readonly ILogger<GetAllCouponsQueryHandler> _logger;

    public GetAllCouponsQueryHandler(
        ICouponService couponService,
        ILogger<GetAllCouponsQueryHandler> logger)
    {
        _couponService = couponService;
        _logger = logger;
    }

    public async Task<Result<List<CouponDto>>> Handle(GetAllCouponsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Tüm kuponları getirme sorgusu işleniyor. IsActive: {IsActive}", request.IsActive);

            var result = await _couponService.GetAllCouponsAsync(request.IsActive, cancellationToken);

            _logger.LogInformation("Tüm kuponlar başarıyla getirildi. Count: {Count}", result.Count);
            return Result.Success<List<CouponDto>>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tüm kuponları getirme sırasında hata oluştu");
            return Result.Failure<List<CouponDto>>(Error.Problem("Coupon.GetAllCouponsError", "Tüm kuponları getirme sırasında bir hata oluştu."));
        }
    }
}
