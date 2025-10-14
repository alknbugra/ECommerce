namespace ECommerce.Domain.Enums;

/// <summary>
/// Stok hareket türü enum'u
/// </summary>
public enum InventoryMovementType
{
    /// <summary>
    /// Stok girişi
    /// </summary>
    StockIn = 0,

    /// <summary>
    /// Stok çıkışı
    /// </summary>
    StockOut = 1,

    /// <summary>
    /// Stok transferi
    /// </summary>
    StockTransfer = 2,

    /// <summary>
    /// Stok düzeltmesi
    /// </summary>
    StockAdjustment = 3,

    /// <summary>
    /// Sipariş rezervasyonu
    /// </summary>
    OrderReservation = 4,

    /// <summary>
    /// Sipariş iptali (rezervasyon kaldırma)
    /// </summary>
    OrderCancellation = 5,

    /// <summary>
    /// Sipariş onayı (stok düşürme)
    /// </summary>
    OrderConfirmation = 6,

    /// <summary>
    /// İade (stok geri ekleme)
    /// </summary>
    Return = 7,

    /// <summary>
    /// Stok sayımı
    /// </summary>
    StockCount = 8,

    /// <summary>
    /// Stok kaybı
    /// </summary>
    StockLoss = 9
}
