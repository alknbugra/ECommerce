using ECommerce.Application.DTOs;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Ürün değerlendirme servis interface'i
/// </summary>
public interface IProductReviewService
{
    /// <summary>
    /// Ürün değerlendirmesi oluştur
    /// </summary>
    /// <param name="createReviewDto">Değerlendirme oluşturma DTO'su</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="userIpAddress">Kullanıcı IP adresi</param>
    /// <param name="userAgent">Kullanıcı agent bilgisi</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Oluşturulan değerlendirme</returns>
    Task<ProductReviewDto?> CreateReviewAsync(CreateProductReviewDto createReviewDto, Guid userId, string? userIpAddress = null, string? userAgent = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ürün değerlendirmesi güncelle
    /// </summary>
    /// <param name="reviewId">Değerlendirme ID'si</param>
    /// <param name="updateReviewDto">Güncelleme DTO'su</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncellenen değerlendirme</returns>
    Task<ProductReviewDto?> UpdateReviewAsync(Guid reviewId, CreateProductReviewDto updateReviewDto, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ürün değerlendirmesi sil
    /// </summary>
    /// <param name="reviewId">Değerlendirme ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silme sonucu</returns>
    Task<bool> DeleteReviewAsync(Guid reviewId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ürün değerlendirmesi onayla
    /// </summary>
    /// <param name="reviewId">Değerlendirme ID'si</param>
    /// <param name="approvedByUserId">Onaylayan kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Onay sonucu</returns>
    Task<bool> ApproveReviewAsync(Guid reviewId, Guid approvedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ürün değerlendirmesi reddet
    /// </summary>
    /// <param name="reviewId">Değerlendirme ID'si</param>
    /// <param name="rejectionReason">Red sebebi</param>
    /// <param name="approvedByUserId">Reddeden kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Red sonucu</returns>
    Task<bool> RejectReviewAsync(Guid reviewId, string rejectionReason, Guid approvedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ürün değerlendirmesi al
    /// </summary>
    /// <param name="reviewId">Değerlendirme ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Değerlendirme bilgileri</returns>
    Task<ProductReviewDto?> GetReviewAsync(Guid reviewId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ürün değerlendirmelerini al
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="isApproved">Onaylanmış değerlendirmeler filtresi (opsiyonel)</param>
    /// <param name="rating">Puan filtresi (opsiyonel)</param>
    /// <param name="pageNumber">Sayfa numarası</param>
    /// <param name="pageSize">Sayfa boyutu</param>
    /// <param name="sortBy">Sıralama kriteri</param>
    /// <param name="sortDirection">Sıralama yönü</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Değerlendirme listesi</returns>
    Task<List<ProductReviewDto>> GetProductReviewsAsync(Guid productId, bool? isApproved = null, int? rating = null, int pageNumber = 1, int pageSize = 10, string sortBy = "CreatedAt", string sortDirection = "desc", CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcı değerlendirmelerini al
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="pageNumber">Sayfa numarası</param>
    /// <param name="pageSize">Sayfa boyutu</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kullanıcı değerlendirme listesi</returns>
    Task<List<ProductReviewDto>> GetUserReviewsAsync(Guid userId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bekleyen değerlendirmeleri al
    /// </summary>
    /// <param name="pageNumber">Sayfa numarası</param>
    /// <param name="pageSize">Sayfa boyutu</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Bekleyen değerlendirme listesi</returns>
    Task<List<ProductReviewDto>> GetPendingReviewsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ürün değerlendirme istatistiklerini al
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Değerlendirme istatistikleri</returns>
    Task<ProductReviewStatsDto> GetProductReviewStatsAsync(Guid productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değerlendirme oyu ver
    /// </summary>
    /// <param name="reviewId">Değerlendirme ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="voteType">Oy türü</param>
    /// <param name="userIpAddress">Kullanıcı IP adresi</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Oy verme sonucu</returns>
    Task<bool> VoteReviewAsync(Guid reviewId, Guid userId, string voteType, string? userIpAddress = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değerlendirme oyunu kaldır
    /// </summary>
    /// <param name="reviewId">Değerlendirme ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Oy kaldırma sonucu</returns>
    Task<bool> RemoveVoteAsync(Guid reviewId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değerlendirme yanıtı ekle
    /// </summary>
    /// <param name="reviewId">Değerlendirme ID'si</param>
    /// <param name="content">Yanıt içeriği</param>
    /// <param name="respondedByUserId">Yanıtlayan kullanıcı ID'si</param>
    /// <param name="responseType">Yanıt türü</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Yanıt ekleme sonucu</returns>
    Task<ReviewResponseDto?> AddReviewResponseAsync(Guid reviewId, string content, Guid respondedByUserId, string responseType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değerlendirme yanıtını güncelle
    /// </summary>
    /// <param name="responseId">Yanıt ID'si</param>
    /// <param name="content">Yeni içerik</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncelleme sonucu</returns>
    Task<bool> UpdateReviewResponseAsync(Guid responseId, string content, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değerlendirme yanıtını sil
    /// </summary>
    /// <param name="responseId">Yanıt ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silme sonucu</returns>
    Task<bool> DeleteReviewResponseAsync(Guid responseId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcı ürünü değerlendirebilir mi kontrol et
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Değerlendirme yapabilir mi?</returns>
    Task<bool> CanUserReviewProductAsync(Guid productId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcı ürünü daha önce değerlendirmiş mi kontrol et
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Daha önce değerlendirmiş mi?</returns>
    Task<bool> HasUserReviewedProductAsync(Guid productId, Guid userId, CancellationToken cancellationToken = default);
}
