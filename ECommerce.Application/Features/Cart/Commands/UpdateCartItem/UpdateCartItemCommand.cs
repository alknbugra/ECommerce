using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Cart.Commands.UpdateCartItem;

/// <summary>
/// Sepet ürünü güncelleme komutu
/// </summary>
public class UpdateCartItemCommand : ICommand<CartDto>
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
    /// Sepet ürünü ID'si
    /// </summary>
    public Guid CartItemId { get; set; }

    /// <summary>
    /// Yeni miktar
    /// </summary>
    public int Quantity { get; set; }
}
