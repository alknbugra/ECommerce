using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Favori ürün stok geçmişi entity'si
/// </summary>
public class WishlistItemStockHistory : BaseEntity
{
    /// <summary>
    /// Favori ürün ID'si
    /// </summary>
    public Guid WishlistItemId { get; set; }

    /// <summary>
    /// Eski stok durumu
    /// </summary>
    public bool OldStockStatus { get; set; }

    /// <summary>
    /// Yeni stok durumu
    /// </summary>
    public bool NewStockStatus { get; set; }

    /// <summary>
    /// Eski stok miktarı
    /// </summary>
    public int OldStockQuantity { get; set; }

    /// <summary>
    /// Yeni stok miktarı
    /// </summary>
    public int NewStockQuantity { get; set; }

    /// <summary>
    /// Stok değişim türü
    /// </summary>
    [MaxLength(20)]
    public string ChangeType { get; set; } = string.Empty; // InStock, OutOfStock, Restocked, QuantityChanged

    /// <summary>
    /// Değişim nedeni
    /// </summary>
    [MaxLength(100)]
    public string? ChangeReason { get; set; }

    /// <summary>
    /// Favori ürün
    /// </summary>
    public virtual WishlistItem WishlistItem { get; set; } = null!;

    /// <summary>
    /// Stok durumu değişti mi?
    /// </summary>
    [NotMapped]
    public bool HasStockStatusChanged => OldStockStatus != NewStockStatus;

    /// <summary>
    /// Stok miktarı değişti mi?
    /// </summary>
    [NotMapped]
    public bool HasQuantityChanged => OldStockQuantity != NewStockQuantity;

    /// <summary>
    /// Stok arttı mı?
    /// </summary>
    [NotMapped]
    public bool IsStockIncrease => NewStockQuantity > OldStockQuantity;

    /// <summary>
    /// Stok azaldı mı?
    /// </summary>
    [NotMapped]
    public bool IsStockDecrease => NewStockQuantity < OldStockQuantity;
}
