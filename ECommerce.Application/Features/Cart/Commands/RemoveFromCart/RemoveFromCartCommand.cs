using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Cart.Commands.RemoveFromCart;

/// <summary>
/// Sepetten ürün çıkarma komutu
/// </summary>
public class RemoveFromCartCommand : ICommand<CartDto>
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
}
