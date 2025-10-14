using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.ProductReviews.Queries.GetProductReviewStats;

/// <summary>
/// Ürün değerlendirme istatistiklerini getirme sorgusu
/// </summary>
public class GetProductReviewStatsQuery : IQuery<ProductReviewStatsDto>
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }
}

/// <summary>
/// Ürün değerlendirme istatistiklerini getirme sorgu işleyicisi
/// </summary>
public class GetProductReviewStatsQueryHandler : IQueryHandler<GetProductReviewStatsQuery, ProductReviewStatsDto>
{
    private readonly IProductReviewService _productReviewService;
    private readonly ILogger<GetProductReviewStatsQueryHandler> _logger;

    public GetProductReviewStatsQueryHandler(
        IProductReviewService productReviewService,
        ILogger<GetProductReviewStatsQueryHandler> logger)
    {
        _productReviewService = productReviewService;
        _logger = logger;
    }

    public async Task<Result<ProductReviewStatsDto>> Handle(GetProductReviewStatsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ürün değerlendirme istatistikleri getirme sorgusu işleniyor. ProductId: {ProductId}", request.ProductId);

            var result = await _productReviewService.GetProductReviewStatsAsync(request.ProductId, cancellationToken);

            _logger.LogInformation("Ürün değerlendirme istatistikleri başarıyla getirildi. ProductId: {ProductId}, TotalReviews: {TotalReviews}", 
                request.ProductId, result.TotalReviews);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirme istatistikleri getirme sırasında hata oluştu. ProductId: {ProductId}", request.ProductId);
            return Result.Failure<ProductReviewStatsDto>(Error.Failure("GetProductReviewStats.Failed", $"Ürün değerlendirme istatistikleri getirilirken hata oluştu: {ex.Message}"));
        }
    }
}
