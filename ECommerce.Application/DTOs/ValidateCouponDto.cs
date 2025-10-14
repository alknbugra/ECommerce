namespace ECommerce.Application.DTOs;

/// <summary>
/// Kupon doğrulama DTO'su
/// </summary>
public class ValidateCouponDto
{
    /// <summary>
    /// Kupon kodu
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Sipariş tutarı
    /// </summary>
    public decimal OrderAmount { get; set; }

    /// <summary>
    /// Sipariş ürünleri
    /// </summary>
    public List<OrderItemDto> OrderItems { get; set; } = new();
}
