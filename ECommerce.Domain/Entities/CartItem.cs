using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Sepet ürünü entity'si
/// </summary>
public class CartItem : BaseEntity
{
    /// <summary>
    /// Sepet ID'si
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Miktar
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Birim fiyat (sepete eklendiği andaki fiyat)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Toplam fiyat
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Sepete eklenme tarihi
    /// </summary>
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Son güncelleme tarihi
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Sepet
    /// </summary>
    public virtual Cart Cart { get; set; } = null!;

    /// <summary>
    /// Ürün
    /// </summary>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Toplam fiyatı hesapla
    /// </summary>
    public void CalculateTotalPrice()
    {
        TotalPrice = UnitPrice * Quantity;
    }
}
