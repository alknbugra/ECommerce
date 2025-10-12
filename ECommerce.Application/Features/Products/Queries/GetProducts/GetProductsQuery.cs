using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Products.Queries.GetProducts;

/// <summary>
/// Ürünleri getirme sorgusu
/// </summary>
public class GetProductsQuery : IQuery<List<ProductDto>>
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
}
