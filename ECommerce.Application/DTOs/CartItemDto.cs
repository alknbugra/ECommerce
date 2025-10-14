namespace ECommerce.Application.DTOs;

/// <summary>
/// Sepet ürünü DTO'su
/// </summary>
public class CartItemDto
{
    /// <summary>
    /// Sepet ürünü ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sepet ID'si
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Ürün adı
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Ürün SKU'su
    /// </summary>
    public string ProductSku { get; set; } = string.Empty;

    /// <summary>
    /// Ürün resmi URL'si
    /// </summary>
    public string? ProductImageUrl { get; set; }

    /// <summary>
    /// Miktar
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Birim fiyat
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Toplam fiyat
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Sepete eklenme tarihi
    /// </summary>
    public DateTime AddedAt { get; set; }

    /// <summary>
    /// Son güncelleme tarihi
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Ürün stokta var mı?
    /// </summary>
    public bool IsInStock { get; set; }

    /// <summary>
    /// Mevcut stok miktarı
    /// </summary>
    public int AvailableStock { get; set; }
}
