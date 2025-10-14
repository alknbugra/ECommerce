namespace ECommerce.Application.DTOs;

/// <summary>
/// Kupon doğrulama sonucu DTO'su
/// </summary>
public class CouponValidationResultDto
{
    /// <summary>
    /// Kupon geçerli mi?
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Kupon bilgileri
    /// </summary>
    public CouponDto? Coupon { get; set; }

    /// <summary>
    /// İndirim tutarı
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Hata mesajı
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Hata kodu
    /// </summary>
    public string? ErrorCode { get; set; }
}
