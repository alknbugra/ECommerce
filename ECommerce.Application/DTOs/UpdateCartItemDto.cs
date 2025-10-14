namespace ECommerce.Application.DTOs;

/// <summary>
/// Sepet ürünü güncelleme DTO'su
/// </summary>
public class UpdateCartItemDto
{
    /// <summary>
    /// Sepet ürünü ID'si
    /// </summary>
    public Guid CartItemId { get; set; }

    /// <summary>
    /// Yeni miktar
    /// </summary>
    public int Quantity { get; set; }
}
