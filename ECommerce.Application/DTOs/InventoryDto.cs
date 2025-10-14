namespace ECommerce.Application.DTOs;

/// <summary>
/// Stok DTO'su
/// </summary>
public class InventoryDto
{
    /// <summary>
    /// Stok ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Ürün adı
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Ürün kodu
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// Mevcut stok miktarı
    /// </summary>
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// Rezerve edilmiş stok miktarı
    /// </summary>
    public decimal ReservedStock { get; set; }

    /// <summary>
    /// Kullanılabilir stok miktarı
    /// </summary>
    public decimal AvailableStock { get; set; }

    /// <summary>
    /// Minimum stok seviyesi
    /// </summary>
    public decimal MinimumStock { get; set; }

    /// <summary>
    /// Maksimum stok seviyesi
    /// </summary>
    public decimal MaximumStock { get; set; }

    /// <summary>
    /// Stok uyarı seviyesi
    /// </summary>
    public decimal AlertStock { get; set; }

    /// <summary>
    /// Stok aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Stok düşük mü?
    /// </summary>
    public bool IsLowStock { get; set; }

    /// <summary>
    /// Stok tükendi mi?
    /// </summary>
    public bool IsOutOfStock { get; set; }

    /// <summary>
    /// Son stok güncelleme tarihi
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
