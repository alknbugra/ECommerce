using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Ürün entity'si
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Ürün adı
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Ürün açıklaması
    /// </summary>
    [MaxLength(2000)]
    public string? Description { get; set; }

    /// <summary>
    /// Kısa açıklama
    /// </summary>
    [MaxLength(500)]
    public string? ShortDescription { get; set; }

    /// <summary>
    /// Ürün kodu/SKU
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Fiyat
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// İndirimli fiyat
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountedPrice { get; set; }

    /// <summary>
    /// Stok miktarı
    /// </summary>
    public int StockQuantity { get; set; } = 0;

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
    [MaxLength(500)]
    public string? MainImageUrl { get; set; }

    /// <summary>
    /// Kategori ID'si
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Kategori
    /// </summary>
    public virtual Category Category { get; set; } = null!;

    /// <summary>
    /// Marka ID'si
    /// </summary>
    public Guid? BrandId { get; set; }

    /// <summary>
    /// Marka
    /// </summary>
    public virtual ProductBrand? Brand { get; set; }

    /// <summary>
    /// Ürün resimleri
    /// </summary>
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    /// <summary>
    /// Sipariş detayları
    /// </summary>
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    /// <summary>
    /// Ürün varyantları
    /// </summary>
    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

    /// <summary>
    /// Ürün özellikleri
    /// </summary>
    public virtual ICollection<ProductProductAttribute> Attributes { get; set; } = new List<ProductProductAttribute>();

    /// <summary>
    /// Ürün değerlendirmeleri
    /// </summary>
    public virtual ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();

    /// <summary>
    /// Ürün aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Stokta var mı?
    /// </summary>
    public bool InStock => StockQuantity > 0;

    /// <summary>
    /// İndirim yüzdesi
    /// </summary>
    public decimal? DiscountPercentage => DiscountedPrice.HasValue && DiscountedPrice < Price 
        ? Math.Round(((Price - DiscountedPrice.Value) / Price) * 100, 2) 
        : null;
}
