using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder;

/// <summary>
/// Sipariş oluşturma komutu
/// </summary>
public class CreateOrderCommand : ICommand<OrderDto>
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Teslimat adresi ID'si
    /// </summary>
    public Guid ShippingAddressId { get; set; }

    /// <summary>
    /// Fatura adresi ID'si (opsiyonel)
    /// </summary>
    public Guid? BillingAddressId { get; set; }

    /// <summary>
    /// Ödeme yöntemi
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Sipariş notları
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Sipariş detayları
    /// </summary>
    public List<CreateOrderItemDto> OrderItems { get; set; } = new();

    /// <summary>
    /// Kargo ücreti
    /// </summary>
    public decimal ShippingCost { get; set; } = 0;

    /// <summary>
    /// Vergi tutarı
    /// </summary>
    public decimal TaxAmount { get; set; } = 0;

    /// <summary>
    /// İndirim tutarı
    /// </summary>
    public decimal DiscountAmount { get; set; } = 0;
}

/// <summary>
/// Sipariş detayı oluşturma DTO'su
/// </summary>
public class CreateOrderItemDto
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Miktar
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Birim fiyat (opsiyonel - ürün fiyatından alınır)
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// İndirim tutarı
    /// </summary>
    public decimal DiscountAmount { get; set; } = 0;
}
