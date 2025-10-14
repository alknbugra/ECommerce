namespace ECommerce.Application.DTOs;

/// <summary>
/// Ödeme oluşturma DTO'su
/// </summary>
public class CreatePaymentDto
{
    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Ödeme yöntemi
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Kredi kartı numarası (maskelenmiş)
    /// </summary>
    public string? CardNumber { get; set; }

    /// <summary>
    /// Kart sahibi adı
    /// </summary>
    public string? CardHolderName { get; set; }

    /// <summary>
    /// Son kullanma ayı
    /// </summary>
    public int? ExpiryMonth { get; set; }

    /// <summary>
    /// Son kullanma yılı
    /// </summary>
    public int? ExpiryYear { get; set; }

    /// <summary>
    /// CVV kodu
    /// </summary>
    public string? Cvv { get; set; }

    /// <summary>
    /// Taksit sayısı
    /// </summary>
    public int? InstallmentCount { get; set; } = 1;

    /// <summary>
    /// 3D Secure kullanılsın mı?
    /// </summary>
    public bool Use3DSecure { get; set; } = true;

    /// <summary>
    /// Ödeme tutarı
    /// </summary>
    public decimal Amount { get; set; }
}
