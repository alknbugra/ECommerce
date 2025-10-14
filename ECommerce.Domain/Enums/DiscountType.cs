namespace ECommerce.Domain.Enums;

/// <summary>
/// İndirim türü enum'u
/// </summary>
public enum DiscountType
{
    /// <summary>
    /// Yüzde indirimi
    /// </summary>
    Percentage = 0,

    /// <summary>
    /// Sabit tutar indirimi
    /// </summary>
    FixedAmount = 1,

    /// <summary>
    /// Ücretsiz kargo
    /// </summary>
    FreeShipping = 2,

    /// <summary>
    /// Ücretsiz ürün
    /// </summary>
    FreeProduct = 3,

    /// <summary>
    /// İkinci ürün yarı fiyat
    /// </summary>
    BuyOneGetOneHalfPrice = 4,

    /// <summary>
    /// İkinci ürün ücretsiz
    /// </summary>
    BuyOneGetOneFree = 5
}
