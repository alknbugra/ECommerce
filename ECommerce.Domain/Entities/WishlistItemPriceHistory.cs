using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Favori ürün fiyat geçmişi entity'si
/// </summary>
public class WishlistItemPriceHistory : BaseEntity
{
    /// <summary>
    /// Favori ürün ID'si
    /// </summary>
    public Guid WishlistItemId { get; set; }

    /// <summary>
    /// Eski fiyat
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal OldPrice { get; set; }

    /// <summary>
    /// Yeni fiyat
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal NewPrice { get; set; }

    /// <summary>
    /// Fiyat değişim yüzdesi
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal PriceChangePercentage { get; set; }

    /// <summary>
    /// Fiyat değişim türü
    /// </summary>
    [MaxLength(10)]
    public string ChangeType { get; set; } = string.Empty; // Increase, Decrease, NoChange

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
    /// Fiyat arttı mı?
    /// </summary>
    [NotMapped]
    public bool IsPriceIncrease => ChangeType == "Increase";

    /// <summary>
    /// Fiyat azaldı mı?
    /// </summary>
    [NotMapped]
    public bool IsPriceDecrease => ChangeType == "Decrease";

    /// <summary>
    /// Fiyat değişim miktarı
    /// </summary>
    [NotMapped]
    public decimal PriceChangeAmount => NewPrice - OldPrice;
}
