namespace ECommerce.Application.DTOs;

/// <summary>
/// Kupon kullanımı DTO'su
/// </summary>
public class CouponUsageDto
{
    /// <summary>
    /// Kullanım ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kupon ID'si
    /// </summary>
    public Guid CouponId { get; set; }

    /// <summary>
    /// Kupon kodu
    /// </summary>
    public string CouponCode { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Kullanıcı adı
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Sipariş numarası
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// İndirim tutarı
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Sipariş tutarı (indirim öncesi)
    /// </summary>
    public decimal OrderAmount { get; set; }

    /// <summary>
    /// Kullanım tarihi
    /// </summary>
    public DateTime UsedAt { get; set; }

    /// <summary>
    /// Kullanıcı IP adresi
    /// </summary>
    public string? UserIpAddress { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
