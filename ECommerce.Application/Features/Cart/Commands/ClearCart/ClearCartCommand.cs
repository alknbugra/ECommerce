using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Cart.Commands.ClearCart;

/// <summary>
/// Sepeti temizleme komutu
/// </summary>
public class ClearCartCommand : ICommand<CartDto>
{
    /// <summary>
    /// Kullanıcı ID'si (opsiyonel - misafir kullanıcılar için)
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Session ID (misafir kullanıcılar için)
    /// </summary>
    public string? SessionId { get; set; }
}
