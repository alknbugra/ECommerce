using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Arama endpoint'leri
/// </summary>
public static class SearchEndpoints
{
    /// <summary>
    /// Arama endpoint'lerini kaydet
    /// </summary>
    public static void MapSearchEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/search")
            .WithTags("Search")
            .WithOpenApi();

        // Gelişmiş arama
        group.MapPost("/products", SearchProducts)
            .WithName("SearchProducts")
            .WithSummary("Gelişmiş ürün arama")
            .WithDescription("Filtreleme, sıralama ve sayfalama ile ürün arama")
            .Produces<SearchResultDto>(200)
            .Produces(400)
            .Produces(500);

        // Arama önerileri
        group.MapGet("/suggestions", GetSearchSuggestions)
            .WithName("GetSearchSuggestions")
            .WithSummary("Arama önerileri")
            .WithDescription("Kullanıcı yazarken arama önerileri getirir")
            .Produces<List<SearchSuggestionDto>>(200)
            .Produces(400)
            .Produces(500);

        // Popüler aramalar
        group.MapGet("/popular", GetPopularSearches)
            .WithName("GetPopularSearches")
            .WithSummary("Popüler aramalar")
            .WithDescription("En popüler arama terimlerini getirir")
            .Produces<List<string>>(200)
            .Produces(500);

        // Ürün tıklama kaydetme
        group.MapPost("/click", SaveProductClick)
            .WithName("SaveProductClick")
            .WithSummary("Ürün tıklama kaydet")
            .WithDescription("Arama sonucunda tıklanan ürünü kaydeder")
            .Produces(200)
            .Produces(400)
            .Produces(500);
    }

    /// <summary>
    /// Gelişmiş ürün arama
    /// </summary>
    private static async Task<IResult> SearchProducts(
        [FromBody] AdvancedSearchDto searchDto,
        [FromServices] ISearchService searchService,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken)
    {
        var result = await searchService.SearchProductsAsync(searchDto, cancellationToken);

        return Results.Ok(result);
    }

    /// <summary>
    /// Arama önerileri getir
    /// </summary>
    private static async Task<IResult> GetSearchSuggestions(
        [FromQuery] string query,
        [FromServices] ISearchService searchService,
        [FromQuery] int limit = 10,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
        {
            return Results.Ok(new List<SearchSuggestionDto>());
        }

        var suggestions = await searchService.GetSearchSuggestionsAsync(query, limit, cancellationToken);

        return Results.Ok(suggestions);
    }

    /// <summary>
    /// Popüler aramalar getir
    /// </summary>
    private static async Task<IResult> GetPopularSearches(
        [FromServices] ISearchService searchService,
        [FromQuery] int limit = 10,
        CancellationToken cancellationToken = default)
    {
        var popularSearches = await searchService.GetPopularSearchesAsync(limit, cancellationToken);
        return Results.Ok(popularSearches);
    }

    /// <summary>
    /// Ürün tıklama kaydet
    /// </summary>
    private static async Task<IResult> SaveProductClick(
        [FromBody] SaveProductClickDto dto,
        [FromServices] ISearchService searchService,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken)
    {
        var ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        
        await searchService.SaveProductClickAsync(
            dto.SearchTerm,
            dto.ProductId,
            dto.UserId,
            ipAddress,
            cancellationToken);

        return Results.Ok();
    }
}

/// <summary>
/// Ürün tıklama DTO'su
/// </summary>
public class SaveProductClickDto
{
    /// <summary>
    /// Arama terimi
    /// </summary>
    public string SearchTerm { get; set; } = string.Empty;

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid? UserId { get; set; }
}
