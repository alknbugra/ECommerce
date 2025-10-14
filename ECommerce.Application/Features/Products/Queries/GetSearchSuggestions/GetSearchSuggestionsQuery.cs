using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Products.Queries.GetSearchSuggestions;

/// <summary>
/// Arama önerileri sorgusu
/// </summary>
public class GetSearchSuggestionsQuery : ICachedQuery<List<SearchSuggestionDto>>
{
    /// <summary>
    /// Arama terimi
    /// </summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// Maksimum öneri sayısı
    /// </summary>
    public int Limit { get; set; } = 10;

    /// <summary>
    /// Kullanıcı ID'si (kişiselleştirme için)
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Cache anahtarı
    /// </summary>
    public string CacheKey => $"search_suggestions:{Query.ToLower()}:{Limit}";

    /// <summary>
    /// Cache süre sonu
    /// </summary>
    public TimeSpan Expiration => TimeSpan.FromMinutes(30);
}
