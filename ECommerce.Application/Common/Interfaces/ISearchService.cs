using ECommerce.Application.DTOs;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Arama servisi interface'i
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// Gelişmiş ürün arama
    /// </summary>
    /// <param name="searchDto">Arama parametreleri</param>
    /// <param name="cancellationToken">İptal token'ı</param>
    /// <returns>Arama sonucu</returns>
    Task<SearchResultDto> SearchProductsAsync(AdvancedSearchDto searchDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Arama önerileri getir
    /// </summary>
    /// <param name="query">Arama terimi</param>
    /// <param name="limit">Maksimum öneri sayısı</param>
    /// <param name="cancellationToken">İptal token'ı</param>
    /// <returns>Arama önerileri</returns>
    Task<List<SearchSuggestionDto>> GetSearchSuggestionsAsync(string query, int limit = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Popüler aramaları getir
    /// </summary>
    /// <param name="limit">Maksimum sayı</param>
    /// <param name="cancellationToken">İptal token'ı</param>
    /// <returns>Popüler aramalar</returns>
    Task<List<string>> GetPopularSearchesAsync(int limit = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Arama geçmişini kaydet
    /// </summary>
    /// <param name="searchTerm">Arama terimi</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="ipAddress">IP adresi</param>
    /// <param name="userAgent">User agent</param>
    /// <param name="resultCount">Sonuç sayısı</param>
    /// <param name="cancellationToken">İptal token'ı</param>
    Task SaveSearchHistoryAsync(string searchTerm, Guid? userId, string? ipAddress, string? userAgent, int resultCount, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ürün tıklama geçmişini kaydet
    /// </summary>
    /// <param name="searchTerm">Arama terimi</param>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="ipAddress">IP adresi</param>
    /// <param name="cancellationToken">İptal token'ı</param>
    Task SaveProductClickAsync(string searchTerm, Guid productId, Guid? userId, string? ipAddress, CancellationToken cancellationToken = default);

    /// <summary>
    /// Arama indeksini yenile
    /// </summary>
    /// <param name="cancellationToken">İptal token'ı</param>
    Task RefreshSearchIndexAsync(CancellationToken cancellationToken = default);
}
