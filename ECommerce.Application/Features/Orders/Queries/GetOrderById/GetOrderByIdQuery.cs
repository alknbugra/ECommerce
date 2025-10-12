using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Orders.Queries.GetOrderById;

/// <summary>
/// ID'ye göre sipariş getirme sorgusu
/// </summary>
public class GetOrderByIdQuery : IQuery<OrderDto?>
{
    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kullanıcı ID'si (güvenlik için)
    /// </summary>
    public Guid? UserId { get; set; }
}
