using ECommerce.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Application.Common.Decorators;

/// <summary>
/// Caching decorator - Query handler'ları için cache desteği
/// </summary>
/// <typeparam name="TQuery">Query tipi</typeparam>
/// <typeparam name="TResult">Sonuç tipi</typeparam>
public class CachingDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : class
{
    private readonly IQueryHandler<TQuery, TResult> _handler;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CachingDecorator<TQuery, TResult>> _logger;

    public CachingDecorator(
        IQueryHandler<TQuery, TResult> handler,
        ICacheService cacheService,
        ILogger<CachingDecorator<TQuery, TResult>> logger)
    {
        _handler = handler;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            // Cache anahtarı oluştur
            var cacheKey = GenerateCacheKey(query);
            
            _logger.LogDebug("Cache anahtarı oluşturuldu: {CacheKey}", cacheKey);

            // Cache'den değer al
            var cachedResult = await _cacheService.GetAsync<TResult>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogDebug("Cache'den sonuç alındı: {CacheKey}", cacheKey);
                return cachedResult;
            }

            // Cache'de yoksa handler'ı çalıştır
            _logger.LogDebug("Cache'de sonuç bulunamadı, handler çalıştırılıyor: {CacheKey}", cacheKey);
            var result = await _handler.HandleAsync(query, cancellationToken);

            // Sonucu cache'e ekle
            if (result != null)
            {
                var cacheExpiration = GetCacheExpiration(query);
                await _cacheService.SetAsync(cacheKey, result, cacheExpiration);
                _logger.LogDebug("Sonuç cache'e eklendi: {CacheKey}", cacheKey);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Caching decorator'da hata oluştu: {QueryType}", typeof(TQuery).Name);
            
            // Cache hatası durumunda handler'ı direkt çalıştır
            return await _handler.HandleAsync(query, cancellationToken);
        }
    }

    /// <summary>
    /// Cache anahtarı oluştur
    /// </summary>
    /// <param name="query">Query nesnesi</param>
    /// <returns>Cache anahtarı</returns>
    private static string GenerateCacheKey(TQuery query)
    {
        var queryType = typeof(TQuery).Name;
        var queryJson = JsonSerializer.Serialize(query, new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        // JSON'dan hash oluştur
        var hash = queryJson.GetHashCode();
        return $"{queryType}:{hash}";
    }

    /// <summary>
    /// Cache süre sonu belirle
    /// </summary>
    /// <param name="query">Query nesnesi</param>
    /// <returns>Cache süre sonu</returns>
    private static TimeSpan GetCacheExpiration(TQuery query)
    {
        // Query tipine göre farklı cache süreleri
        return query switch
        {
            // Product query'leri 15 dakika cache'de kalsın
            var q when q.GetType().Name.Contains("Product") => TimeSpan.FromMinutes(15),
            
            // Category query'leri 30 dakika cache'de kalsın
            var q when q.GetType().Name.Contains("Category") => TimeSpan.FromMinutes(30),
            
            // User query'leri 10 dakika cache'de kalsın
            var q when q.GetType().Name.Contains("User") => TimeSpan.FromMinutes(10),
            
            // Order query'leri 5 dakika cache'de kalsın
            var q when q.GetType().Name.Contains("Order") => TimeSpan.FromMinutes(5),
            
            // Diğer query'ler için varsayılan 15 dakika
            _ => TimeSpan.FromMinutes(15)
        };
    }
}
