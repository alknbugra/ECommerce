using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Services.Inventory;

/// <summary>
/// Stok servis implementasyonu
/// </summary>
public class InventoryService : IInventoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(
        IUnitOfWork unitOfWork,
        ILogger<InventoryService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<InventoryDto?> GetInventoryAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        try
        {
            var inventory = await _unitOfWork.Inventories.FirstOrDefaultAsync(
                i => i.ProductId == productId && i.IsActive);

            if (inventory == null)
                return null;

            return new InventoryDto
            {
                Id = inventory.Id,
                ProductId = inventory.ProductId,
                ProductName = inventory.Product.Name,
                ProductCode = inventory.Product.Sku,
                CurrentStock = inventory.CurrentStock,
                ReservedStock = inventory.ReservedStock,
                AvailableStock = inventory.AvailableStock,
                MinimumStock = inventory.MinimumStock,
                MaximumStock = inventory.MaximumStock,
                AlertStock = inventory.AlertStock,
                IsActive = inventory.IsActive,
                IsLowStock = inventory.IsLowStock,
                IsOutOfStock = inventory.IsOutOfStock,
                LastUpdated = inventory.LastUpdated,
                CreatedAt = inventory.CreatedAt,
                UpdatedAt = inventory.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok bilgileri alınırken hata oluştu. ProductId: {ProductId}", productId);
            return null;
        }
    }

    public async Task<List<InventoryDto>> GetAllInventoriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var inventories = await _unitOfWork.Inventories.GetAll()
                .Where(i => i.IsActive)
                .Include(i => i.Product)
                .ToListAsync(cancellationToken);

            return inventories.Select(inventory => new InventoryDto
            {
                Id = inventory.Id,
                ProductId = inventory.ProductId,
                ProductName = inventory.Product.Name,
                ProductCode = inventory.Product.Sku,
                CurrentStock = inventory.CurrentStock,
                ReservedStock = inventory.ReservedStock,
                AvailableStock = inventory.AvailableStock,
                MinimumStock = inventory.MinimumStock,
                MaximumStock = inventory.MaximumStock,
                AlertStock = inventory.AlertStock,
                IsActive = inventory.IsActive,
                IsLowStock = inventory.IsLowStock,
                IsOutOfStock = inventory.IsOutOfStock,
                LastUpdated = inventory.LastUpdated,
                CreatedAt = inventory.CreatedAt,
                UpdatedAt = inventory.UpdatedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tüm stok bilgileri alınırken hata oluştu");
            return new List<InventoryDto>();
        }
    }

    public async Task<List<InventoryDto>> GetLowStockInventoriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var inventories = await _unitOfWork.Inventories.GetAll()
                .Where(i => i.IsActive && i.CurrentStock <= i.AlertStock)
                .Include(i => i.Product)
                .ToListAsync(cancellationToken);

            return inventories.Select(inventory => new InventoryDto
            {
                Id = inventory.Id,
                ProductId = inventory.ProductId,
                ProductName = inventory.Product.Name,
                ProductCode = inventory.Product.Sku,
                CurrentStock = inventory.CurrentStock,
                ReservedStock = inventory.ReservedStock,
                AvailableStock = inventory.AvailableStock,
                MinimumStock = inventory.MinimumStock,
                MaximumStock = inventory.MaximumStock,
                AlertStock = inventory.AlertStock,
                IsActive = inventory.IsActive,
                IsLowStock = inventory.IsLowStock,
                IsOutOfStock = inventory.IsOutOfStock,
                LastUpdated = inventory.LastUpdated,
                CreatedAt = inventory.CreatedAt,
                UpdatedAt = inventory.UpdatedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Düşük stoklu ürünler alınırken hata oluştu");
            return new List<InventoryDto>();
        }
    }

    public async Task<List<InventoryMovementDto>> GetInventoryMovementsAsync(Guid? productId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.InventoryMovements.GetAll()
                .Include(m => m.Product)
                .Include(m => m.User)
                .AsQueryable();

            if (productId.HasValue)
                query = query.Where(m => m.ProductId == productId.Value);

            var movements = await query
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync(cancellationToken);

            return movements.Select(movement => new InventoryMovementDto
            {
                Id = movement.Id,
                ProductId = movement.ProductId,
                ProductName = movement.Product.Name,
                ProductCode = movement.Product.Sku,
                MovementType = movement.MovementType,
                Quantity = movement.Quantity,
                PreviousStock = movement.PreviousStock,
                NewStock = movement.NewStock,
                Reason = movement.Reason,
                RelatedEntityId = movement.RelatedEntityId,
                RelatedEntityType = movement.RelatedEntityType,
                MovementDate = movement.MovementDate,
                UserId = movement.UserId,
                UserName = movement.User?.FirstName + " " + movement.User?.LastName,
                Notes = movement.Notes,
                CreatedAt = movement.CreatedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok hareketleri alınırken hata oluştu. ProductId: {ProductId}", productId);
            return new List<InventoryMovementDto>();
        }
    }

    public async Task<List<StockAlertDto>> GetStockAlertsAsync(bool? isRead = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.StockAlerts.GetAll()
                .Include(a => a.Product)
                .Include(a => a.ReadByUser)
                .AsQueryable();

            if (isRead.HasValue)
                query = query.Where(a => a.IsRead == isRead.Value);

            var alerts = await query
                .OrderByDescending(a => a.AlertDate)
                .ToListAsync(cancellationToken);

            return alerts.Select(alert => new StockAlertDto
            {
                Id = alert.Id,
                ProductId = alert.ProductId,
                ProductName = alert.Product.Name,
                ProductCode = alert.Product.Sku,
                AlertType = alert.AlertType,
                CurrentStock = alert.CurrentStock,
                AlertLevel = alert.AlertLevel,
                Message = alert.Message,
                Status = alert.Status,
                AlertDate = alert.AlertDate,
                IsRead = alert.IsRead,
                ReadDate = alert.ReadDate,
                ReadByUserId = alert.ReadByUserId,
                ReadByUserName = alert.ReadByUser != null ? 
                    alert.ReadByUser.FirstName + " " + alert.ReadByUser.LastName : null,
                CreatedAt = alert.CreatedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok uyarıları alınırken hata oluştu");
            return new List<StockAlertDto>();
        }
    }

    public async Task<bool> StockInAsync(Guid productId, decimal quantity, string reason, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Stok girişi yapılıyor. ProductId: {ProductId}, Quantity: {Quantity}", productId, quantity);

            var inventory = await GetOrCreateInventoryAsync(productId);
            if (inventory == null)
                return false;

            var previousStock = inventory.CurrentStock;
            inventory.CurrentStock += quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            inventory.UpdatedAt = DateTime.UtcNow;

            // Stok hareketi kaydet
            var movement = new InventoryMovement
            {
                ProductId = productId,
                MovementType = InventoryMovementType.StockIn.ToString(),
                Quantity = quantity,
                PreviousStock = previousStock,
                NewStock = inventory.CurrentStock,
                Reason = reason,
                MovementDate = DateTime.UtcNow,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.InventoryMovements.AddAsync(movement);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Stok girişi başarılı. ProductId: {ProductId}, NewStock: {NewStock}", 
                productId, inventory.CurrentStock);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok girişi sırasında hata oluştu. ProductId: {ProductId}", productId);
            return false;
        }
    }

    public async Task<bool> StockOutAsync(Guid productId, decimal quantity, string reason, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Stok çıkışı yapılıyor. ProductId: {ProductId}, Quantity: {Quantity}", productId, quantity);

            var inventory = await GetOrCreateInventoryAsync(productId);
            if (inventory == null)
                return false;

            if (inventory.CurrentStock < quantity)
            {
                _logger.LogWarning("Yetersiz stok. ProductId: {ProductId}, Required: {Required}, Available: {Available}", 
                    productId, quantity, inventory.CurrentStock);
                return false;
            }

            var previousStock = inventory.CurrentStock;
            inventory.CurrentStock -= quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            inventory.UpdatedAt = DateTime.UtcNow;

            // Stok hareketi kaydet
            var movement = new InventoryMovement
            {
                ProductId = productId,
                MovementType = InventoryMovementType.StockOut.ToString(),
                Quantity = quantity,
                PreviousStock = previousStock,
                NewStock = inventory.CurrentStock,
                Reason = reason,
                MovementDate = DateTime.UtcNow,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.InventoryMovements.AddAsync(movement);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Stok çıkışı başarılı. ProductId: {ProductId}, NewStock: {NewStock}", 
                productId, inventory.CurrentStock);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok çıkışı sırasında hata oluştu. ProductId: {ProductId}", productId);
            return false;
        }
    }

    public async Task<bool> ReserveStockAsync(Guid productId, decimal quantity, Guid relatedEntityId, string relatedEntityType, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Stok rezervasyonu yapılıyor. ProductId: {ProductId}, Quantity: {Quantity}", productId, quantity);

            var inventory = await GetOrCreateInventoryAsync(productId);
            if (inventory == null)
                return false;

            if (inventory.AvailableStock < quantity)
            {
                _logger.LogWarning("Yetersiz stok rezervasyonu. ProductId: {ProductId}, Required: {Required}, Available: {Available}", 
                    productId, quantity, inventory.AvailableStock);
                return false;
            }

            inventory.ReservedStock += quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            inventory.UpdatedAt = DateTime.UtcNow;

            // Stok hareketi kaydet
            var movement = new InventoryMovement
            {
                ProductId = productId,
                MovementType = InventoryMovementType.OrderReservation.ToString(),
                Quantity = quantity,
                PreviousStock = inventory.CurrentStock,
                NewStock = inventory.CurrentStock,
                Reason = $"{relatedEntityType} rezervasyonu",
                RelatedEntityId = relatedEntityId,
                RelatedEntityType = relatedEntityType,
                MovementDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.InventoryMovements.AddAsync(movement);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Stok rezervasyonu başarılı. ProductId: {ProductId}, ReservedStock: {ReservedStock}", 
                productId, inventory.ReservedStock);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok rezervasyonu sırasında hata oluştu. ProductId: {ProductId}", productId);
            return false;
        }
    }

    public async Task<bool> ReleaseReservedStockAsync(Guid productId, decimal quantity, Guid relatedEntityId, string relatedEntityType, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Stok rezervasyonu kaldırılıyor. ProductId: {ProductId}, Quantity: {Quantity}", productId, quantity);

            var inventory = await GetOrCreateInventoryAsync(productId);
            if (inventory == null)
                return false;

            if (inventory.ReservedStock < quantity)
            {
                _logger.LogWarning("Yetersiz rezerve stok. ProductId: {ProductId}, Required: {Required}, Reserved: {Reserved}", 
                    productId, quantity, inventory.ReservedStock);
                return false;
            }

            inventory.ReservedStock -= quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            inventory.UpdatedAt = DateTime.UtcNow;

            // Stok hareketi kaydet
            var movement = new InventoryMovement
            {
                ProductId = productId,
                MovementType = InventoryMovementType.OrderCancellation.ToString(),
                Quantity = quantity,
                PreviousStock = inventory.CurrentStock,
                NewStock = inventory.CurrentStock,
                Reason = $"{relatedEntityType} rezervasyon iptali",
                RelatedEntityId = relatedEntityId,
                RelatedEntityType = relatedEntityType,
                MovementDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.InventoryMovements.AddAsync(movement);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Stok rezervasyonu kaldırma başarılı. ProductId: {ProductId}, ReservedStock: {ReservedStock}", 
                productId, inventory.ReservedStock);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok rezervasyonu kaldırma sırasında hata oluştu. ProductId: {ProductId}", productId);
            return false;
        }
    }

    public async Task<bool> CheckStockAvailabilityAsync(List<OrderItemDto> orderItems, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var item in orderItems)
            {
                var inventory = await GetOrCreateInventoryAsync(item.ProductId);
                if (inventory == null)
                    return false;

                if (inventory.AvailableStock < item.Quantity)
                {
                    _logger.LogWarning("Yetersiz stok. ProductId: {ProductId}, Required: {Required}, Available: {Available}", 
                        item.ProductId, item.Quantity, inventory.AvailableStock);
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok kontrolü sırasında hata oluştu");
            return false;
        }
    }

    public async Task<bool> ProcessOrderStockAsync(Guid orderId, List<OrderItemDto> orderItems, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Sipariş stok işlemi başlatılıyor. OrderId: {OrderId}", orderId);

            foreach (var item in orderItems)
            {
                // Rezerve stoku kaldır ve gerçek stoktan düş
                var inventory = await GetOrCreateInventoryAsync(item.ProductId);
                if (inventory == null)
                    return false;

                // Rezerve stoku kaldır
                inventory.ReservedStock -= item.Quantity;
                
                // Gerçek stoktan düş
                var previousStock = inventory.CurrentStock;
                inventory.CurrentStock -= item.Quantity;
                inventory.LastUpdated = DateTime.UtcNow;
                inventory.UpdatedAt = DateTime.UtcNow;

                // Stok hareketi kaydet
                var movement = new InventoryMovement
                {
                    ProductId = item.ProductId,
                    MovementType = InventoryMovementType.OrderConfirmation.ToString(),
                    Quantity = item.Quantity,
                    PreviousStock = previousStock,
                    NewStock = inventory.CurrentStock,
                    Reason = "Sipariş onayı",
                    RelatedEntityId = orderId,
                    RelatedEntityType = "Order",
                    MovementDate = DateTime.UtcNow,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.InventoryMovements.AddAsync(movement);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Sipariş stok işlemi başarılı. OrderId: {OrderId}", orderId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş stok işlemi sırasında hata oluştu. OrderId: {OrderId}", orderId);
            return false;
        }
    }

    public async Task<int> CheckStockAlertsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Stok uyarıları kontrol ediliyor");

            var inventories = await _unitOfWork.Inventories.GetAll()
                .Where(i => i.IsActive)
                .Include(i => i.Product)
                .ToListAsync(cancellationToken);

            var alertCount = 0;

            foreach (var inventory in inventories)
            {
                // Düşük stok uyarısı
                if (inventory.CurrentStock <= inventory.AlertStock && inventory.CurrentStock > 0)
                {
                    var existingAlert = await _unitOfWork.StockAlerts.FirstOrDefaultAsync(
                        a => a.ProductId == inventory.ProductId && 
                             a.AlertType == StockAlertType.LowStock.ToString() && 
                             a.Status == "Active");

                    if (existingAlert == null)
                    {
                        var alert = new StockAlert
                        {
                            ProductId = inventory.ProductId,
                            AlertType = StockAlertType.LowStock.ToString(),
                            CurrentStock = inventory.CurrentStock,
                            AlertLevel = inventory.AlertStock,
                            Message = $"{inventory.Product.Name} ürününde stok düşük! Mevcut: {inventory.CurrentStock}, Uyarı seviyesi: {inventory.AlertStock}",
                            Status = "Active",
                            AlertDate = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        await _unitOfWork.StockAlerts.AddAsync(alert);
                        alertCount++;
                    }
                }

                // Stok tükendi uyarısı
                if (inventory.CurrentStock <= 0)
                {
                    var existingAlert = await _unitOfWork.StockAlerts.FirstOrDefaultAsync(
                        a => a.ProductId == inventory.ProductId && 
                             a.AlertType == StockAlertType.OutOfStock.ToString() && 
                             a.Status == "Active");

                    if (existingAlert == null)
                    {
                        var alert = new StockAlert
                        {
                            ProductId = inventory.ProductId,
                            AlertType = StockAlertType.OutOfStock.ToString(),
                            CurrentStock = inventory.CurrentStock,
                            AlertLevel = 0,
                            Message = $"{inventory.Product.Name} ürününde stok tükendi!",
                            Status = "Active",
                            AlertDate = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        await _unitOfWork.StockAlerts.AddAsync(alert);
                        alertCount++;
                    }
                }
            }

            if (alertCount > 0)
            {
                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Stok uyarı kontrolü tamamlandı. Oluşturulan uyarı sayısı: {AlertCount}", alertCount);
            return alertCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok uyarı kontrolü sırasında hata oluştu");
            return 0;
        }
    }

    public async Task<bool> MarkAlertAsReadAsync(Guid alertId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var alert = await _unitOfWork.StockAlerts.GetByIdAsync(alertId);
            if (alert == null)
                return false;

            alert.IsRead = true;
            alert.ReadDate = DateTime.UtcNow;
            alert.ReadByUserId = userId;
            alert.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Stok uyarısı okundu olarak işaretlendi. AlertId: {AlertId}, UserId: {UserId}", alertId, userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok uyarısı okundu işaretleme sırasında hata oluştu. AlertId: {AlertId}", alertId);
            return false;
        }
    }

    private async Task<Domain.Entities.Inventory?> GetOrCreateInventoryAsync(Guid productId)
    {
        var inventory = await _unitOfWork.Inventories.FirstOrDefaultAsync(
            i => i.ProductId == productId && i.IsActive);

        if (inventory == null)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
                return null;

            inventory = new Domain.Entities.Inventory
            {
                ProductId = productId,
                CurrentStock = 0,
                ReservedStock = 0,
                MinimumStock = 0,
                MaximumStock = 1000,
                AlertStock = 10,
                IsActive = true,
                LastUpdated = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Inventories.AddAsync(inventory);
            await _unitOfWork.SaveChangesAsync();
        }

        return inventory;
    }
}
