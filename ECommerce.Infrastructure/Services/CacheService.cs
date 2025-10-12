using ECommerce.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace ECommerce.Infrastructure.Services;

/// <summary>
/// Memory cache servisi implementasyonu
/// </summary>
public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CacheService> _logger;
    private readonly ConcurrentDictionary<string, bool> _keys;

    public CacheService(IMemoryCache memoryCache, ILogger<CacheService> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _keys = new ConcurrentDictionary<string, bool>();
    }

    public T? Get<T>(string key) where T : class
    {
        try
        {
            _logger.LogDebug("Cache'den değer alınıyor: {Key}", key);
            
            if (_memoryCache.TryGetValue(key, out var value) && value is T typedValue)
            {
                _logger.LogDebug("Cache'den değer bulundu: {Key}", key);
                return typedValue;
            }

            _logger.LogDebug("Cache'de değer bulunamadı: {Key}", key);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache'den değer alınırken hata oluştu: {Key}", key);
            return null;
        }
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        return await Task.FromResult(Get<T>(key));
    }

    public void Set<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        try
        {
            _logger.LogDebug("Cache'e değer ekleniyor: {Key}", key);

            var options = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.Normal,
                SlidingExpiration = expiration ?? TimeSpan.FromMinutes(30)
            };

            _memoryCache.Set(key, value, options);
            _keys.TryAdd(key, true);

            _logger.LogDebug("Cache'e değer eklendi: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache'e değer eklenirken hata oluştu: {Key}", key);
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        await Task.Run(() => Set(key, value, expiration));
    }

    public void Remove(string key)
    {
        try
        {
            _logger.LogDebug("Cache'den değer siliniyor: {Key}", key);
            
            _memoryCache.Remove(key);
            _keys.TryRemove(key, out _);

            _logger.LogDebug("Cache'den değer silindi: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache'den değer silinirken hata oluştu: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        await Task.Run(() => Remove(key));
    }

    public void RemoveByPattern(string pattern)
    {
        try
        {
            _logger.LogDebug("Cache'den pattern ile değerler siliniyor: {Pattern}", pattern);

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var keysToRemove = _keys.Keys.Where(key => regex.IsMatch(key)).ToList();

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
                _keys.TryRemove(key, out _);
            }

            _logger.LogDebug("Cache'den {Count} değer silindi: {Pattern}", keysToRemove.Count, pattern);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache'den pattern ile değerler silinirken hata oluştu: {Pattern}", pattern);
        }
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        await Task.Run(() => RemoveByPattern(pattern));
    }

    public bool Exists(string key)
    {
        try
        {
            return _memoryCache.TryGetValue(key, out _);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache'de değer varlığı kontrol edilirken hata oluştu: {Key}", key);
            return false;
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await Task.FromResult(Exists(key));
    }

    public void Clear()
    {
        try
        {
            _logger.LogDebug("Cache temizleniyor");

            // MemoryCache'i temizlemek için dispose edip yeniden oluşturmak gerekir
            // Bu durumda sadece tracked key'leri temizleyelim
            foreach (var key in _keys.Keys.ToList())
            {
                _memoryCache.Remove(key);
            }
            _keys.Clear();

            _logger.LogDebug("Cache temizlendi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache temizlenirken hata oluştu");
        }
    }

    public async Task ClearAsync()
    {
        await Task.Run(() => Clear());
    }

    public T GetOrCreate<T>(string key, Func<T> factory, TimeSpan? expiration = null) where T : class
    {
        try
        {
            _logger.LogDebug("Cache'den değer alınıyor veya oluşturuluyor: {Key}", key);

            if (_memoryCache.TryGetValue(key, out var value) && value is T cachedValue)
            {
                _logger.LogDebug("Cache'den değer bulundu: {Key}", key);
                return cachedValue;
            }

            _logger.LogDebug("Cache'de değer bulunamadı, yeni değer oluşturuluyor: {Key}", key);
            var newValue = factory();
            Set(key, newValue, expiration);

            return newValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache'den değer alınırken veya oluşturulurken hata oluştu: {Key}", key);
            return factory();
        }
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null) where T : class
    {
        try
        {
            _logger.LogDebug("Cache'den değer alınıyor veya oluşturuluyor (async): {Key}", key);

            if (_memoryCache.TryGetValue(key, out var value) && value is T cachedValue)
            {
                _logger.LogDebug("Cache'den değer bulundu: {Key}", key);
                return cachedValue;
            }

            _logger.LogDebug("Cache'de değer bulunamadı, yeni değer oluşturuluyor: {Key}", key);
            var newValue = await factory();
            await SetAsync(key, newValue, expiration);

            return newValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache'den değer alınırken veya oluşturulurken hata oluştu: {Key}", key);
            return await factory();
        }
    }
}
