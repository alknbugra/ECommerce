using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Cart.Queries.GetCart;

/// <summary>
/// Sepet getirme sorgusu
/// </summary>
public class GetCartQuery : IQuery<CartDto>
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
