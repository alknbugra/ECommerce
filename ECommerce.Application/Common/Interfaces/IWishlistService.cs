using ECommerce.Application.DTOs;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Favori liste servis interface'i
/// </summary>
public interface IWishlistService
{
    /// <summary>
    /// Favori liste oluştur
    /// </summary>
    /// <param name="createWishlistDto">Favori liste oluşturma DTO'su</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Oluşturulan favori liste</returns>
    Task<WishlistDto?> CreateWishlistAsync(CreateWishlistDto createWishlistDto, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Favori liste güncelle
    /// </summary>
    /// <param name="wishlistId">Favori liste ID'si</param>
    /// <param name="updateWishlistDto">Güncelleme DTO'su</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncellenen favori liste</returns>
    Task<WishlistDto?> UpdateWishlistAsync(Guid wishlistId, CreateWishlistDto updateWishlistDto, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Favori liste sil
    /// </summary>
    /// <param name="wishlistId">Favori liste ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silme sonucu</returns>
    Task<bool> DeleteWishlistAsync(Guid wishlistId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Favori liste getir
    /// </summary>
    /// <param name="wishlistId">Favori liste ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Favori liste bilgileri</returns>
    Task<WishlistDto?> GetWishlistAsync(Guid wishlistId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcı favori listelerini getir
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Favori liste listesi</returns>
    Task<List<WishlistDto>> GetUserWishlistsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Varsayılan favori liste getir
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Varsayılan favori liste</returns>
    Task<WishlistDto?> GetDefaultWishlistAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ürünü favorilere ekle
    /// </summary>
    /// <param name="addToWishlistDto">Favorilere ekleme DTO'su</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Eklenen favori ürün</returns>
    Task<WishlistItemDto?> AddToWishlistAsync(AddToWishlistDto addToWishlistDto, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ürünü favorilerden çıkar
    /// </summary>
    /// <param name="wishlistItemId">Favori ürün ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Çıkarma sonucu</returns>
    Task<bool> RemoveFromWishlistAsync(Guid wishlistItemId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Favori ürün güncelle
    /// </summary>
    /// <param name="wishlistItemId">Favori ürün ID'si</param>
    /// <param name="updateWishlistItemDto">Güncelleme DTO'su</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncellenen favori ürün</returns>
    Task<WishlistItemDto?> UpdateWishlistItemAsync(Guid wishlistItemId, AddToWishlistDto updateWishlistItemDto, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Favori liste paylaş
    /// </summary>
    /// <param name="wishlistId">Favori liste ID'si</param>
    /// <param name="shareType">Paylaşım türü</param>
    /// <param name="emailAddress">E-posta adresi (opsiyonel)</param>
    /// <param name="message">Paylaşım mesajı (opsiyonel)</param>
    /// <param name="expirationDays">Süre (gün) (opsiyonel)</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paylaşım bilgileri</returns>
    Task<WishlistShareDto?> ShareWishlistAsync(Guid wishlistId, string shareType, string? emailAddress = null, string? message = null, int? expirationDays = null, Guid userId = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Paylaşılan favori listeyi getir
    /// </summary>
    /// <param name="shareCode">Paylaşım kodu</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paylaşılan favori liste</returns>
    Task<WishlistDto?> GetSharedWishlistAsync(string shareCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Paylaşımı iptal et
    /// </summary>
    /// <param name="shareId">Paylaşım ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İptal sonucu</returns>
    Task<bool> CancelShareAsync(Guid shareId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcı ürünü favorilerinde var mı kontrol et
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Favorilerde var mı?</returns>
    Task<bool> IsProductInWishlistAsync(Guid productId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ürünün hangi favori listelerinde olduğunu getir
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Favori liste listesi</returns>
    Task<List<WishlistDto>> GetWishlistsContainingProductAsync(Guid productId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fiyat değişikliklerini kontrol et ve bildirim gönder
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kontrol sonucu</returns>
    Task<bool> CheckPriceChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stok değişikliklerini kontrol et ve bildirim gönder
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kontrol sonucu</returns>
    Task<bool> CheckStockChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Favori liste istatistiklerini getir
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İstatistik bilgileri</returns>
    Task<WishlistStatsDto> GetWishlistStatsAsync(Guid userId, CancellationToken cancellationToken = default);
}
