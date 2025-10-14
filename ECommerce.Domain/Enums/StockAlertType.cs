namespace ECommerce.Domain.Enums;

/// <summary>
/// Stok uyarı türü enum'u
/// </summary>
public enum StockAlertType
{
    /// <summary>
    /// Düşük stok uyarısı
    /// </summary>
    LowStock = 0,

    /// <summary>
    /// Stok tükendi uyarısı
    /// </summary>
    OutOfStock = 1,

    /// <summary>
    /// Yüksek stok uyarısı
    /// </summary>
    HighStock = 2,

    /// <summary>
    /// Stok hareket uyarısı
    /// </summary>
    StockMovement = 3,

    /// <summary>
    /// Stok sayım uyarısı
    /// </summary>
    StockCount = 4,

    /// <summary>
    /// Stok transfer uyarısı
    /// </summary>
    StockTransfer = 5,

    /// <summary>
    /// Stok düzeltme uyarısı
    /// </summary>
    StockAdjustment = 6,

    /// <summary>
    /// Stok rezervasyon uyarısı
    /// </summary>
    StockReservation = 7,

    /// <summary>
    /// Stok iptal uyarısı
    /// </summary>
    StockCancellation = 8
}
