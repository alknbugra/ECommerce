namespace ECommerce.Application.DTOs;

/// <summary>
/// Kupon oluşturma DTO'su
/// </summary>
public class CreateCouponDto
{
    /// <summary>
    /// Kupon kodu
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Kupon adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kupon açıklaması
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// İndirim türü
    /// </summary>
    public string DiscountType { get; set; } = string.Empty;

    /// <summary>
    /// İndirim değeri
    /// </summary>
    public decimal DiscountValue { get; set; }

    /// <summary>
    /// Minimum sipariş tutarı
    /// </summary>
    public decimal MinimumOrderAmount { get; set; }

    /// <summary>
    /// Maksimum indirim tutarı
    /// </summary>
    public decimal? MaximumDiscountAmount { get; set; }

    /// <summary>
    /// Kullanım limiti (toplam)
    /// </summary>
    public int? UsageLimit { get; set; }

    /// <summary>
    /// Kullanıcı başına kullanım limiti
    /// </summary>
    public int? UsageLimitPerUser { get; set; }

    /// <summary>
    /// Başlangıç tarihi
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Bitiş tarihi
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Sadece yeni kullanıcılar için mi?
    /// </summary>
    public bool IsForNewUsersOnly { get; set; }

    /// <summary>
    /// Sadece belirli kategoriler için mi?
    /// </summary>
    public bool IsForSpecificCategories { get; set; }

    /// <summary>
    /// Sadece belirli ürünler için mi?
    /// </summary>
    public bool IsForSpecificProducts { get; set; }

    /// <summary>
    /// Sadece belirli kullanıcılar için mi?
    /// </summary>
    public bool IsForSpecificUsers { get; set; }

    /// <summary>
    /// Kategori ID'leri
    /// </summary>
    public List<Guid>? CategoryIds { get; set; }

    /// <summary>
    /// Ürün ID'leri
    /// </summary>
    public List<Guid>? ProductIds { get; set; }

    /// <summary>
    /// Kullanıcı ID'leri
    /// </summary>
    public List<Guid>? UserIds { get; set; }
}
