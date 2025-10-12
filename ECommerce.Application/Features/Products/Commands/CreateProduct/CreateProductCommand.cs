using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Ürün oluşturma komutu
/// </summary>
public class CreateProductCommand : ICommand<ProductDto>
{
    /// <summary>
    /// Ürün adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Ürün açıklaması
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Kısa açıklama
    /// </summary>
    public string? ShortDescription { get; set; }

    /// <summary>
    /// Ürün kodu/SKU
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Fiyat
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// İndirimli fiyat
    /// </summary>
    public decimal? DiscountedPrice { get; set; }

    /// <summary>
    /// Stok miktarı
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Minimum stok uyarısı
    /// </summary>
    public int MinStockLevel { get; set; } = 5;

    /// <summary>
    /// Ürün ağırlığı (gram)
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// Ürün boyutları (cm)
    /// </summary>
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }

    /// <summary>
    /// Ana resim URL'si
    /// </summary>
    public string? MainImageUrl { get; set; }

    /// <summary>
    /// Kategori ID'si
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Ürün aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;
}
