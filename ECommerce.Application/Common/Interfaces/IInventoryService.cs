using ECommerce.Application.DTOs;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Stok servis interface'i
/// </summary>
public interface IInventoryService
{
    /// <summary>
    /// Ürün stok bilgilerini al
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Stok bilgileri</returns>
    Task<InventoryDto?> GetInventoryAsync(Guid productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tüm stok bilgilerini al
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Stok listesi</returns>
    Task<List<InventoryDto>> GetAllInventoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Düşük stoklu ürünleri al
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Düşük stoklu ürünler</returns>
    Task<List<InventoryDto>> GetLowStockInventoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stok hareketlerini al
    /// </summary>
    /// <param name="productId">Ürün ID'si (opsiyonel)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Stok hareketleri</returns>
    Task<List<InventoryMovementDto>> GetInventoryMovementsAsync(Guid? productId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stok uyarılarını al
    /// </summary>
    /// <param name="isRead">Okunmuş/okunmamış filtresi (opsiyonel)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Stok uyarıları</returns>
    Task<List<StockAlertDto>> GetStockAlertsAsync(bool? isRead = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stok girişi yap
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="quantity">Miktar</param>
    /// <param name="reason">Neden</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İşlem sonucu</returns>
    Task<bool> StockInAsync(Guid productId, decimal quantity, string reason, Guid? userId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stok çıkışı yap
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="quantity">Miktar</param>
    /// <param name="reason">Neden</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İşlem sonucu</returns>
    Task<bool> StockOutAsync(Guid productId, decimal quantity, string reason, Guid? userId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stok rezervasyonu yap
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="quantity">Miktar</param>
    /// <param name="relatedEntityId">İlişkili entity ID'si</param>
    /// <param name="relatedEntityType">İlişkili entity türü</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İşlem sonucu</returns>
    Task<bool> ReserveStockAsync(Guid productId, decimal quantity, Guid relatedEntityId, string relatedEntityType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stok rezervasyonunu kaldır
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="quantity">Miktar</param>
    /// <param name="relatedEntityId">İlişkili entity ID'si</param>
    /// <param name="relatedEntityType">İlişkili entity türü</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İşlem sonucu</returns>
    Task<bool> ReleaseReservedStockAsync(Guid productId, decimal quantity, Guid relatedEntityId, string relatedEntityType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sipariş için stok kontrolü yap
    /// </summary>
    /// <param name="orderItems">Sipariş ürünleri</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Stok yeterli mi?</returns>
    Task<bool> CheckStockAvailabilityAsync(List<OrderItemDto> orderItems, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sipariş için stok düşür
    /// </summary>
    /// <param name="orderId">Sipariş ID'si</param>
    /// <param name="orderItems">Sipariş ürünleri</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İşlem sonucu</returns>
    Task<bool> ProcessOrderStockAsync(Guid orderId, List<OrderItemDto> orderItems, Guid? userId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stok uyarılarını kontrol et
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Oluşturulan uyarı sayısı</returns>
    Task<int> CheckStockAlertsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stok uyarısını okundu olarak işaretle
    /// </summary>
    /// <param name="alertId">Uyarı ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İşlem sonucu</returns>
    Task<bool> MarkAlertAsReadAsync(Guid alertId, Guid userId, CancellationToken cancellationToken = default);
}
