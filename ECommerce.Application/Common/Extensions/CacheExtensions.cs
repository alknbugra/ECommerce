using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Application.Common.Extensions;

/// <summary>
/// Cache extension metodları
/// </summary>
public static class CacheExtensions
{
    /// <summary>
    /// Cache anahtarı oluştur
    /// </summary>
    /// <param name="prefix">Ön ek</param>
    /// <param name="parameters">Parametreler</param>
    /// <returns>Cache anahtarı</returns>
    public static string CreateCacheKey(string prefix, params object?[] parameters)
    {
        var keyParts = new List<string> { prefix };
        keyParts.AddRange(parameters.Select(p => p?.ToString() ?? "null"));
        return string.Join(":", keyParts);
    }

    /// <summary>
    /// Product cache anahtarı oluştur
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <returns>Cache anahtarı</returns>
    public static string CreateProductCacheKey(Guid productId)
    {
        return CreateCacheKey("product", productId);
    }

    /// <summary>
    /// Product list cache anahtarı oluştur
    /// </summary>
    /// <param name="categoryId">Kategori ID'si (opsiyonel)</param>
    /// <param name="page">Sayfa numarası</param>
    /// <param name="pageSize">Sayfa boyutu</param>
    /// <param name="searchTerm">Arama terimi (opsiyonel)</param>
    /// <returns>Cache anahtarı</returns>
    public static string CreateProductListCacheKey(Guid? categoryId = null, int page = 1, int pageSize = 10, string? searchTerm = null)
    {
        return CreateCacheKey("products", categoryId, page, pageSize, searchTerm);
    }

    /// <summary>
    /// Category cache anahtarı oluştur
    /// </summary>
    /// <param name="categoryId">Kategori ID'si</param>
    /// <returns>Cache anahtarı</returns>
    public static string CreateCategoryCacheKey(Guid categoryId)
    {
        return CreateCacheKey("category", categoryId);
    }

    /// <summary>
    /// Category list cache anahtarı oluştur
    /// </summary>
    /// <param name="parentCategoryId">Üst kategori ID'si (opsiyonel)</param>
    /// <returns>Cache anahtarı</returns>
    public static string CreateCategoryListCacheKey(Guid? parentCategoryId = null)
    {
        return CreateCacheKey("categories", parentCategoryId);
    }

    /// <summary>
    /// User cache anahtarı oluştur
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <returns>Cache anahtarı</returns>
    public static string CreateUserCacheKey(Guid userId)
    {
        return CreateCacheKey("user", userId);
    }

    /// <summary>
    /// Order cache anahtarı oluştur
    /// </summary>
    /// <param name="orderId">Sipariş ID'si</param>
    /// <returns>Cache anahtarı</returns>
    public static string CreateOrderCacheKey(Guid orderId)
    {
        return CreateCacheKey("order", orderId);
    }

    /// <summary>
    /// User orders cache anahtarı oluştur
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="page">Sayfa numarası</param>
    /// <param name="pageSize">Sayfa boyutu</param>
    /// <returns>Cache anahtarı</returns>
    public static string CreateUserOrdersCacheKey(Guid userId, int page = 1, int pageSize = 10)
    {
        return CreateCacheKey("user-orders", userId, page, pageSize);
    }

    /// <summary>
    /// Cache'den değer al veya oluştur (generic)
    /// </summary>
    /// <typeparam name="T">Değer tipi</typeparam>
    /// <param name="cacheService">Cache servisi</param>
    /// <param name="key">Cache anahtarı</param>
    /// <param name="factory">Değer oluşturma fonksiyonu</param>
    /// <param name="expiration">Süre sonu</param>
    /// <returns>Cache'deki veya yeni oluşturulan değer</returns>
    public static async Task<T> GetOrCreateAsync<T>(
        this ICacheService cacheService,
        string key,
        Func<Task<T>> factory,
        TimeSpan? expiration = null) where T : class
    {
        return await cacheService.GetOrCreateAsync(key, factory, expiration);
    }

    /// <summary>
    /// Cache'i temizle (pattern ile)
    /// </summary>
    /// <param name="cacheService">Cache servisi</param>
    /// <param name="pattern">Pattern</param>
    public static async Task InvalidateByPatternAsync(this ICacheService cacheService, string pattern)
    {
        await cacheService.RemoveByPatternAsync(pattern);
    }

    /// <summary>
    /// Product cache'ini temizle
    /// </summary>
    /// <param name="cacheService">Cache servisi</param>
    /// <param name="productId">Ürün ID'si (opsiyonel)</param>
    public static async Task InvalidateProductCacheAsync(this ICacheService cacheService, Guid? productId = null)
    {
        if (productId.HasValue)
        {
            // Belirli ürün cache'ini temizle
            await cacheService.RemoveAsync(CreateProductCacheKey(productId.Value));
        }
        
        // Tüm ürün listesi cache'lerini temizle
        await cacheService.InvalidateByPatternAsync("products:*");
    }

    /// <summary>
    /// Category cache'ini temizle
    /// </summary>
    /// <param name="cacheService">Cache servisi</param>
    /// <param name="categoryId">Kategori ID'si (opsiyonel)</param>
    public static async Task InvalidateCategoryCacheAsync(this ICacheService cacheService, Guid? categoryId = null)
    {
        if (categoryId.HasValue)
        {
            // Belirli kategori cache'ini temizle
            await cacheService.RemoveAsync(CreateCategoryCacheKey(categoryId.Value));
        }
        
        // Tüm kategori listesi cache'lerini temizle
        await cacheService.InvalidateByPatternAsync("categories:*");
    }

    /// <summary>
    /// User cache'ini temizle
    /// </summary>
    /// <param name="cacheService">Cache servisi</param>
    /// <param name="userId">Kullanıcı ID'si (opsiyonel)</param>
    public static async Task InvalidateUserCacheAsync(this ICacheService cacheService, Guid? userId = null)
    {
        if (userId.HasValue)
        {
            // Belirli kullanıcı cache'ini temizle
            await cacheService.RemoveAsync(CreateUserCacheKey(userId.Value));
            // Kullanıcının sipariş cache'lerini de temizle
            await cacheService.InvalidateByPatternAsync($"user-orders:{userId.Value}:*");
        }
        
        // Tüm kullanıcı listesi cache'lerini temizle
        await cacheService.InvalidateByPatternAsync("users:*");
    }

    /// <summary>
    /// Order cache'ini temizle
    /// </summary>
    /// <param name="cacheService">Cache servisi</param>
    /// <param name="orderId">Sipariş ID'si (opsiyonel)</param>
    /// <param name="userId">Kullanıcı ID'si (opsiyonel)</param>
    public static async Task InvalidateOrderCacheAsync(this ICacheService cacheService, Guid? orderId = null, Guid? userId = null)
    {
        if (orderId.HasValue)
        {
            // Belirli sipariş cache'ini temizle
            await cacheService.RemoveAsync(CreateOrderCacheKey(orderId.Value));
        }

        if (userId.HasValue)
        {
            // Kullanıcının sipariş listesi cache'lerini temizle
            await cacheService.InvalidateByPatternAsync($"user-orders:{userId.Value}:*");
        }
        
        // Tüm sipariş listesi cache'lerini temizle
        await cacheService.InvalidateByPatternAsync("orders:*");
    }
}
