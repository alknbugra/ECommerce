namespace ECommerce.Application.DTOs;

/// <summary>
/// Ürün DTO'su
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid Id { get; set; }

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
    /// Minimum stok seviyesi
    /// </summary>
    public int MinStockLevel { get; set; }

    /// <summary>
    /// Ağırlık
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// Uzunluk
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// Genişlik
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// Yükseklik
    /// </summary>
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
    /// Kategori adı
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>
    /// Marka ID'si
    /// </summary>
    public Guid? BrandId { get; set; }

    /// <summary>
    /// Marka adı
    /// </summary>
    public string? BrandName { get; set; }

    /// <summary>
    /// Ürün aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Stokta var mı?
    /// </summary>
    public bool InStock { get; set; }

    /// <summary>
    /// İndirim yüzdesi
    /// </summary>
    public decimal? DiscountPercentage { get; set; }

    /// <summary>
    /// Ürün resimleri
    /// </summary>
    public List<ProductImageDto> Images { get; set; } = new();

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Güncellenme tarihi
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
