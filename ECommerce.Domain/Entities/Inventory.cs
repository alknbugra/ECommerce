using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Stok entity'si
/// </summary>
public class Inventory : BaseEntity
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Mevcut stok miktarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// Rezerve edilmiş stok miktarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal ReservedStock { get; set; }

    /// <summary>
    /// Minimum stok seviyesi
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal MinimumStock { get; set; }

    /// <summary>
    /// Maksimum stok seviyesi
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal MaximumStock { get; set; }

    /// <summary>
    /// Stok uyarı seviyesi
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal AlertStock { get; set; }

    /// <summary>
    /// Stok aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Son stok güncelleme tarihi
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Ürün
    /// </summary>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Kullanılabilir stok (Mevcut - Rezerve)
    /// </summary>
    [NotMapped]
    public decimal AvailableStock => CurrentStock - ReservedStock;

    /// <summary>
    /// Stok düşük mü?
    /// </summary>
    [NotMapped]
    public bool IsLowStock => CurrentStock <= AlertStock;

    /// <summary>
    /// Stok tükendi mi?
    /// </summary>
    [NotMapped]
    public bool IsOutOfStock => CurrentStock <= 0;
}
