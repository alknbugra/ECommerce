namespace ECommerce.Application.DTOs;

/// <summary>
/// Favori ürün stok geçmişi DTO'su
/// </summary>
public class WishlistItemStockHistoryDto
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
    public string ChangeType { get; set; } = string.Empty;

    /// <summary>
    /// Değişim nedeni
    /// </summary>
    public string? ChangeReason { get; set; }

    /// <summary>
    /// Stok durumu değişti mi?
    /// </summary>
    public bool HasStockStatusChanged { get; set; }

    /// <summary>
    /// Stok miktarı değişti mi?
    /// </summary>
    public bool HasQuantityChanged { get; set; }

    /// <summary>
    /// Stok arttı mı?
    /// </summary>
    public bool IsStockIncrease { get; set; }

    /// <summary>
    /// Stok azaldı mı?
    /// </summary>
    public bool IsStockDecrease { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
