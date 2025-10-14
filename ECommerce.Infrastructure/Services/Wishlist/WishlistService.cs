using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Services.Wishlist;

/// <summary>
/// Favori liste servis implementasyonu
/// </summary>
public class WishlistService : IWishlistService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WishlistService> _logger;
    private readonly INotificationService _notificationService;

    public WishlistService(
        IUnitOfWork unitOfWork,
        ILogger<WishlistService> logger,
        INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task<WishlistDto?> CreateWishlistAsync(CreateWishlistDto createWishlistDto, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Favori liste oluşturuluyor. UserId: {UserId}, Name: {Name}", userId, createWishlistDto.Name);

            // Eğer varsayılan liste olarak işaretleniyorsa, diğer listeleri varsayılan olmaktan çıkar
            if (createWishlistDto.IsDefault)
            {
                var existingDefaultLists = await _unitOfWork.Wishlists.GetAll()
                    .Where(w => w.UserId == userId && w.IsDefault && !w.IsDeleted)
                    .ToListAsync(cancellationToken);

                foreach (var list in existingDefaultLists)
                {
                    list.IsDefault = false;
                    list.UpdatedAt = DateTime.UtcNow;
                }
            }

            var wishlist = new Domain.Entities.Wishlist
            {
                UserId = userId,
                Name = createWishlistDto.Name,
                Description = createWishlistDto.Description,
                ListType = createWishlistDto.ListType,
                IsShareable = createWishlistDto.IsShareable,
                IsDefault = createWishlistDto.IsDefault,
                SortOrder = createWishlistDto.SortOrder,
                Color = createWishlistDto.Color,
                Icon = createWishlistDto.Icon,
                PriceTrackingEnabled = createWishlistDto.PriceTrackingEnabled,
                StockTrackingEnabled = createWishlistDto.StockTrackingEnabled,
                EmailNotificationsEnabled = createWishlistDto.EmailNotificationsEnabled,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Wishlists.AddAsync(wishlist);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Favori liste başarıyla oluşturuldu. Id: {Id}, UserId: {UserId}", wishlist.Id, userId);

            return await GetWishlistAsync(wishlist.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Favori liste oluşturma sırasında hata oluştu. UserId: {UserId}", userId);
            return null;
        }
    }

    public async Task<WishlistDto?> UpdateWishlistAsync(Guid wishlistId, CreateWishlistDto updateWishlistDto, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Favori liste güncelleniyor. WishlistId: {WishlistId}, UserId: {UserId}", wishlistId, userId);

            var wishlist = await _unitOfWork.Wishlists.GetByIdAsync(wishlistId);
            if (wishlist == null || wishlist.UserId != userId)
            {
                _logger.LogWarning("Favori liste bulunamadı veya kullanıcı yetkisi yok. WishlistId: {WishlistId}, UserId: {UserId}", wishlistId, userId);
                return null;
            }

            // Eğer varsayılan liste olarak işaretleniyorsa, diğer listeleri varsayılan olmaktan çıkar
            if (updateWishlistDto.IsDefault && !wishlist.IsDefault)
            {
                var existingDefaultLists = await _unitOfWork.Wishlists.GetAll()
                    .Where(w => w.UserId == userId && w.IsDefault && w.Id != wishlistId && !w.IsDeleted)
                    .ToListAsync(cancellationToken);

                foreach (var list in existingDefaultLists)
                {
                    list.IsDefault = false;
                    list.UpdatedAt = DateTime.UtcNow;
                }
            }

            wishlist.Name = updateWishlistDto.Name;
            wishlist.Description = updateWishlistDto.Description;
            wishlist.ListType = updateWishlistDto.ListType;
            wishlist.IsShareable = updateWishlistDto.IsShareable;
            wishlist.IsDefault = updateWishlistDto.IsDefault;
            wishlist.SortOrder = updateWishlistDto.SortOrder;
            wishlist.Color = updateWishlistDto.Color;
            wishlist.Icon = updateWishlistDto.Icon;
            wishlist.PriceTrackingEnabled = updateWishlistDto.PriceTrackingEnabled;
            wishlist.StockTrackingEnabled = updateWishlistDto.StockTrackingEnabled;
            wishlist.EmailNotificationsEnabled = updateWishlistDto.EmailNotificationsEnabled;
            wishlist.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Favori liste başarıyla güncellendi. WishlistId: {WishlistId}", wishlistId);

            return await GetWishlistAsync(wishlistId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Favori liste güncelleme sırasında hata oluştu. WishlistId: {WishlistId}", wishlistId);
            return null;
        }
    }

    public async Task<bool> DeleteWishlistAsync(Guid wishlistId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Favori liste siliniyor. WishlistId: {WishlistId}, UserId: {UserId}", wishlistId, userId);

            var wishlist = await _unitOfWork.Wishlists.GetByIdAsync(wishlistId);
            if (wishlist == null || wishlist.UserId != userId)
            {
                _logger.LogWarning("Favori liste bulunamadı veya kullanıcı yetkisi yok. WishlistId: {WishlistId}, UserId: {UserId}", wishlistId, userId);
                return false;
            }

            // Varsayılan liste silinemez
            if (wishlist.IsDefault)
            {
                _logger.LogWarning("Varsayılan favori liste silinemez. WishlistId: {WishlistId}", wishlistId);
                return false;
            }

            wishlist.IsDeleted = true;
            wishlist.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Favori liste başarıyla silindi. WishlistId: {WishlistId}", wishlistId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Favori liste silme sırasında hata oluştu. WishlistId: {WishlistId}", wishlistId);
            return false;
        }
    }

    public async Task<WishlistDto?> GetWishlistAsync(Guid wishlistId, CancellationToken cancellationToken = default)
    {
        try
        {
            var wishlist = await _unitOfWork.Wishlists.GetAll()
                .Include(w => w.User)
                .Include(w => w.WishlistItems)
                    .ThenInclude(wi => wi.Product)
                .Include(w => w.WishlistShares)
                .FirstOrDefaultAsync(w => w.Id == wishlistId, cancellationToken);

            if (wishlist == null)
                return null;

            return MapToWishlistDto(wishlist);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Favori liste bilgileri alınırken hata oluştu. WishlistId: {WishlistId}", wishlistId);
            return null;
        }
    }

    public async Task<List<WishlistDto>> GetUserWishlistsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var wishlists = await _unitOfWork.Wishlists.GetAll()
                .Include(w => w.User)
                .Include(w => w.WishlistItems)
                .Include(w => w.WishlistShares)
                .Where(w => w.UserId == userId && !w.IsDeleted)
                .OrderBy(w => w.SortOrder)
                .ThenBy(w => w.CreatedAt)
                .ToListAsync(cancellationToken);

            return wishlists.Select(MapToWishlistDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı favori listeleri alınırken hata oluştu. UserId: {UserId}", userId);
            return new List<WishlistDto>();
        }
    }

    public async Task<WishlistDto?> GetDefaultWishlistAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var wishlist = await _unitOfWork.Wishlists.GetAll()
                .Include(w => w.User)
                .Include(w => w.WishlistItems)
                    .ThenInclude(wi => wi.Product)
                .Include(w => w.WishlistShares)
                .FirstOrDefaultAsync(w => w.UserId == userId && w.IsDefault && !w.IsDeleted, cancellationToken);

            if (wishlist == null)
            {
                // Varsayılan liste yoksa oluştur
                var createDto = new CreateWishlistDto
                {
                    Name = "Favorilerim",
                    Description = "Varsayılan favori listem",
                    ListType = "Personal",
                    IsDefault = true
                };

                return await CreateWishlistAsync(createDto, userId, cancellationToken);
            }

            return MapToWishlistDto(wishlist);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Varsayılan favori liste alınırken hata oluştu. UserId: {UserId}", userId);
            return null;
        }
    }

    public async Task<WishlistItemDto?> AddToWishlistAsync(AddToWishlistDto addToWishlistDto, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Ürün favorilere ekleniyor. ProductId: {ProductId}, UserId: {UserId}", addToWishlistDto.ProductId, userId);

            // Ürün zaten favorilerde var mı kontrol et
            var existingItem = await _unitOfWork.WishlistItems.GetAll()
                .Include(wi => wi.Wishlist)
                .FirstOrDefaultAsync(wi => wi.ProductId == addToWishlistDto.ProductId && 
                                          wi.Wishlist.UserId == userId && 
                                          !wi.IsDeleted, cancellationToken);

            if (existingItem != null)
            {
                _logger.LogWarning("Ürün zaten favorilerde. ProductId: {ProductId}, UserId: {UserId}", addToWishlistDto.ProductId, userId);
                return null;
            }

            // Favori liste belirle
            Domain.Entities.Wishlist wishlist;
            if (addToWishlistDto.WishlistId.HasValue)
            {
                wishlist = await _unitOfWork.Wishlists.GetByIdAsync(addToWishlistDto.WishlistId.Value);
                if (wishlist == null || wishlist.UserId != userId)
                {
                    _logger.LogWarning("Favori liste bulunamadı veya kullanıcı yetkisi yok. WishlistId: {WishlistId}, UserId: {UserId}", addToWishlistDto.WishlistId, userId);
                    return null;
                }
            }
            else
            {
                // Varsayılan listeyi kullan
                var defaultWishlist = await GetDefaultWishlistAsync(userId, cancellationToken);
                if (defaultWishlist == null)
                {
                    _logger.LogError("Varsayılan favori liste oluşturulamadı. UserId: {UserId}", userId);
                    return null;
                }
                wishlist = await _unitOfWork.Wishlists.GetByIdAsync(defaultWishlist.Id);
            }

            // Ürün bilgilerini al
            var product = await _unitOfWork.Products.GetByIdAsync(addToWishlistDto.ProductId);
            if (product == null)
            {
                _logger.LogWarning("Ürün bulunamadı. ProductId: {ProductId}", addToWishlistDto.ProductId);
                return null;
            }

            var wishlistItem = new WishlistItem
            {
                WishlistId = wishlist.Id,
                ProductId = addToWishlistDto.ProductId,
                PriceAtTime = product.Price,
                DiscountedPriceAtTime = product.DiscountedPrice,
                WasInStock = product.StockQuantity > 0,
                Notes = addToWishlistDto.Notes,
                Priority = addToWishlistDto.Priority,
                TargetPrice = addToWishlistDto.TargetPrice,
                PriceTrackingEnabled = addToWishlistDto.PriceTrackingEnabled,
                StockTrackingEnabled = addToWishlistDto.StockTrackingEnabled,
                EmailNotificationsEnabled = addToWishlistDto.EmailNotificationsEnabled,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.WishlistItems.AddAsync(wishlistItem);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Ürün başarıyla favorilere eklendi. WishlistItemId: {WishlistItemId}, ProductId: {ProductId}", wishlistItem.Id, addToWishlistDto.ProductId);

            return await GetWishlistItemAsync(wishlistItem.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün favorilere ekleme sırasında hata oluştu. ProductId: {ProductId}", addToWishlistDto.ProductId);
            return null;
        }
    }

    public async Task<bool> RemoveFromWishlistAsync(Guid wishlistItemId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Ürün favorilerden çıkarılıyor. WishlistItemId: {WishlistItemId}, UserId: {UserId}", wishlistItemId, userId);

            var wishlistItem = await _unitOfWork.WishlistItems.GetAll()
                .Include(wi => wi.Wishlist)
                .FirstOrDefaultAsync(wi => wi.Id == wishlistItemId && wi.Wishlist.UserId == userId, cancellationToken);

            if (wishlistItem == null)
            {
                _logger.LogWarning("Favori ürün bulunamadı veya kullanıcı yetkisi yok. WishlistItemId: {WishlistItemId}, UserId: {UserId}", wishlistItemId, userId);
                return false;
            }

            wishlistItem.IsDeleted = true;
            wishlistItem.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Ürün başarıyla favorilerden çıkarıldı. WishlistItemId: {WishlistItemId}", wishlistItemId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün favorilerden çıkarma sırasında hata oluştu. WishlistItemId: {WishlistItemId}", wishlistItemId);
            return false;
        }
    }

    public async Task<WishlistItemDto?> UpdateWishlistItemAsync(Guid wishlistItemId, AddToWishlistDto updateWishlistItemDto, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Favori ürün güncelleniyor. WishlistItemId: {WishlistItemId}, UserId: {UserId}", wishlistItemId, userId);

            var wishlistItem = await _unitOfWork.WishlistItems.GetAll()
                .Include(wi => wi.Wishlist)
                .FirstOrDefaultAsync(wi => wi.Id == wishlistItemId && wi.Wishlist.UserId == userId, cancellationToken);

            if (wishlistItem == null)
            {
                _logger.LogWarning("Favori ürün bulunamadı veya kullanıcı yetkisi yok. WishlistItemId: {WishlistItemId}, UserId: {UserId}", wishlistItemId, userId);
                return null;
            }

            wishlistItem.Notes = updateWishlistItemDto.Notes;
            wishlistItem.Priority = updateWishlistItemDto.Priority;
            wishlistItem.TargetPrice = updateWishlistItemDto.TargetPrice;
            wishlistItem.PriceTrackingEnabled = updateWishlistItemDto.PriceTrackingEnabled;
            wishlistItem.StockTrackingEnabled = updateWishlistItemDto.StockTrackingEnabled;
            wishlistItem.EmailNotificationsEnabled = updateWishlistItemDto.EmailNotificationsEnabled;
            wishlistItem.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Favori ürün başarıyla güncellendi. WishlistItemId: {WishlistItemId}", wishlistItemId);

            return await GetWishlistItemAsync(wishlistItemId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Favori ürün güncelleme sırasında hata oluştu. WishlistItemId: {WishlistItemId}", wishlistItemId);
            return null;
        }
    }

    public async Task<WishlistShareDto?> ShareWishlistAsync(Guid wishlistId, string shareType, string? emailAddress = null, string? message = null, int? expirationDays = null, Guid userId = default, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Favori liste paylaşılıyor. WishlistId: {WishlistId}, ShareType: {ShareType}, UserId: {UserId}", wishlistId, shareType, userId);

            var wishlist = await _unitOfWork.Wishlists.GetByIdAsync(wishlistId);
            if (wishlist == null || wishlist.UserId != userId)
            {
                _logger.LogWarning("Favori liste bulunamadı veya kullanıcı yetkisi yok. WishlistId: {WishlistId}, UserId: {UserId}", wishlistId, userId);
                return null;
            }

            // Paylaşım kodu oluştur
            var shareCode = Guid.NewGuid().ToString("N")[..8].ToUpper();

            var wishlistShare = new WishlistShare
            {
                WishlistId = wishlistId,
                ShareType = shareType,
                ShareCode = shareCode,
                EmailAddress = emailAddress,
                Message = message,
                ExpirationDays = expirationDays,
                ExpiresAt = expirationDays.HasValue ? DateTime.UtcNow.AddDays(expirationDays.Value) : null,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.WishlistShares.AddAsync(wishlistShare);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Favori liste başarıyla paylaşıldı. ShareId: {ShareId}, ShareCode: {ShareCode}", wishlistShare.Id, shareCode);

            return new WishlistShareDto
            {
                Id = wishlistShare.Id,
                WishlistId = wishlistShare.WishlistId,
                ShareType = wishlistShare.ShareType,
                ShareCode = wishlistShare.ShareCode,
                EmailAddress = wishlistShare.EmailAddress,
                Message = wishlistShare.Message,
                ExpirationDays = wishlistShare.ExpirationDays,
                ExpiresAt = wishlistShare.ExpiresAt,
                IsActive = wishlistShare.IsActive,
                ViewCount = wishlistShare.ViewCount,
                LastViewedAt = wishlistShare.LastViewedAt,
                IsExpired = wishlistShare.IsExpired,
                IsValid = wishlistShare.IsValid,
                CreatedAt = wishlistShare.CreatedAt,
                UpdatedAt = wishlistShare.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Favori liste paylaşma sırasında hata oluştu. WishlistId: {WishlistId}", wishlistId);
            return null;
        }
    }

    public async Task<WishlistDto?> GetSharedWishlistAsync(string shareCode, CancellationToken cancellationToken = default)
    {
        try
        {
            var wishlistShare = await _unitOfWork.WishlistShares.GetAll()
                .Include(ws => ws.Wishlist)
                    .ThenInclude(w => w.User)
                .Include(ws => ws.Wishlist)
                    .ThenInclude(w => w.WishlistItems)
                        .ThenInclude(wi => wi.Product)
                .FirstOrDefaultAsync(ws => ws.ShareCode == shareCode && ws.IsValid, cancellationToken);

            if (wishlistShare == null)
            {
                _logger.LogWarning("Paylaşılan favori liste bulunamadı. ShareCode: {ShareCode}", shareCode);
                return null;
            }

            // Görüntülenme sayısını artır
            wishlistShare.ViewCount++;
            wishlistShare.LastViewedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            return MapToWishlistDto(wishlistShare.Wishlist);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Paylaşılan favori liste alınırken hata oluştu. ShareCode: {ShareCode}", shareCode);
            return null;
        }
    }

    public async Task<bool> CancelShareAsync(Guid shareId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Favori liste paylaşımı iptal ediliyor. ShareId: {ShareId}, UserId: {UserId}", shareId, userId);

            var wishlistShare = await _unitOfWork.WishlistShares.GetAll()
                .Include(ws => ws.Wishlist)
                .FirstOrDefaultAsync(ws => ws.Id == shareId && ws.Wishlist.UserId == userId, cancellationToken);

            if (wishlistShare == null)
            {
                _logger.LogWarning("Paylaşım bulunamadı veya kullanıcı yetkisi yok. ShareId: {ShareId}, UserId: {UserId}", shareId, userId);
                return false;
            }

            wishlistShare.IsActive = false;
            wishlistShare.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Favori liste paylaşımı başarıyla iptal edildi. ShareId: {ShareId}", shareId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Favori liste paylaşımı iptal etme sırasında hata oluştu. ShareId: {ShareId}", shareId);
            return false;
        }
    }

    public async Task<bool> IsProductInWishlistAsync(Guid productId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _unitOfWork.WishlistItems.GetAll()
                .Include(wi => wi.Wishlist)
                .AnyAsync(wi => wi.ProductId == productId && 
                               wi.Wishlist.UserId == userId && 
                               !wi.IsDeleted, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün favori kontrolü sırasında hata oluştu. ProductId: {ProductId}, UserId: {UserId}", productId, userId);
            return false;
        }
    }

    public async Task<List<WishlistDto>> GetWishlistsContainingProductAsync(Guid productId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var wishlists = await _unitOfWork.Wishlists.GetAll()
                .Include(w => w.User)
                .Include(w => w.WishlistItems)
                .Where(w => w.UserId == userId && 
                           w.WishlistItems.Any(wi => wi.ProductId == productId && !wi.IsDeleted) && 
                           !w.IsDeleted)
                .ToListAsync(cancellationToken);

            return wishlists.Select(MapToWishlistDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün içeren favori listeler alınırken hata oluştu. ProductId: {ProductId}, UserId: {UserId}", productId, userId);
            return new List<WishlistDto>();
        }
    }

    public async Task<bool> CheckPriceChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fiyat değişiklikleri kontrol ediliyor");

            var wishlistItems = await _unitOfWork.WishlistItems.GetAll()
                .Include(wi => wi.Product)
                .Include(wi => wi.Wishlist)
                    .ThenInclude(w => w.User)
                .Where(wi => wi.PriceTrackingEnabled && !wi.IsDeleted)
                .ToListAsync(cancellationToken);

            var priceChangedItems = new List<WishlistItem>();

            foreach (var item in wishlistItems)
            {
                if (item.Product == null) continue;

                var currentPrice = item.Product.Price;
                var oldPrice = item.PriceAtTime;

                if (currentPrice != oldPrice)
                {
                    // Fiyat geçmişi kaydet
                    var priceHistory = new WishlistItemPriceHistory
                    {
                        WishlistItemId = item.Id,
                        OldPrice = oldPrice,
                        NewPrice = currentPrice,
                        PriceChangePercentage = oldPrice > 0 ? ((currentPrice - oldPrice) / oldPrice) * 100 : 0,
                        ChangeType = currentPrice > oldPrice ? "Increase" : "Decrease",
                        ChangeReason = "Otomatik fiyat takibi",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.WishlistItemPriceHistories.AddAsync(priceHistory);

                    // Favori ürün fiyatını güncelle
                    item.PriceAtTime = currentPrice;
                    item.UpdatedAt = DateTime.UtcNow;

                    priceChangedItems.Add(item);
                }
            }

            if (priceChangedItems.Any())
            {
                await _unitOfWork.SaveChangesAsync();

                // Bildirim gönder
                foreach (var item in priceChangedItems)
                {
                    if (item.EmailNotificationsEnabled && item.Wishlist.EmailNotificationsEnabled)
                    {
                        await _notificationService.CreateNotificationAsync(
                            item.Wishlist.UserId,
                            $"Fiyat Değişikliği - {item.Product.Name}",
                            $"{item.Product.Name} ürününün fiyatı değişti.",
                            Domain.Enums.NotificationType.Price,
                            Domain.Enums.NotificationPriority.Normal,
                            "Product",
                            item.ProductId,
                            item.Wishlist.User.FirstName + " " + item.Wishlist.User.LastName);
                    }
                }

                _logger.LogInformation("Fiyat değişiklikleri kontrol edildi. {Count} ürün fiyatı değişti", priceChangedItems.Count);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fiyat değişiklikleri kontrol edilirken hata oluştu");
            return false;
        }
    }

    public async Task<bool> CheckStockChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Stok değişiklikleri kontrol ediliyor");

            var wishlistItems = await _unitOfWork.WishlistItems.GetAll()
                .Include(wi => wi.Product)
                .Include(wi => wi.Wishlist)
                    .ThenInclude(w => w.User)
                .Where(wi => wi.StockTrackingEnabled && !wi.IsDeleted)
                .ToListAsync(cancellationToken);

            var stockChangedItems = new List<WishlistItem>();

            foreach (var item in wishlistItems)
            {
                if (item.Product == null) continue;

                var currentStockStatus = item.Product.StockQuantity > 0;
                var oldStockStatus = item.WasInStock;

                if (currentStockStatus != oldStockStatus)
                {
                    // Stok geçmişi kaydet
                    var stockHistory = new WishlistItemStockHistory
                    {
                        WishlistItemId = item.Id,
                        OldStockStatus = oldStockStatus,
                        NewStockStatus = currentStockStatus,
                        OldStockQuantity = oldStockStatus ? 1 : 0, // Tam miktar bilinmiyor
                        NewStockQuantity = item.Product.StockQuantity,
                        ChangeType = currentStockStatus ? "InStock" : "OutOfStock",
                        ChangeReason = "Otomatik stok takibi",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.WishlistItemStockHistories.AddAsync(stockHistory);

                    // Favori ürün stok durumunu güncelle
                    item.WasInStock = currentStockStatus;
                    item.UpdatedAt = DateTime.UtcNow;

                    stockChangedItems.Add(item);
                }
            }

            if (stockChangedItems.Any())
            {
                await _unitOfWork.SaveChangesAsync();

                // Bildirim gönder
                foreach (var item in stockChangedItems)
                {
                    if (item.EmailNotificationsEnabled && item.Wishlist.EmailNotificationsEnabled)
                    {
                        await _notificationService.CreateNotificationAsync(
                            item.Wishlist.UserId,
                            $"Stok Değişikliği - {item.Product.Name}",
                            $"{item.Product.Name} ürününün stok durumu değişti.",
                            Domain.Enums.NotificationType.Stock,
                            Domain.Enums.NotificationPriority.Normal,
                            "Product",
                            item.ProductId,
                            item.Wishlist.User.FirstName + " " + item.Wishlist.User.LastName);
                    }
                }

                _logger.LogInformation("Stok değişiklikleri kontrol edildi. {Count} ürün stok durumu değişti", stockChangedItems.Count);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok değişiklikleri kontrol edilirken hata oluştu");
            return false;
        }
    }

    public async Task<WishlistStatsDto> GetWishlistStatsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var wishlists = await _unitOfWork.Wishlists.GetAll()
                .Include(w => w.WishlistItems)
                    .ThenInclude(wi => wi.Product)
                .Where(w => w.UserId == userId && !w.IsDeleted)
                .ToListAsync(cancellationToken);

            var wishlistItems = wishlists.SelectMany(w => w.WishlistItems).Where(wi => !wi.IsDeleted).ToList();

            var stats = new WishlistStatsDto
            {
                UserId = userId,
                TotalWishlists = wishlists.Count,
                TotalWishlistItems = wishlistItems.Count,
                SharedWishlists = wishlists.Count(w => !string.IsNullOrEmpty(w.ShareCode)),
                PriceTrackingItems = wishlistItems.Count(wi => wi.PriceTrackingEnabled),
                StockTrackingItems = wishlistItems.Count(wi => wi.StockTrackingEnabled),
                PriceDroppedItems = wishlistItems.Count(wi => wi.HasPriceDropped),
                TargetPriceReachedItems = wishlistItems.Count(wi => wi.HasReachedTargetPrice),
                StockStatusChangedItems = wishlistItems.Count(wi => wi.HasStockStatusChanged),
                LastUpdatedAt = DateTime.UtcNow
            };

            // Toplam tasarruf hesapla
            stats.TotalSavings = wishlistItems
                .Where(wi => wi.HasPriceDropped && wi.Product != null)
                .Sum(wi => wi.PriceAtTime - wi.Product.Price);

            // Ortalama fiyat değişim yüzdesi
            var priceChanges = wishlistItems
                .Where(wi => wi.HasPriceDropped && wi.Product != null)
                .Select(wi => ((wi.Product.Price - wi.PriceAtTime) / wi.PriceAtTime) * 100)
                .ToList();

            stats.AveragePriceChangePercentage = priceChanges.Any() ? priceChanges.Average() : 0;

            return stats;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Favori liste istatistikleri alınırken hata oluştu. UserId: {UserId}", userId);
            return new WishlistStatsDto { UserId = userId };
        }
    }

    private async Task<WishlistItemDto?> GetWishlistItemAsync(Guid wishlistItemId, CancellationToken cancellationToken = default)
    {
        try
        {
            var wishlistItem = await _unitOfWork.WishlistItems.GetAll()
                .Include(wi => wi.Product)
                .Include(wi => wi.Wishlist)
                .Include(wi => wi.PriceHistory)
                .Include(wi => wi.StockHistory)
                .FirstOrDefaultAsync(wi => wi.Id == wishlistItemId, cancellationToken);

            if (wishlistItem == null)
                return null;

            return MapToWishlistItemDto(wishlistItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Favori ürün bilgileri alınırken hata oluştu. WishlistItemId: {WishlistItemId}", wishlistItemId);
            return null;
        }
    }

    private WishlistDto MapToWishlistDto(Domain.Entities.Wishlist wishlist)
    {
        return new WishlistDto
        {
            Id = wishlist.Id,
            UserId = wishlist.UserId,
            UserName = wishlist.User.FirstName + " " + wishlist.User.LastName,
            Name = wishlist.Name,
            Description = wishlist.Description,
            ListType = wishlist.ListType,
            IsShareable = wishlist.IsShareable,
            ShareCode = wishlist.ShareCode,
            IsDefault = wishlist.IsDefault,
            SortOrder = wishlist.SortOrder,
            Color = wishlist.Color,
            Icon = wishlist.Icon,
            PriceTrackingEnabled = wishlist.PriceTrackingEnabled,
            StockTrackingEnabled = wishlist.StockTrackingEnabled,
            EmailNotificationsEnabled = wishlist.EmailNotificationsEnabled,
            IsActive = wishlist.IsActive,
            IsShared = wishlist.IsShared,
            TotalItems = wishlist.TotalItems,
            CreatedAt = wishlist.CreatedAt,
            UpdatedAt = wishlist.UpdatedAt,
            WishlistItems = wishlist.WishlistItems?.Select(MapToWishlistItemDto).ToList(),
            WishlistShares = wishlist.WishlistShares?.Select(MapToWishlistShareDto).ToList()
        };
    }

    private WishlistItemDto MapToWishlistItemDto(WishlistItem wishlistItem)
    {
        return new WishlistItemDto
        {
            Id = wishlistItem.Id,
            WishlistId = wishlistItem.WishlistId,
            ProductId = wishlistItem.ProductId,
            Product = wishlistItem.Product != null ? new ProductDto
            {
                Id = wishlistItem.Product.Id,
                Name = wishlistItem.Product.Name,
                Description = wishlistItem.Product.Description,
                Price = wishlistItem.Product.Price,
                DiscountedPrice = wishlistItem.Product.DiscountedPrice,
                StockQuantity = wishlistItem.Product.StockQuantity,
                IsActive = wishlistItem.Product.IsActive
            } : null,
            PriceAtTime = wishlistItem.PriceAtTime,
            DiscountedPriceAtTime = wishlistItem.DiscountedPriceAtTime,
            WasInStock = wishlistItem.WasInStock,
            Notes = wishlistItem.Notes,
            Priority = wishlistItem.Priority,
            TargetPrice = wishlistItem.TargetPrice,
            PriceTrackingEnabled = wishlistItem.PriceTrackingEnabled,
            StockTrackingEnabled = wishlistItem.StockTrackingEnabled,
            EmailNotificationsEnabled = wishlistItem.EmailNotificationsEnabled,
            LastPriceNotificationAt = wishlistItem.LastPriceNotificationAt,
            LastStockNotificationAt = wishlistItem.LastStockNotificationAt,
            IsProductActive = wishlistItem.IsProductActive,
            HasPriceDropped = wishlistItem.HasPriceDropped,
            HasReachedTargetPrice = wishlistItem.HasReachedTargetPrice,
            HasStockStatusChanged = wishlistItem.HasStockStatusChanged,
            CreatedAt = wishlistItem.CreatedAt,
            UpdatedAt = wishlistItem.UpdatedAt,
            PriceHistory = wishlistItem.PriceHistory?.Select(MapToPriceHistoryDto).ToList(),
            StockHistory = wishlistItem.StockHistory?.Select(MapToStockHistoryDto).ToList()
        };
    }

    private WishlistShareDto MapToWishlistShareDto(WishlistShare wishlistShare)
    {
        return new WishlistShareDto
        {
            Id = wishlistShare.Id,
            WishlistId = wishlistShare.WishlistId,
            SharedWithUserId = wishlistShare.SharedWithUserId,
            SharedWithUserName = wishlistShare.SharedWithUser != null ? 
                wishlistShare.SharedWithUser.FirstName + " " + wishlistShare.SharedWithUser.LastName : null,
            ShareType = wishlistShare.ShareType,
            ShareCode = wishlistShare.ShareCode,
            EmailAddress = wishlistShare.EmailAddress,
            Message = wishlistShare.Message,
            ExpirationDays = wishlistShare.ExpirationDays,
            ExpiresAt = wishlistShare.ExpiresAt,
            IsActive = wishlistShare.IsActive,
            ViewCount = wishlistShare.ViewCount,
            LastViewedAt = wishlistShare.LastViewedAt,
            IsExpired = wishlistShare.IsExpired,
            IsValid = wishlistShare.IsValid,
            CreatedAt = wishlistShare.CreatedAt,
            UpdatedAt = wishlistShare.UpdatedAt
        };
    }

    private WishlistItemPriceHistoryDto MapToPriceHistoryDto(WishlistItemPriceHistory priceHistory)
    {
        return new WishlistItemPriceHistoryDto
        {
            Id = priceHistory.Id,
            WishlistItemId = priceHistory.WishlistItemId,
            OldPrice = priceHistory.OldPrice,
            NewPrice = priceHistory.NewPrice,
            PriceChangePercentage = priceHistory.PriceChangePercentage,
            ChangeType = priceHistory.ChangeType,
            ChangeReason = priceHistory.ChangeReason,
            IsPriceIncrease = priceHistory.IsPriceIncrease,
            IsPriceDecrease = priceHistory.IsPriceDecrease,
            PriceChangeAmount = priceHistory.PriceChangeAmount,
            CreatedAt = priceHistory.CreatedAt
        };
    }

    private WishlistItemStockHistoryDto MapToStockHistoryDto(WishlistItemStockHistory stockHistory)
    {
        return new WishlistItemStockHistoryDto
        {
            Id = stockHistory.Id,
            WishlistItemId = stockHistory.WishlistItemId,
            OldStockStatus = stockHistory.OldStockStatus,
            NewStockStatus = stockHistory.NewStockStatus,
            OldStockQuantity = stockHistory.OldStockQuantity,
            NewStockQuantity = stockHistory.NewStockQuantity,
            ChangeType = stockHistory.ChangeType,
            ChangeReason = stockHistory.ChangeReason,
            HasStockStatusChanged = stockHistory.HasStockStatusChanged,
            HasQuantityChanged = stockHistory.HasQuantityChanged,
            IsStockIncrease = stockHistory.IsStockIncrease,
            IsStockDecrease = stockHistory.IsStockDecrease,
            CreatedAt = stockHistory.CreatedAt
        };
    }
}
