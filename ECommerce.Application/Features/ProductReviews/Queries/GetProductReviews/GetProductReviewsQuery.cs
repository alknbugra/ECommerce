using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.ProductReviews.Queries.GetProductReviews;

/// <summary>
/// Ürün değerlendirmelerini getirme sorgusu
/// </summary>
public class GetProductReviewsQuery : IQuery<List<ProductReviewDto>>
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Onaylanmış değerlendirmeler filtresi (opsiyonel)
    /// </summary>
    public bool? IsApproved { get; set; }

    /// <summary>
    /// Puan filtresi (opsiyonel)
    /// </summary>
    public int? Rating { get; set; }

    /// <summary>
    /// Sayfa numarası
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Sıralama kriteri
    /// </summary>
    public string SortBy { get; set; } = "CreatedAt";

    /// <summary>
    /// Sıralama yönü
    /// </summary>
    public string SortDirection { get; set; } = "desc";
}

/// <summary>
/// Ürün değerlendirmelerini getirme sorgu işleyicisi
/// </summary>
public class GetProductReviewsQueryHandler : IQueryHandler<GetProductReviewsQuery, List<ProductReviewDto>>
{
    private readonly IProductReviewService _productReviewService;
    private readonly ILogger<GetProductReviewsQueryHandler> _logger;

    public GetProductReviewsQueryHandler(
        IProductReviewService productReviewService,
        ILogger<GetProductReviewsQueryHandler> logger)
    {
        _productReviewService = productReviewService;
        _logger = logger;
    }

    public async Task<Result<List<ProductReviewDto>>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ürün değerlendirmeleri getirme sorgusu işleniyor. ProductId: {ProductId}, PageNumber: {PageNumber}, PageSize: {PageSize}", 
                request.ProductId, request.PageNumber, request.PageSize);

            var result = await _productReviewService.GetProductReviewsAsync(
                request.ProductId,
                request.IsApproved,
                request.Rating,
                request.PageNumber,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                cancellationToken);

            _logger.LogInformation("Ürün değerlendirmeleri başarıyla getirildi. ProductId: {ProductId}, Count: {Count}", 
                request.ProductId, result.Count);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmeleri getirme sırasında hata oluştu. ProductId: {ProductId}", request.ProductId);
            return Result.Failure<List<ProductReviewDto>>(Error.Failure("GetProductReviews.Failed", $"Ürün değerlendirmeleri getirilirken hata oluştu: {ex.Message}"));
        }
    }
}
