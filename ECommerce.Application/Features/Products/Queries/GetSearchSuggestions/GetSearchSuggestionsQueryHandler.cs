using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Messaging;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Products.Queries.GetSearchSuggestions;

/// <summary>
/// Arama önerileri sorgu handler'ı
/// </summary>
public class GetSearchSuggestionsQueryHandler : IQueryHandler<GetSearchSuggestionsQuery, List<SearchSuggestionDto>>
{
    private readonly ISearchService _searchService;
    private readonly ILogger<GetSearchSuggestionsQueryHandler> _logger;

    public GetSearchSuggestionsQueryHandler(
        ISearchService searchService,
        ILogger<GetSearchSuggestionsQueryHandler> logger)
    {
        _searchService = searchService;
        _logger = logger;
    }

    public async Task<Result<List<SearchSuggestionDto>>> Handle(GetSearchSuggestionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Query) || request.Query.Length < 2)
            {
                return Result.Success(new List<SearchSuggestionDto>());
            }

            _logger.LogInformation("Arama önerileri getiriliyor - Terim: {Query}, Limit: {Limit}", 
                request.Query, request.Limit);

            var suggestions = await _searchService.GetSearchSuggestionsAsync(
                request.Query, 
                request.Limit, 
                cancellationToken);

            _logger.LogInformation("Arama önerileri getirildi - {Count} öneri bulundu", suggestions.Count);

            return Result.Success(suggestions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Arama önerileri getirilirken hata oluştu");
            return Result.Failure<List<SearchSuggestionDto>>(Error.Failure("GetSearchSuggestions.Failed", 
                $"Arama önerileri getirilirken hata oluştu: {ex.Message}"));
        }
    }
}
