namespace ECommerce.Domain.Enums;

/// <summary>
/// Kargo türleri
/// </summary>
public enum CargoType
{
    /// <summary>
    /// Standart kargo
    /// </summary>
    Standard = 0,

    /// <summary>
    /// Hızlı kargo
    /// </summary>
    Express = 1,

    /// <summary>
    /// Aynı gün teslimat
    /// </summary>
    SameDay = 2,

    /// <summary>
    /// Özel kargo (kırılabilir, değerli eşya)
    /// </summary>
    Special = 3,

    /// <summary>
    /// Soğuk zincir kargo
    /// </summary>
    ColdChain = 4,

    /// <summary>
    /// Tehlikeli madde kargo
    /// </summary>
    Hazardous = 5,

    /// <summary>
    /// Büyük eşya kargo
    /// </summary>
    Oversized = 6
}
