namespace ECommerce.Application.DTOs;

/// <summary>
/// Sipariş detayı DTO'su
/// </summary>
public class OrderItemDto
{
    /// <summary>
    /// Sipariş detayı ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

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
    /// İndirim tutarı
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Ürün resmi URL'si
    /// </summary>
    public string? ProductImageUrl { get; set; }

    /// <summary>
    /// Ürün kategorileri
    /// </summary>
    public List<Guid>? ProductCategories { get; set; }
}
