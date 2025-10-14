namespace ECommerce.Domain.Enums;

/// <summary>
/// Değerlendirme türü enum'u
/// </summary>
public enum ReviewType
{
    /// <summary>
    /// Doğrulanmış değerlendirme (satın alan kullanıcı)
    /// </summary>
    Verified = 0,

    /// <summary>
    /// Doğrulanmamış değerlendirme
    /// </summary>
    Unverified = 1,

    /// <summary>
    /// Misafir değerlendirmesi
    /// </summary>
    Guest = 2
}

/// <summary>
/// Değerlendirme durumu enum'u
/// </summary>
public enum ReviewStatus
{
    /// <summary>
    /// Beklemede
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Onaylandı
    /// </summary>
    Approved = 1,

    /// <summary>
    /// Reddedildi
    /// </summary>
    Rejected = 2
}

/// <summary>
/// Değerlendirme oy türü enum'u
/// </summary>
public enum ReviewVoteType
{
    /// <summary>
    /// Yararlı
    /// </summary>
    Helpful = 0,

    /// <summary>
    /// Yararsız
    /// </summary>
    NotHelpful = 1
}

/// <summary>
/// Yanıt türü enum'u
/// </summary>
public enum ResponseType
{
    /// <summary>
    /// Satıcı yanıtı
    /// </summary>
    Seller = 0,

    /// <summary>
    /// Admin yanıtı
    /// </summary>
    Admin = 1,

    /// <summary>
    /// Müşteri yanıtı
    /// </summary>
    Customer = 2
}
