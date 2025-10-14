using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Services.ProductReview;

/// <summary>
/// Ürün değerlendirme servis implementasyonu
/// </summary>
public class ProductReviewService : IProductReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductReviewService> _logger;

    public ProductReviewService(
        IUnitOfWork unitOfWork,
        ILogger<ProductReviewService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ProductReviewDto?> CreateReviewAsync(CreateProductReviewDto createReviewDto, Guid userId, string? userIpAddress = null, string? userAgent = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Ürün değerlendirmesi oluşturuluyor. ProductId: {ProductId}, UserId: {UserId}", createReviewDto.ProductId, userId);

            // Kullanıcı daha önce bu ürünü değerlendirmiş mi kontrol et
            var hasReviewed = await HasUserReviewedProductAsync(createReviewDto.ProductId, userId, cancellationToken);
            if (hasReviewed)
            {
                _logger.LogWarning("Kullanıcı daha önce bu ürünü değerlendirmiş. ProductId: {ProductId}, UserId: {UserId}", createReviewDto.ProductId, userId);
                return null;
            }

            // Kullanıcı bu ürünü değerlendirebilir mi kontrol et
            var canReview = await CanUserReviewProductAsync(createReviewDto.ProductId, userId, cancellationToken);
            if (!canReview && createReviewDto.ReviewType == "Verified")
            {
                _logger.LogWarning("Kullanıcı bu ürünü değerlendiremez. ProductId: {ProductId}, UserId: {UserId}", createReviewDto.ProductId, userId);
                return null;
            }

            var review = new Domain.Entities.ProductReview
            {
                ProductId = createReviewDto.ProductId,
                UserId = userId,
                OrderId = createReviewDto.OrderId,
                OrderItemId = createReviewDto.OrderItemId,
                Rating = createReviewDto.Rating,
                Title = createReviewDto.Title,
                Content = createReviewDto.Content,
                ReviewType = createReviewDto.ReviewType,
                UserIpAddress = userIpAddress,
                UserAgent = userAgent,
                IsApproved = createReviewDto.ReviewType == "Verified", // Doğrulanmış değerlendirmeler otomatik onaylanır
                IsRejected = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ProductReviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            // Resimleri ekle
            if (createReviewDto.ImageFiles?.Any() == true)
            {
                for (int i = 0; i < createReviewDto.ImageFiles.Count; i++)
                {
                    var imageFile = createReviewDto.ImageFiles[i];
                    // Burada resim upload işlemi yapılacak
                    // Şimdilik sadece placeholder
                    var reviewImage = new ReviewImage
                    {
                        ReviewId = review.Id,
                        FileName = $"review_image_{i + 1}.jpg",
                        ImageUrl = imageFile, // Bu gerçek URL olacak
                        FileSize = 0, // Gerçek boyut hesaplanacak
                        ContentType = "image/jpeg",
                        SortOrder = i,
                        IsApproved = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.ReviewImages.AddAsync(reviewImage);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Ürün değerlendirmesi başarıyla oluşturuldu. Id: {Id}, ProductId: {ProductId}", review.Id, createReviewDto.ProductId);

            return await GetReviewAsync(review.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmesi oluşturma sırasında hata oluştu. ProductId: {ProductId}", createReviewDto.ProductId);
            return null;
        }
    }

    public async Task<ProductReviewDto?> UpdateReviewAsync(Guid reviewId, CreateProductReviewDto updateReviewDto, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Ürün değerlendirmesi güncelleniyor. ReviewId: {ReviewId}, UserId: {UserId}", reviewId, userId);

            var review = await _unitOfWork.ProductReviews.GetByIdAsync(reviewId);
            if (review == null || review.UserId != userId)
            {
                _logger.LogWarning("Değerlendirme bulunamadı veya kullanıcı yetkisi yok. ReviewId: {ReviewId}, UserId: {UserId}", reviewId, userId);
                return null;
            }

            review.Rating = updateReviewDto.Rating;
            review.Title = updateReviewDto.Title;
            review.Content = updateReviewDto.Content;
            review.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Ürün değerlendirmesi başarıyla güncellendi. ReviewId: {ReviewId}", reviewId);

            return await GetReviewAsync(reviewId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmesi güncelleme sırasında hata oluştu. ReviewId: {ReviewId}", reviewId);
            return null;
        }
    }

    public async Task<bool> DeleteReviewAsync(Guid reviewId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Ürün değerlendirmesi siliniyor. ReviewId: {ReviewId}, UserId: {UserId}", reviewId, userId);

            var review = await _unitOfWork.ProductReviews.GetByIdAsync(reviewId);
            if (review == null || review.UserId != userId)
            {
                _logger.LogWarning("Değerlendirme bulunamadı veya kullanıcı yetkisi yok. ReviewId: {ReviewId}, UserId: {UserId}", reviewId, userId);
                return false;
            }

            review.IsDeleted = true;
            review.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Ürün değerlendirmesi başarıyla silindi. ReviewId: {ReviewId}", reviewId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmesi silme sırasında hata oluştu. ReviewId: {ReviewId}", reviewId);
            return false;
        }
    }

    public async Task<bool> ApproveReviewAsync(Guid reviewId, Guid approvedByUserId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Ürün değerlendirmesi onaylanıyor. ReviewId: {ReviewId}, ApprovedByUserId: {ApprovedByUserId}", reviewId, approvedByUserId);

            var review = await _unitOfWork.ProductReviews.GetByIdAsync(reviewId);
            if (review == null)
            {
                _logger.LogWarning("Değerlendirme bulunamadı. ReviewId: {ReviewId}", reviewId);
                return false;
            }

            review.IsApproved = true;
            review.IsRejected = false;
            review.ApprovedByUserId = approvedByUserId;
            review.ApprovedAt = DateTime.UtcNow;
            review.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Ürün değerlendirmesi başarıyla onaylandı. ReviewId: {ReviewId}", reviewId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmesi onaylama sırasında hata oluştu. ReviewId: {ReviewId}", reviewId);
            return false;
        }
    }

    public async Task<bool> RejectReviewAsync(Guid reviewId, string rejectionReason, Guid approvedByUserId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Ürün değerlendirmesi reddediliyor. ReviewId: {ReviewId}, ApprovedByUserId: {ApprovedByUserId}", reviewId, approvedByUserId);

            var review = await _unitOfWork.ProductReviews.GetByIdAsync(reviewId);
            if (review == null)
            {
                _logger.LogWarning("Değerlendirme bulunamadı. ReviewId: {ReviewId}", reviewId);
                return false;
            }

            review.IsApproved = false;
            review.IsRejected = true;
            review.RejectionReason = rejectionReason;
            review.ApprovedByUserId = approvedByUserId;
            review.ApprovedAt = DateTime.UtcNow;
            review.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Ürün değerlendirmesi başarıyla reddedildi. ReviewId: {ReviewId}", reviewId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmesi reddetme sırasında hata oluştu. ReviewId: {ReviewId}", reviewId);
            return false;
        }
    }

    public async Task<ProductReviewDto?> GetReviewAsync(Guid reviewId, CancellationToken cancellationToken = default)
    {
        try
        {
            var review = await _unitOfWork.ProductReviews.GetAll()
                .Include(r => r.Product)
                .Include(r => r.User)
                .Include(r => r.Order)
                .Include(r => r.ApprovedByUser)
                .Include(r => r.ReviewResponses)
                    .ThenInclude(rr => rr.RespondedByUser)
                .Include(r => r.ReviewImages)
                .FirstOrDefaultAsync(r => r.Id == reviewId, cancellationToken);

            if (review == null)
                return null;

            return MapToDto(review);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmesi bilgileri alınırken hata oluştu. ReviewId: {ReviewId}", reviewId);
            return null;
        }
    }

    public async Task<List<ProductReviewDto>> GetProductReviewsAsync(Guid productId, bool? isApproved = null, int? rating = null, int pageNumber = 1, int pageSize = 10, string sortBy = "CreatedAt", string sortDirection = "desc", CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.ProductReviews.GetAll()
                .Include(r => r.Product)
                .Include(r => r.User)
                .Include(r => r.Order)
                .Include(r => r.ApprovedByUser)
                .Include(r => r.ReviewResponses)
                    .ThenInclude(rr => rr.RespondedByUser)
                .Include(r => r.ReviewImages)
                .Where(r => r.ProductId == productId && !r.IsDeleted)
                .AsQueryable();

            if (isApproved.HasValue)
                query = query.Where(r => r.IsApproved == isApproved.Value);

            if (rating.HasValue)
                query = query.Where(r => r.Rating == rating.Value);

            // Sıralama
            query = sortBy.ToLower() switch
            {
                "rating" => sortDirection.ToLower() == "asc" ? query.OrderBy(r => r.Rating) : query.OrderByDescending(r => r.Rating),
                "helpful" => sortDirection.ToLower() == "asc" ? query.OrderBy(r => r.HelpfulCount) : query.OrderByDescending(r => r.HelpfulCount),
                "createdat" => sortDirection.ToLower() == "asc" ? query.OrderBy(r => r.CreatedAt) : query.OrderByDescending(r => r.CreatedAt),
                _ => query.OrderByDescending(r => r.CreatedAt)
            };

            var reviews = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return reviews.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmeleri alınırken hata oluştu. ProductId: {ProductId}", productId);
            return new List<ProductReviewDto>();
        }
    }

    public async Task<List<ProductReviewDto>> GetUserReviewsAsync(Guid userId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var reviews = await _unitOfWork.ProductReviews.GetAll()
                .Include(r => r.Product)
                .Include(r => r.User)
                .Include(r => r.Order)
                .Include(r => r.ApprovedByUser)
                .Include(r => r.ReviewResponses)
                    .ThenInclude(rr => rr.RespondedByUser)
                .Include(r => r.ReviewImages)
                .Where(r => r.UserId == userId && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return reviews.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı değerlendirmeleri alınırken hata oluştu. UserId: {UserId}", userId);
            return new List<ProductReviewDto>();
        }
    }

    public async Task<List<ProductReviewDto>> GetPendingReviewsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var reviews = await _unitOfWork.ProductReviews.GetAll()
                .Include(r => r.Product)
                .Include(r => r.User)
                .Include(r => r.Order)
                .Include(r => r.ApprovedByUser)
                .Include(r => r.ReviewResponses)
                    .ThenInclude(rr => rr.RespondedByUser)
                .Include(r => r.ReviewImages)
                .Where(r => !r.IsApproved && !r.IsRejected && !r.IsDeleted)
                .OrderBy(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return reviews.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bekleyen değerlendirmeler alınırken hata oluştu");
            return new List<ProductReviewDto>();
        }
    }

    public async Task<ProductReviewStatsDto> GetProductReviewStatsAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        try
        {
            var reviews = await _unitOfWork.ProductReviews.GetAll()
                .Where(r => r.ProductId == productId && r.IsApproved && !r.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!reviews.Any())
            {
                return new ProductReviewStatsDto { ProductId = productId };
            }

            var stats = new ProductReviewStatsDto
            {
                ProductId = productId,
                TotalReviews = reviews.Count,
                ApprovedReviews = reviews.Count(r => r.IsApproved),
                PendingReviews = await _unitOfWork.ProductReviews.GetAll()
                    .CountAsync(r => r.ProductId == productId && !r.IsApproved && !r.IsRejected && !r.IsDeleted, cancellationToken),
                AverageRating = (decimal)reviews.Average(r => r.Rating),
                FiveStarCount = reviews.Count(r => r.Rating == 5),
                FourStarCount = reviews.Count(r => r.Rating == 4),
                ThreeStarCount = reviews.Count(r => r.Rating == 3),
                TwoStarCount = reviews.Count(r => r.Rating == 2),
                OneStarCount = reviews.Count(r => r.Rating == 1),
                LastReviewDate = reviews.Max(r => r.CreatedAt),
                FirstReviewDate = reviews.Min(r => r.CreatedAt)
            };

            // Puan dağılımı yüzdesi
            var totalCount = stats.TotalReviews;
            if (totalCount > 0)
            {
                stats.RatingDistribution[5] = Math.Round((decimal)stats.FiveStarCount / totalCount * 100, 2);
                stats.RatingDistribution[4] = Math.Round((decimal)stats.FourStarCount / totalCount * 100, 2);
                stats.RatingDistribution[3] = Math.Round((decimal)stats.ThreeStarCount / totalCount * 100, 2);
                stats.RatingDistribution[2] = Math.Round((decimal)stats.TwoStarCount / totalCount * 100, 2);
                stats.RatingDistribution[1] = Math.Round((decimal)stats.OneStarCount / totalCount * 100, 2);
            }

            return stats;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirme istatistikleri alınırken hata oluştu. ProductId: {ProductId}", productId);
            return new ProductReviewStatsDto { ProductId = productId };
        }
    }

    public async Task<bool> VoteReviewAsync(Guid reviewId, Guid userId, string voteType, string? userIpAddress = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Değerlendirme oyu veriliyor. ReviewId: {ReviewId}, UserId: {UserId}, VoteType: {VoteType}", reviewId, userId, voteType);

            // Kullanıcı daha önce oy vermiş mi kontrol et
            var existingVote = await _unitOfWork.ReviewVotes.GetAll()
                .FirstOrDefaultAsync(v => v.ReviewId == reviewId && v.UserId == userId, cancellationToken);

            if (existingVote != null)
            {
                // Mevcut oyu güncelle
                existingVote.VoteType = voteType;
                existingVote.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                // Yeni oy ekle
                var vote = new ReviewVote
                {
                    ReviewId = reviewId,
                    UserId = userId,
                    VoteType = voteType,
                    UserIpAddress = userIpAddress,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _unitOfWork.ReviewVotes.AddAsync(vote);
            }

            // Değerlendirme oy sayılarını güncelle
            var review = await _unitOfWork.ProductReviews.GetByIdAsync(reviewId);
            if (review != null)
            {
                var helpfulCount = await _unitOfWork.ReviewVotes.GetAll()
                    .CountAsync(v => v.ReviewId == reviewId && v.VoteType == "Helpful", cancellationToken);
                var notHelpfulCount = await _unitOfWork.ReviewVotes.GetAll()
                    .CountAsync(v => v.ReviewId == reviewId && v.VoteType == "NotHelpful", cancellationToken);

                review.HelpfulCount = helpfulCount;
                review.NotHelpfulCount = notHelpfulCount;
                review.UpdatedAt = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Değerlendirme oyu başarıyla verildi. ReviewId: {ReviewId}, UserId: {UserId}", reviewId, userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Değerlendirme oyu verme sırasında hata oluştu. ReviewId: {ReviewId}", reviewId);
            return false;
        }
    }

    public async Task<bool> RemoveVoteAsync(Guid reviewId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Değerlendirme oyu kaldırılıyor. ReviewId: {ReviewId}, UserId: {UserId}", reviewId, userId);

            var vote = await _unitOfWork.ReviewVotes.GetAll()
                .FirstOrDefaultAsync(v => v.ReviewId == reviewId && v.UserId == userId, cancellationToken);

            if (vote != null)
            {
                vote.IsDeleted = true;
                vote.UpdatedAt = DateTime.UtcNow;

                // Değerlendirme oy sayılarını güncelle
                var review = await _unitOfWork.ProductReviews.GetByIdAsync(reviewId);
                if (review != null)
                {
                    var helpfulCount = await _unitOfWork.ReviewVotes.GetAll()
                        .CountAsync(v => v.ReviewId == reviewId && v.VoteType == "Helpful" && !v.IsDeleted, cancellationToken);
                    var notHelpfulCount = await _unitOfWork.ReviewVotes.GetAll()
                        .CountAsync(v => v.ReviewId == reviewId && v.VoteType == "NotHelpful" && !v.IsDeleted, cancellationToken);

                    review.HelpfulCount = helpfulCount;
                    review.NotHelpfulCount = notHelpfulCount;
                    review.UpdatedAt = DateTime.UtcNow;
                }

                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Değerlendirme oyu başarıyla kaldırıldı. ReviewId: {ReviewId}, UserId: {UserId}", reviewId, userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Değerlendirme oyu kaldırma sırasında hata oluştu. ReviewId: {ReviewId}", reviewId);
            return false;
        }
    }

    public async Task<ReviewResponseDto?> AddReviewResponseAsync(Guid reviewId, string content, Guid respondedByUserId, string responseType, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Değerlendirme yanıtı ekleniyor. ReviewId: {ReviewId}, RespondedByUserId: {RespondedByUserId}", reviewId, respondedByUserId);

            var response = new ReviewResponse
            {
                ReviewId = reviewId,
                RespondedByUserId = respondedByUserId,
                Content = content,
                ResponseType = responseType,
                IsApproved = responseType == "Seller", // Satıcı yanıtları otomatik onaylanır
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ReviewResponses.AddAsync(response);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Değerlendirme yanıtı başarıyla eklendi. ResponseId: {ResponseId}", response.Id);

            return new ReviewResponseDto
            {
                Id = response.Id,
                ReviewId = response.ReviewId,
                RespondedByUserId = response.RespondedByUserId,
                Content = response.Content,
                ResponseType = response.ResponseType,
                IsApproved = response.IsApproved,
                CreatedAt = response.CreatedAt,
                UpdatedAt = response.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Değerlendirme yanıtı ekleme sırasında hata oluştu. ReviewId: {ReviewId}", reviewId);
            return null;
        }
    }

    public async Task<bool> UpdateReviewResponseAsync(Guid responseId, string content, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Değerlendirme yanıtı güncelleniyor. ResponseId: {ResponseId}, UserId: {UserId}", responseId, userId);

            var response = await _unitOfWork.ReviewResponses.GetByIdAsync(responseId);
            if (response == null || response.RespondedByUserId != userId)
            {
                _logger.LogWarning("Yanıt bulunamadı veya kullanıcı yetkisi yok. ResponseId: {ResponseId}, UserId: {UserId}", responseId, userId);
                return false;
            }

            response.Content = content;
            response.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Değerlendirme yanıtı başarıyla güncellendi. ResponseId: {ResponseId}", responseId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Değerlendirme yanıtı güncelleme sırasında hata oluştu. ResponseId: {ResponseId}", responseId);
            return false;
        }
    }

    public async Task<bool> DeleteReviewResponseAsync(Guid responseId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Değerlendirme yanıtı siliniyor. ResponseId: {ResponseId}, UserId: {UserId}", responseId, userId);

            var response = await _unitOfWork.ReviewResponses.GetByIdAsync(responseId);
            if (response == null || response.RespondedByUserId != userId)
            {
                _logger.LogWarning("Yanıt bulunamadı veya kullanıcı yetkisi yok. ResponseId: {ResponseId}, UserId: {UserId}", responseId, userId);
                return false;
            }

            response.IsDeleted = true;
            response.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Değerlendirme yanıtı başarıyla silindi. ResponseId: {ResponseId}", responseId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Değerlendirme yanıtı silme sırasında hata oluştu. ResponseId: {ResponseId}", responseId);
            return false;
        }
    }

    public async Task<bool> CanUserReviewProductAsync(Guid productId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Kullanıcının bu ürünü satın alıp almadığını kontrol et
            var hasOrdered = await _unitOfWork.OrderItems.GetAll()
                .Include(oi => oi.Order)
                .AnyAsync(oi => oi.ProductId == productId && 
                               oi.Order.UserId == userId && 
                               oi.Order.Status == "Delivered", 
                               cancellationToken);

            return hasOrdered;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı ürün değerlendirme yetkisi kontrol edilirken hata oluştu. ProductId: {ProductId}, UserId: {UserId}", productId, userId);
            return false;
        }
    }

    public async Task<bool> HasUserReviewedProductAsync(Guid productId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _unitOfWork.ProductReviews.GetAll()
                .AnyAsync(r => r.ProductId == productId && r.UserId == userId && !r.IsDeleted, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı ürün değerlendirme kontrolü sırasında hata oluştu. ProductId: {ProductId}, UserId: {UserId}", productId, userId);
            return false;
        }
    }

    private ProductReviewDto MapToDto(Domain.Entities.ProductReview review)
    {
        return new ProductReviewDto
        {
            Id = review.Id,
            ProductId = review.ProductId,
            ProductName = review.Product.Name,
            UserId = review.UserId,
            UserName = review.User.FirstName + " " + review.User.LastName,
            OrderId = review.OrderId,
            OrderNumber = review.Order?.OrderNumber,
            Rating = review.Rating,
            Title = review.Title,
            Content = review.Content,
            IsApproved = review.IsApproved,
            IsRejected = review.IsRejected,
            RejectionReason = review.RejectionReason,
            ApprovedByUserId = review.ApprovedByUserId,
            ApprovedByUserName = review.ApprovedByUser?.FirstName + " " + review.ApprovedByUser?.LastName,
            ApprovedAt = review.ApprovedAt,
            HelpfulCount = review.HelpfulCount,
            NotHelpfulCount = review.NotHelpfulCount,
            NetHelpfulScore = review.NetHelpfulScore,
            ReviewType = review.ReviewType,
            IsActive = review.IsActive,
            IsPending = review.IsPending,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt,
            ReviewResponses = review.ReviewResponses?.Select(rr => new ReviewResponseDto
            {
                Id = rr.Id,
                ReviewId = rr.ReviewId,
                RespondedByUserId = rr.RespondedByUserId,
                RespondedByUserName = rr.RespondedByUser.FirstName + " " + rr.RespondedByUser.LastName,
                Content = rr.Content,
                ResponseType = rr.ResponseType,
                IsApproved = rr.IsApproved,
                ApprovedByUserId = rr.ApprovedByUserId,
                ApprovedByUserName = rr.ApprovedByUser?.FirstName + " " + rr.ApprovedByUser?.LastName,
                ApprovedAt = rr.ApprovedAt,
                CreatedAt = rr.CreatedAt,
                UpdatedAt = rr.UpdatedAt
            }).ToList(),
            ReviewImages = review.ReviewImages?.Select(ri => new ReviewImageDto
            {
                Id = ri.Id,
                ReviewId = ri.ReviewId,
                FileName = ri.FileName,
                ImageUrl = ri.ImageUrl,
                FileSize = ri.FileSize,
                ContentType = ri.ContentType,
                SortOrder = ri.SortOrder,
                IsApproved = ri.IsApproved,
                CreatedAt = ri.CreatedAt
            }).ToList()
        };
    }
}
