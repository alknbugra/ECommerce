namespace ECommerce.Application.DTOs;

/// <summary>
/// Sipariş DTO'su
/// </summary>
public class OrderDto
{
    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sipariş numarası
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Kullanıcı adı
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Sipariş durumu
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme durumu
    /// </summary>
    public string PaymentStatus { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme yöntemi
    /// </summary>
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// Alt toplam
    /// </summary>
    public decimal SubTotal { get; set; }

    /// <summary>
    /// Kargo ücreti
    /// </summary>
    public decimal ShippingCost { get; set; }

    /// <summary>
    /// Vergi tutarı
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// İndirim tutarı
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Toplam tutar
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Kargo takip numarası
    /// </summary>
    public string? TrackingNumber { get; set; }

    /// <summary>
    /// Kargo şirketi
    /// </summary>
    public string? ShippingCompany { get; set; }

    /// <summary>
    /// Sipariş notları
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Sipariş tarihi
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Gönderim tarihi
    /// </summary>
    public DateTime? ShippedDate { get; set; }

    /// <summary>
    /// Teslim tarihi
    /// </summary>
    public DateTime? DeliveredDate { get; set; }

    /// <summary>
    /// Teslimat adresi
    /// </summary>
    public AddressDto? ShippingAddress { get; set; }

    /// <summary>
    /// Fatura adresi
    /// </summary>
    public AddressDto? BillingAddress { get; set; }

    /// <summary>
    /// Sipariş detayları
    /// </summary>
    public List<OrderItemDto> OrderItems { get; set; } = new();

    /// <summary>
    /// Sipariş durumu geçmişi
    /// </summary>
    public List<OrderStatusHistoryDto> StatusHistory { get; set; } = new();
}
