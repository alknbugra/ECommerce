using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Orders.Commands.UpdateOrderStatus;

/// <summary>
/// Sipariş durumu güncelleme komutu
/// </summary>
public class UpdateOrderStatusCommand : ICommand<OrderDto>
{
    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Yeni durum
    /// </summary>
    public string NewStatus { get; set; } = string.Empty;

    /// <summary>
    /// Durum değişiklik notu
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Durumu değiştiren kullanıcı ID'si
    /// </summary>
    public Guid ChangedByUserId { get; set; }

    /// <summary>
    /// Kargo takip numarası (Shipped durumu için)
    /// </summary>
    public string? TrackingNumber { get; set; }

    /// <summary>
    /// Kargo şirketi (Shipped durumu için)
    /// </summary>
    public string? ShippingCompany { get; set; }
}
