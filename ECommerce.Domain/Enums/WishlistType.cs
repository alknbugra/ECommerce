namespace ECommerce.Domain.Enums;

/// <summary>
/// Favori liste türü enum'u
/// </summary>
public enum WishlistType
{
    /// <summary>
    /// Kişisel liste
    /// </summary>
    Personal = 0,

    /// <summary>
    /// Paylaşılan liste
    /// </summary>
    Shared = 1,

    /// <summary>
    /// Genel liste
    /// </summary>
    Public = 2,

    /// <summary>
    /// Hediyelik liste
    /// </summary>
    Gift = 3,

    /// <summary>
    /// Alışveriş listesi
    /// </summary>
    Shopping = 4
}

/// <summary>
/// Favori liste paylaşım türü enum'u
/// </summary>
public enum WishlistShareType
{
    /// <summary>
    /// Genel paylaşım
    /// </summary>
    Public = 0,

    /// <summary>
    /// Özel paylaşım
    /// </summary>
    Private = 1,

    /// <summary>
    /// E-posta paylaşımı
    /// </summary>
    Email = 2,

    /// <summary>
    /// Sosyal medya paylaşımı
    /// </summary>
    Social = 3
}

/// <summary>
/// Favori ürün öncelik seviyesi enum'u
/// </summary>
public enum WishlistItemPriority
{
    /// <summary>
    /// Normal öncelik
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Yüksek öncelik
    /// </summary>
    High = 1,

    /// <summary>
    /// Çok yüksek öncelik
    /// </summary>
    VeryHigh = 2,

    /// <summary>
    /// Acil
    /// </summary>
    Urgent = 3
}

/// <summary>
/// Fiyat değişim türü enum'u
/// </summary>
public enum PriceChangeType
{
    /// <summary>
    /// Fiyat arttı
    /// </summary>
    Increase = 0,

    /// <summary>
    /// Fiyat azaldı
    /// </summary>
    Decrease = 1,

    /// <summary>
    /// Fiyat değişmedi
    /// </summary>
    NoChange = 2
}

/// <summary>
/// Stok değişim türü enum'u
/// </summary>
public enum StockChangeType
{
    /// <summary>
    /// Stokta
    /// </summary>
    InStock = 0,

    /// <summary>
    /// Stokta yok
    /// </summary>
    OutOfStock = 1,

    /// <summary>
    /// Stok yenilendi
    /// </summary>
    Restocked = 2,

    /// <summary>
    /// Stok miktarı değişti
    /// </summary>
    QuantityChanged = 3
}
