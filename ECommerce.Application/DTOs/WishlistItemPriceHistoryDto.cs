namespace ECommerce.Application.DTOs;

/// <summary>
/// Favori ürün fiyat geçmişi DTO'su
/// </summary>
public class WishlistItemPriceHistoryDto
{
    /// <summary>
    /// Geçmiş ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Favori ürün ID'si
    /// </summary>
    public Guid WishlistItemId { get; set; }

    /// <summary>
    /// Eski fiyat
    /// </summary>
    public decimal OldPrice { get; set; }

    /// <summary>
    /// Yeni fiyat
    /// </summary>
    public decimal NewPrice { get; set; }

    /// <summary>
    /// Fiyat değişim yüzdesi
    /// </summary>
    public decimal PriceChangePercentage { get; set; }

    /// <summary>
    /// Fiyat değişim türü
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;

    /// <summary>
    /// Değişim nedeni
    /// </summary>
    public string? ChangeReason { get; set; }

    /// <summary>
    /// Fiyat arttı mı?
    /// </summary>
    public bool IsPriceIncrease { get; set; }

    /// <summary>
    /// Fiyat azaldı mı?
    /// </summary>
    public bool IsPriceDecrease { get; set; }

    /// <summary>
    /// Fiyat değişim miktarı
    /// </summary>
    public decimal PriceChangeAmount { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
