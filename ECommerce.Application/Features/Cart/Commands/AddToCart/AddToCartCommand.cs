using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Cart.Commands.AddToCart;

/// <summary>
/// Sepete ürün ekleme komutu
/// </summary>
public class AddToCartCommand : ICommand<CartDto>
{
    /// <summary>
    /// Kullanıcı ID'si (opsiyonel - misafir kullanıcılar için)
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Session ID (misafir kullanıcılar için)
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Miktar
    /// </summary>
    public int Quantity { get; set; } = 1;
}
