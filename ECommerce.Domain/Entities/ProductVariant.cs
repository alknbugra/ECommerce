using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Ürün varyantı entity'si (renk, beden, model vb.)
/// </summary>
public class ProductVariant : BaseEntity
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Ürün
    /// </summary>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Varyant adı (örn: "Kırmızı - L Beden")
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Varyant kodu/SKU
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Fiyat (varsayılan ürün fiyatından farklıysa)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Price { get; set; }

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
    /// Varyant resmi URL'si
    /// </summary>
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Varyant aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Varyant özellikleri
    /// </summary>
    public virtual ICollection<ProductVariantAttribute> Attributes { get; set; } = new List<ProductVariantAttribute>();

    /// <summary>
    /// Stokta var mı?
    /// </summary>
    public bool InStock => StockQuantity > 0;

    /// <summary>
    /// Gerçek fiyat (varyant fiyatı varsa onu, yoksa ürün fiyatını döner)
    /// </summary>
    public decimal ActualPrice => Price ?? Product.Price;

    /// <summary>
    /// Gerçek indirimli fiyat
    /// </summary>
    public decimal? ActualDiscountedPrice => DiscountedPrice ?? Product.DiscountedPrice;
}
