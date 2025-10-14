namespace ECommerce.Application.DTOs;

/// <summary>
/// Sepete ürün ekleme DTO'su
/// </summary>
public class AddToCartDto
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Miktar
    /// </summary>
    public int Quantity { get; set; } = 1;
}
