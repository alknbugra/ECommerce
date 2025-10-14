namespace ECommerce.Domain.Enums;

/// <summary>
/// Sıralama türleri
/// </summary>
public enum SortType
{
    /// <summary>
    /// Varsayılan sıralama
    /// </summary>
    Default = 0,

    /// <summary>
    /// İsme göre sıralama
    /// </summary>
    Name = 1,

    /// <summary>
    /// Fiyata göre sıralama
    /// </summary>
    Price = 2,

    /// <summary>
    /// Popülerliğe göre sıralama
    /// </summary>
    Popularity = 3,

    /// <summary>
    /// Değerlendirmeye göre sıralama
    /// </summary>
    Rating = 4,

    /// <summary>
    /// Yeni ürünlere göre sıralama
    /// </summary>
    Newest = 5,

    /// <summary>
    /// İndirim oranına göre sıralama
    /// </summary>
    Discount = 6,

    /// <summary>
    /// Satış sayısına göre sıralama
    /// </summary>
    Sales = 7
}
