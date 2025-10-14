using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using System.Text.Json;

namespace ECommerce.Application.Features.Products.Queries.AdvancedSearch;

/// <summary>
/// Gelişmiş arama sorgusu
/// </summary>
public class AdvancedSearchQuery : ICachedQuery<SearchResultDto>
{
    /// <summary>
    /// Arama terimi
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Kategori ID'leri
    /// </summary>
    public List<Guid>? CategoryIds { get; set; }

    /// <summary>
    /// Marka ID'leri
    /// </summary>
    public List<Guid>? BrandIds { get; set; }

    /// <summary>
    /// Minimum fiyat
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Maksimum fiyat
    /// </summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Sadece stokta olan ürünler
    /// </summary>
    public bool? InStock { get; set; }

    /// <summary>
    /// Sadece indirimli ürünler
    /// </summary>
    public bool? OnSale { get; set; }

    /// <summary>
    /// Minimum değerlendirme puanı
    /// </summary>
    public decimal? MinRating { get; set; }

    /// <summary>
    /// Ürün özellikleri filtresi
    /// </summary>
    public Dictionary<string, List<string>>? Attributes { get; set; }

    /// <summary>
    /// Sıralama türü
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sıralama yönü
    /// </summary>
    public string? SortDirection { get; set; }

    /// <summary>
    /// Sayfa numarası
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Kullanıcı ID'si (kişiselleştirme için)
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// IP adresi (analytics için)
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Cache anahtarı
    /// </summary>
    public string CacheKey
    {
        get
        {
            var queryJson = JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            var hash = queryJson.GetHashCode();
            return $"advanced_search:{hash}";
        }
    }

    /// <summary>
    /// Cache süre sonu
    /// </summary>
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}
