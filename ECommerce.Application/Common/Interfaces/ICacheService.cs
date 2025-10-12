using Microsoft.Extensions.Caching.Memory;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Cache servisi interface'i
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Cache'den değer al
    /// </summary>
    /// <typeparam name="T">Değer tipi</typeparam>
    /// <param name="key">Cache anahtarı</param>
    /// <returns>Cache'deki değer veya null</returns>
    T? Get<T>(string key) where T : class;

    /// <summary>
    /// Cache'den değer al (async)
    /// </summary>
    /// <typeparam name="T">Değer tipi</typeparam>
    /// <param name="key">Cache anahtarı</param>
    /// <returns>Cache'deki değer veya null</returns>
    Task<T?> GetAsync<T>(string key) where T : class;

    /// <summary>
    /// Cache'e değer ekle
    /// </summary>
    /// <typeparam name="T">Değer tipi</typeparam>
    /// <param name="key">Cache anahtarı</param>
    /// <param name="value">Eklenecek değer</param>
    /// <param name="expiration">Süre sonu (opsiyonel)</param>
    void Set<T>(string key, T value, TimeSpan? expiration = null) where T : class;

    /// <summary>
    /// Cache'e değer ekle (async)
    /// </summary>
    /// <typeparam name="T">Değer tipi</typeparam>
    /// <param name="key">Cache anahtarı</param>
    /// <param name="value">Eklenecek değer</param>
    /// <param name="expiration">Süre sonu (opsiyonel)</param>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class;

    /// <summary>
    /// Cache'den değer sil
    /// </summary>
    /// <param name="key">Cache anahtarı</param>
    void Remove(string key);

    /// <summary>
    /// Cache'den değer sil (async)
    /// </summary>
    /// <param name="key">Cache anahtarı</param>
    Task RemoveAsync(string key);

    /// <summary>
    /// Cache'den pattern'e uyan tüm değerleri sil
    /// </summary>
    /// <param name="pattern">Silinecek anahtar pattern'i</param>
    void RemoveByPattern(string pattern);

    /// <summary>
    /// Cache'den pattern'e uyan tüm değerleri sil (async)
    /// </summary>
    /// <param name="pattern">Silinecek anahtar pattern'i</param>
    Task RemoveByPatternAsync(string pattern);

    /// <summary>
    /// Cache'de değer var mı kontrol et
    /// </summary>
    /// <param name="key">Cache anahtarı</param>
    /// <returns>Var mı?</returns>
    bool Exists(string key);

    /// <summary>
    /// Cache'de değer var mı kontrol et (async)
    /// </summary>
    /// <param name="key">Cache anahtarı</param>
    /// <returns>Var mı?</returns>
    Task<bool> ExistsAsync(string key);

    /// <summary>
    /// Cache'i temizle
    /// </summary>
    void Clear();

    /// <summary>
    /// Cache'i temizle (async)
    /// </summary>
    Task ClearAsync();

    /// <summary>
    /// Cache'den değer al veya oluştur
    /// </summary>
    /// <typeparam name="T">Değer tipi</typeparam>
    /// <param name="key">Cache anahtarı</param>
    /// <param name="factory">Değer oluşturma fonksiyonu</param>
    /// <param name="expiration">Süre sonu (opsiyonel)</param>
    /// <returns>Cache'deki veya yeni oluşturulan değer</returns>
    T GetOrCreate<T>(string key, Func<T> factory, TimeSpan? expiration = null) where T : class;

    /// <summary>
    /// Cache'den değer al veya oluştur (async)
    /// </summary>
    /// <typeparam name="T">Değer tipi</typeparam>
    /// <param name="key">Cache anahtarı</param>
    /// <param name="factory">Değer oluşturma fonksiyonu</param>
    /// <param name="expiration">Süre sonu (opsiyonel)</param>
    /// <returns>Cache'deki veya yeni oluşturulan değer</returns>
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null) where T : class;
}
