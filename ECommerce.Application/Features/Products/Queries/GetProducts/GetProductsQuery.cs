using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using System.Text.Json;

namespace ECommerce.Application.Features.Products.Queries.GetProducts;

/// <summary>
/// Ürünleri getirme sorgusu
/// </summary>
public class GetProductsQuery : ICachedQuery<List<ProductDto>>
{
    /// <summary>
    /// Kategori ID'si (filtreleme için)
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Arama terimi
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Sadece aktif ürünler
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Sadece stokta olan ürünler
    /// </summary>
    public bool? InStock { get; set; }

    /// <summary>
    /// Sayfa numarası
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Sıralama alanı
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sıralama yönü (asc/desc)
    /// </summary>
    public string? SortDirection { get; set; } = "asc";

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
            return $"products:{hash}";
        }
    }

    /// <summary>
    /// Cache süre sonu
    /// </summary>
    public TimeSpan Expiration => TimeSpan.FromMinutes(15);
}
