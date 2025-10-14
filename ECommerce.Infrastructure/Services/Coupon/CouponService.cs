using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Services.Coupon;

/// <summary>
/// Kupon servis implementasyonu
/// </summary>
public class CouponService : ICouponService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CouponService> _logger;

    public CouponService(
        IUnitOfWork unitOfWork,
        ILogger<CouponService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CouponDto?> CreateCouponAsync(CreateCouponDto createCouponDto, Guid? createdByUserId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Kupon oluşturuluyor. Code: {Code}, Name: {Name}", createCouponDto.Code, createCouponDto.Name);

            // Kupon kodu benzersiz mi kontrol et
            var isUnique = await IsCouponCodeUniqueAsync(createCouponDto.Code, null, cancellationToken);
            if (!isUnique)
            {
                _logger.LogWarning("Kupon kodu zaten mevcut. Code: {Code}", createCouponDto.Code);
                return null;
            }

            var coupon = new Domain.Entities.Coupon
            {
                Code = createCouponDto.Code.ToUpper(),
                Name = createCouponDto.Name,
                Description = createCouponDto.Description,
                DiscountType = createCouponDto.DiscountType,
                DiscountValue = createCouponDto.DiscountValue,
                MinimumOrderAmount = createCouponDto.MinimumOrderAmount,
                MaximumDiscountAmount = createCouponDto.MaximumDiscountAmount,
                UsageLimit = createCouponDto.UsageLimit,
                UsageLimitPerUser = createCouponDto.UsageLimitPerUser,
                StartDate = createCouponDto.StartDate,
                EndDate = createCouponDto.EndDate,
                IsActive = true,
                IsForNewUsersOnly = createCouponDto.IsForNewUsersOnly,
                IsForSpecificCategories = createCouponDto.IsForSpecificCategories,
                IsForSpecificProducts = createCouponDto.IsForSpecificProducts,
                IsForSpecificUsers = createCouponDto.IsForSpecificUsers,
                CreatedByUserId = createdByUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Coupons.AddAsync(coupon);
            await _unitOfWork.SaveChangesAsync();

            // Kategori ilişkilerini ekle
            if (createCouponDto.CategoryIds?.Any() == true)
            {
                foreach (var categoryId in createCouponDto.CategoryIds)
                {
                    var couponCategory = new CouponCategory
                    {
                        CouponId = coupon.Id,
                        CategoryId = categoryId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.CouponCategories.AddAsync(couponCategory);
                }
            }

            // Ürün ilişkilerini ekle
            if (createCouponDto.ProductIds?.Any() == true)
            {
                foreach (var productId in createCouponDto.ProductIds)
                {
                    var couponProduct = new CouponProduct
                    {
                        CouponId = coupon.Id,
                        ProductId = productId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.CouponProducts.AddAsync(couponProduct);
                }
            }

            // Kullanıcı ilişkilerini ekle
            if (createCouponDto.UserIds?.Any() == true)
            {
                foreach (var userId in createCouponDto.UserIds)
                {
                    var couponUser = new CouponUser
                    {
                        CouponId = coupon.Id,
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.CouponUsers.AddAsync(couponUser);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kupon başarıyla oluşturuldu. Id: {Id}, Code: {Code}", coupon.Id, coupon.Code);

            return new CouponDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                Name = coupon.Name,
                Description = coupon.Description,
                DiscountType = coupon.DiscountType,
                DiscountValue = coupon.DiscountValue,
                MinimumOrderAmount = coupon.MinimumOrderAmount,
                MaximumDiscountAmount = coupon.MaximumDiscountAmount,
                UsageLimit = coupon.UsageLimit,
                UsageLimitPerUser = coupon.UsageLimitPerUser,
                UsageCount = coupon.UsageCount,
                StartDate = coupon.StartDate,
                EndDate = coupon.EndDate,
                IsActive = coupon.IsActive,
                IsForNewUsersOnly = coupon.IsForNewUsersOnly,
                IsForSpecificCategories = coupon.IsForSpecificCategories,
                IsForSpecificProducts = coupon.IsForSpecificProducts,
                IsForSpecificUsers = coupon.IsForSpecificUsers,
                IsValid = coupon.IsValid,
                IsExpired = coupon.IsExpired,
                IsUsageLimitReached = coupon.IsUsageLimitReached,
                CreatedAt = coupon.CreatedAt,
                UpdatedAt = coupon.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon oluşturma sırasında hata oluştu. Code: {Code}", createCouponDto.Code);
            return null;
        }
    }

    public async Task<CouponDto?> UpdateCouponAsync(Guid couponId, CreateCouponDto updateCouponDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Kupon güncelleniyor. Id: {Id}", couponId);

            var coupon = await _unitOfWork.Coupons.GetByIdAsync(couponId);
            if (coupon == null)
            {
                _logger.LogWarning("Kupon bulunamadı. Id: {Id}", couponId);
                return null;
            }

            // Kupon kodu benzersiz mi kontrol et (kendisi hariç)
            if (coupon.Code != updateCouponDto.Code.ToUpper())
            {
                var isUnique = await IsCouponCodeUniqueAsync(updateCouponDto.Code, couponId, cancellationToken);
                if (!isUnique)
                {
                    _logger.LogWarning("Kupon kodu zaten mevcut. Code: {Code}", updateCouponDto.Code);
                    return null;
                }
            }

            coupon.Code = updateCouponDto.Code.ToUpper();
            coupon.Name = updateCouponDto.Name;
            coupon.Description = updateCouponDto.Description;
            coupon.DiscountType = updateCouponDto.DiscountType;
            coupon.DiscountValue = updateCouponDto.DiscountValue;
            coupon.MinimumOrderAmount = updateCouponDto.MinimumOrderAmount;
            coupon.MaximumDiscountAmount = updateCouponDto.MaximumDiscountAmount;
            coupon.UsageLimit = updateCouponDto.UsageLimit;
            coupon.UsageLimitPerUser = updateCouponDto.UsageLimitPerUser;
            coupon.StartDate = updateCouponDto.StartDate;
            coupon.EndDate = updateCouponDto.EndDate;
            coupon.IsForNewUsersOnly = updateCouponDto.IsForNewUsersOnly;
            coupon.IsForSpecificCategories = updateCouponDto.IsForSpecificCategories;
            coupon.IsForSpecificProducts = updateCouponDto.IsForSpecificProducts;
            coupon.IsForSpecificUsers = updateCouponDto.IsForSpecificUsers;
            coupon.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kupon başarıyla güncellendi. Id: {Id}", couponId);

            return new CouponDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                Name = coupon.Name,
                Description = coupon.Description,
                DiscountType = coupon.DiscountType,
                DiscountValue = coupon.DiscountValue,
                MinimumOrderAmount = coupon.MinimumOrderAmount,
                MaximumDiscountAmount = coupon.MaximumDiscountAmount,
                UsageLimit = coupon.UsageLimit,
                UsageLimitPerUser = coupon.UsageLimitPerUser,
                UsageCount = coupon.UsageCount,
                StartDate = coupon.StartDate,
                EndDate = coupon.EndDate,
                IsActive = coupon.IsActive,
                IsForNewUsersOnly = coupon.IsForNewUsersOnly,
                IsForSpecificCategories = coupon.IsForSpecificCategories,
                IsForSpecificProducts = coupon.IsForSpecificProducts,
                IsForSpecificUsers = coupon.IsForSpecificUsers,
                IsValid = coupon.IsValid,
                IsExpired = coupon.IsExpired,
                IsUsageLimitReached = coupon.IsUsageLimitReached,
                CreatedAt = coupon.CreatedAt,
                UpdatedAt = coupon.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon güncelleme sırasında hata oluştu. Id: {Id}", couponId);
            return null;
        }
    }

    public async Task<bool> DeleteCouponAsync(Guid couponId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Kupon siliniyor. Id: {Id}", couponId);

            var coupon = await _unitOfWork.Coupons.GetByIdAsync(couponId);
            if (coupon == null)
            {
                _logger.LogWarning("Kupon bulunamadı. Id: {Id}", couponId);
                return false;
            }

            coupon.IsDeleted = true;
            coupon.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kupon başarıyla silindi. Id: {Id}", couponId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon silme sırasında hata oluştu. Id: {Id}", couponId);
            return false;
        }
    }

    public async Task<CouponDto?> GetCouponAsync(Guid couponId, CancellationToken cancellationToken = default)
    {
        try
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(couponId);
            if (coupon == null)
                return null;

            return MapToDto(coupon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon bilgileri alınırken hata oluştu. Id: {Id}", couponId);
            return null;
        }
    }

    public async Task<CouponDto?> GetCouponByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        try
        {
            var coupon = await _unitOfWork.Coupons.FirstOrDefaultAsync(
                c => c.Code == code.ToUpper() && !c.IsDeleted);

            if (coupon == null)
                return null;

            return MapToDto(coupon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon kodu ile kupon bilgileri alınırken hata oluştu. Code: {Code}", code);
            return null;
        }
    }

    public async Task<List<CouponDto>> GetAllCouponsAsync(bool? isActive = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Coupons.GetAll().AsQueryable();

            if (isActive.HasValue)
                query = query.Where(c => c.IsActive == isActive.Value);

            var coupons = await query.ToListAsync(cancellationToken);

            return coupons.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tüm kuponlar alınırken hata oluştu");
            return new List<CouponDto>();
        }
    }

    public async Task<List<CouponDto>> GetActiveCouponsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var coupons = await _unitOfWork.Coupons.GetAll()
                .Where(c => c.IsActive && !c.IsDeleted && c.IsValid)
                .ToListAsync(cancellationToken);

            return coupons.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Aktif kuponlar alınırken hata oluştu");
            return new List<CouponDto>();
        }
    }

    public async Task<CouponValidationResultDto> ValidateCouponAsync(ValidateCouponDto validateCouponDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Kupon doğrulanıyor. Code: {Code}, UserId: {UserId}", validateCouponDto.Code, validateCouponDto.UserId);

            var coupon = await _unitOfWork.Coupons.FirstOrDefaultAsync(
                c => c.Code == validateCouponDto.Code.ToUpper() && !c.IsDeleted);

            if (coupon == null)
            {
                return new CouponValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Kupon bulunamadı",
                    ErrorCode = "COUPON_NOT_FOUND"
                };
            }

            // Temel doğrulamalar
            if (!coupon.IsActive)
            {
                return new CouponValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Kupon aktif değil",
                    ErrorCode = "COUPON_INACTIVE"
                };
            }

            if (coupon.IsExpired)
            {
                return new CouponValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Kupon süresi dolmuş",
                    ErrorCode = "COUPON_EXPIRED"
                };
            }

            if (coupon.IsUsageLimitReached)
            {
                return new CouponValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Kupon kullanım limiti dolmuş",
                    ErrorCode = "COUPON_USAGE_LIMIT_REACHED"
                };
            }

            // Minimum sipariş tutarı kontrolü
            if (validateCouponDto.OrderAmount < coupon.MinimumOrderAmount)
            {
                return new CouponValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = $"Minimum sipariş tutarı {coupon.MinimumOrderAmount:C} olmalıdır",
                    ErrorCode = "MINIMUM_ORDER_AMOUNT_NOT_MET"
                };
            }

            // Kullanıcı başına kullanım limiti kontrolü
            if (coupon.UsageLimitPerUser.HasValue)
            {
                var userUsageCount = await _unitOfWork.CouponUsages.GetAll()
                    .CountAsync(cu => cu.CouponId == coupon.Id && cu.UserId == validateCouponDto.UserId, cancellationToken);

                if (userUsageCount >= coupon.UsageLimitPerUser.Value)
                {
                    return new CouponValidationResultDto
                    {
                        IsValid = false,
                        ErrorMessage = "Bu kuponu daha fazla kullanamazsınız",
                        ErrorCode = "USER_USAGE_LIMIT_REACHED"
                    };
                }
            }

            // Yeni kullanıcı kontrolü
            if (coupon.IsForNewUsersOnly)
            {
                var userOrderCount = await _unitOfWork.Orders.GetAll()
                    .CountAsync(o => o.UserId == validateCouponDto.UserId, cancellationToken);

                if (userOrderCount > 0)
                {
                    return new CouponValidationResultDto
                    {
                        IsValid = false,
                        ErrorMessage = "Bu kupon sadece yeni kullanıcılar için geçerlidir",
                        ErrorCode = "NOT_NEW_USER"
                    };
                }
            }

            // Kategori kontrolü
            if (coupon.IsForSpecificCategories)
            {
                var couponCategories = await _unitOfWork.CouponCategories.GetAll()
                    .Where(cc => cc.CouponId == coupon.Id)
                    .Select(cc => cc.CategoryId)
                    .ToListAsync(cancellationToken);

                var orderCategoryIds = validateCouponDto.OrderItems
                    .SelectMany(oi => oi.ProductCategories ?? new List<Guid>())
                    .Distinct()
                    .ToList();

                if (!orderCategoryIds.Any(ocId => couponCategories.Contains(ocId)))
                {
                    return new CouponValidationResultDto
                    {
                        IsValid = false,
                        ErrorMessage = "Bu kupon sepetinizdeki ürünler için geçerli değil",
                        ErrorCode = "CATEGORY_NOT_ELIGIBLE"
                    };
                }
            }

            // Ürün kontrolü
            if (coupon.IsForSpecificProducts)
            {
                var couponProducts = await _unitOfWork.CouponProducts.GetAll()
                    .Where(cp => cp.CouponId == coupon.Id)
                    .Select(cp => cp.ProductId)
                    .ToListAsync(cancellationToken);

                var orderProductIds = validateCouponDto.OrderItems
                    .Select(oi => oi.ProductId)
                    .ToList();

                if (!orderProductIds.Any(opId => couponProducts.Contains(opId)))
                {
                    return new CouponValidationResultDto
                    {
                        IsValid = false,
                        ErrorMessage = "Bu kupon sepetinizdeki ürünler için geçerli değil",
                        ErrorCode = "PRODUCT_NOT_ELIGIBLE"
                    };
                }
            }

            // Kullanıcı kontrolü
            if (coupon.IsForSpecificUsers)
            {
                var isUserEligible = await _unitOfWork.CouponUsers.GetAll()
                    .AnyAsync(cu => cu.CouponId == coupon.Id && cu.UserId == validateCouponDto.UserId, cancellationToken);

                if (!isUserEligible)
                {
                    return new CouponValidationResultDto
                    {
                        IsValid = false,
                        ErrorMessage = "Bu kuponu kullanma yetkiniz yok",
                        ErrorCode = "USER_NOT_ELIGIBLE"
                    };
                }
            }

            // İndirim tutarını hesapla
            var discountAmount = CalculateDiscountAmount(coupon, validateCouponDto.OrderAmount);

            _logger.LogInformation("Kupon doğrulama başarılı. Code: {Code}, DiscountAmount: {DiscountAmount}", validateCouponDto.Code, discountAmount);

            return new CouponValidationResultDto
            {
                IsValid = true,
                Coupon = MapToDto(coupon),
                DiscountAmount = discountAmount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon doğrulama sırasında hata oluştu. Code: {Code}", validateCouponDto.Code);
            return new CouponValidationResultDto
            {
                IsValid = false,
                ErrorMessage = "Kupon doğrulama sırasında bir hata oluştu",
                ErrorCode = "VALIDATION_ERROR"
            };
        }
    }

    public async Task<CouponValidationResultDto> UseCouponAsync(string couponCode, Guid userId, Guid orderId, decimal orderAmount, string? userIpAddress = null, string? userAgent = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Kupon kullanılıyor. Code: {Code}, UserId: {UserId}, OrderId: {OrderId}", couponCode, userId, orderId);

            var validateDto = new ValidateCouponDto
            {
                Code = couponCode,
                UserId = userId,
                OrderAmount = orderAmount,
                OrderItems = new List<OrderItemDto>() // Burada sipariş ürünleri de geçilebilir
            };

            var validationResult = await ValidateCouponAsync(validateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var coupon = await _unitOfWork.Coupons.FirstOrDefaultAsync(
                c => c.Code == couponCode.ToUpper() && !c.IsDeleted);

            if (coupon == null)
            {
                return new CouponValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Kupon bulunamadı",
                    ErrorCode = "COUPON_NOT_FOUND"
                };
            }

            // Kupon kullanım sayısını artır
            coupon.UsageCount++;
            coupon.UpdatedAt = DateTime.UtcNow;

            // Kupon kullanımını kaydet
            var couponUsage = new CouponUsage
            {
                CouponId = coupon.Id,
                UserId = userId,
                OrderId = orderId,
                DiscountAmount = validationResult.DiscountAmount,
                OrderAmount = orderAmount,
                UsedAt = DateTime.UtcNow,
                UserIpAddress = userIpAddress,
                UserAgent = userAgent,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.CouponUsages.AddAsync(couponUsage);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kupon başarıyla kullanıldı. Code: {Code}, DiscountAmount: {DiscountAmount}", couponCode, validationResult.DiscountAmount);

            return new CouponValidationResultDto
            {
                IsValid = true,
                Coupon = MapToDto(coupon),
                DiscountAmount = validationResult.DiscountAmount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon kullanma sırasında hata oluştu. Code: {Code}", couponCode);
            return new CouponValidationResultDto
            {
                IsValid = false,
                ErrorMessage = "Kupon kullanma sırasında bir hata oluştu",
                ErrorCode = "USAGE_ERROR"
            };
        }
    }

    public async Task<List<CouponUsageDto>> GetCouponUsagesAsync(Guid? couponId = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.CouponUsages.GetAll()
                .Include(cu => cu.Coupon)
                .Include(cu => cu.User)
                .Include(cu => cu.Order)
                .AsQueryable();

            if (couponId.HasValue)
                query = query.Where(cu => cu.CouponId == couponId.Value);

            if (userId.HasValue)
                query = query.Where(cu => cu.UserId == userId.Value);

            var usages = await query
                .OrderByDescending(cu => cu.UsedAt)
                .ToListAsync(cancellationToken);

            return usages.Select(usage => new CouponUsageDto
            {
                Id = usage.Id,
                CouponId = usage.CouponId,
                CouponCode = usage.Coupon.Code,
                UserId = usage.UserId,
                UserName = usage.User.FirstName + " " + usage.User.LastName,
                OrderId = usage.OrderId,
                OrderNumber = usage.Order.OrderNumber,
                DiscountAmount = usage.DiscountAmount,
                OrderAmount = usage.OrderAmount,
                UsedAt = usage.UsedAt,
                UserIpAddress = usage.UserIpAddress,
                CreatedAt = usage.CreatedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon kullanımları alınırken hata oluştu");
            return new List<CouponUsageDto>();
        }
    }

    public async Task<CouponStatsDto> GetCouponStatsAsync(Guid couponId, CancellationToken cancellationToken = default)
    {
        try
        {
            var usages = await _unitOfWork.CouponUsages.GetAll()
                .Where(cu => cu.CouponId == couponId)
                .ToListAsync(cancellationToken);

            if (!usages.Any())
            {
                return new CouponStatsDto();
            }

            return new CouponStatsDto
            {
                TotalUsageCount = usages.Count,
                TotalDiscountAmount = usages.Sum(u => u.DiscountAmount),
                UniqueUserCount = usages.Select(u => u.UserId).Distinct().Count(),
                AverageDiscountAmount = usages.Average(u => u.DiscountAmount),
                MaxDiscountAmount = usages.Max(u => u.DiscountAmount),
                MinDiscountAmount = usages.Min(u => u.DiscountAmount),
                LastUsedAt = usages.Max(u => u.UsedAt),
                FirstUsedAt = usages.Min(u => u.UsedAt)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon istatistikleri alınırken hata oluştu. CouponId: {CouponId}", couponId);
            return new CouponStatsDto();
        }
    }

    public async Task<string> GenerateCouponCodeAsync(int length = 8, string? prefix = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var random = new Random();
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code;

            do
            {
                var codeChars = new char[length];
                for (int i = 0; i < length; i++)
                {
                    codeChars[i] = characters[random.Next(characters.Length)];
                }
                code = prefix + new string(codeChars);
            }
            while (!await IsCouponCodeUniqueAsync(code, null, cancellationToken));

            return code;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon kodu oluşturma sırasında hata oluştu");
            return Guid.NewGuid().ToString("N")[..8].ToUpper();
        }
    }

    public async Task<bool> IsCouponCodeUniqueAsync(string code, Guid? excludeCouponId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Coupons.GetAll()
                .Where(c => c.Code == code.ToUpper() && !c.IsDeleted);

            if (excludeCouponId.HasValue)
                query = query.Where(c => c.Id != excludeCouponId.Value);

            return !await query.AnyAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon kodu benzersizlik kontrolü sırasında hata oluştu. Code: {Code}", code);
            return false;
        }
    }

    private CouponDto MapToDto(Domain.Entities.Coupon coupon)
    {
        return new CouponDto
        {
            Id = coupon.Id,
            Code = coupon.Code,
            Name = coupon.Name,
            Description = coupon.Description,
            DiscountType = coupon.DiscountType,
            DiscountValue = coupon.DiscountValue,
            MinimumOrderAmount = coupon.MinimumOrderAmount,
            MaximumDiscountAmount = coupon.MaximumDiscountAmount,
            UsageLimit = coupon.UsageLimit,
            UsageLimitPerUser = coupon.UsageLimitPerUser,
            UsageCount = coupon.UsageCount,
            StartDate = coupon.StartDate,
            EndDate = coupon.EndDate,
            IsActive = coupon.IsActive,
            IsForNewUsersOnly = coupon.IsForNewUsersOnly,
            IsForSpecificCategories = coupon.IsForSpecificCategories,
            IsForSpecificProducts = coupon.IsForSpecificProducts,
            IsForSpecificUsers = coupon.IsForSpecificUsers,
            IsValid = coupon.IsValid,
            IsExpired = coupon.IsExpired,
            IsUsageLimitReached = coupon.IsUsageLimitReached,
            CreatedAt = coupon.CreatedAt,
            UpdatedAt = coupon.UpdatedAt
        };
    }

    private decimal CalculateDiscountAmount(Domain.Entities.Coupon coupon, decimal orderAmount)
    {
        decimal discountAmount = 0;

        switch (coupon.DiscountType)
        {
            case "Percentage":
                discountAmount = orderAmount * (coupon.DiscountValue / 100);
                break;
            case "FixedAmount":
                discountAmount = coupon.DiscountValue;
                break;
            case "FreeShipping":
                // Bu durumda kargo ücreti hesaplanmalı
                discountAmount = 0; // Kargo ücreti ayrı hesaplanır
                break;
        }

        // Maksimum indirim tutarı kontrolü
        if (coupon.MaximumDiscountAmount.HasValue && discountAmount > coupon.MaximumDiscountAmount.Value)
        {
            discountAmount = coupon.MaximumDiscountAmount.Value;
        }

        // Sipariş tutarından fazla indirim yapılamaz
        if (discountAmount > orderAmount)
        {
            discountAmount = orderAmount;
        }

        return discountAmount;
    }
}
