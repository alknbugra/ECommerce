using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Messaging;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Products.Queries.AdvancedSearch;

/// <summary>
/// Gelişmiş arama sorgu handler'ı
/// </summary>
public class AdvancedSearchQueryHandler : IQueryHandler<AdvancedSearchQuery, SearchResultDto>
{
    private readonly ISearchService _searchService;
    private readonly ILogger<AdvancedSearchQueryHandler> _logger;

    public AdvancedSearchQueryHandler(
        ISearchService searchService,
        ILogger<AdvancedSearchQueryHandler> logger)
    {
        _searchService = searchService;
        _logger = logger;
    }

    public async Task<Result<SearchResultDto>> Handle(AdvancedSearchQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Gelişmiş arama başlatılıyor - Terim: {SearchTerm}, Sayfa: {PageNumber}", 
                request.SearchTerm, request.PageNumber);

            var searchDto = new AdvancedSearchDto
            {
                SearchTerm = request.SearchTerm,
                CategoryIds = request.CategoryIds,
                BrandIds = request.BrandIds,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                InStock = request.InStock,
                OnSale = request.OnSale,
                MinRating = request.MinRating,
                Attributes = request.Attributes,
                SortBy = request.SortBy,
                SortDirection = request.SortDirection,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            var result = await _searchService.SearchProductsAsync(searchDto, cancellationToken);

            // Arama geçmişini kaydet
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                await _searchService.SaveSearchHistoryAsync(
                    request.SearchTerm,
                    request.UserId,
                    request.IpAddress,
                    null, // User agent burada yok
                    result.TotalCount,
                    cancellationToken);
            }

            _logger.LogInformation("Gelişmiş arama tamamlandı - {TotalCount} sonuç bulundu, {SearchTimeMs}ms sürdü", 
                result.TotalCount, result.SearchTimeMs);

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gelişmiş arama sırasında hata oluştu");
            return Result.Failure<SearchResultDto>(Error.Failure("AdvancedSearch.Failed", 
                $"Arama sırasında hata oluştu: {ex.Message}"));
        }
    }
}
