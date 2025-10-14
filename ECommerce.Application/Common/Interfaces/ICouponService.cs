using ECommerce.Application.DTOs;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Kupon servis interface'i
/// </summary>
public interface ICouponService
{
    /// <summary>
    /// Kupon oluştur
    /// </summary>
    /// <param name="createCouponDto">Kupon oluşturma DTO'su</param>
    /// <param name="createdByUserId">Oluşturan kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Oluşturulan kupon</returns>
    Task<CouponDto?> CreateCouponAsync(CreateCouponDto createCouponDto, Guid? createdByUserId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kupon güncelle
    /// </summary>
    /// <param name="couponId">Kupon ID'si</param>
    /// <param name="updateCouponDto">Kupon güncelleme DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncellenen kupon</returns>
    Task<CouponDto?> UpdateCouponAsync(Guid couponId, CreateCouponDto updateCouponDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kupon sil
    /// </summary>
    /// <param name="couponId">Kupon ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silme sonucu</returns>
    Task<bool> DeleteCouponAsync(Guid couponId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kupon al
    /// </summary>
    /// <param name="couponId">Kupon ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kupon bilgileri</returns>
    Task<CouponDto?> GetCouponAsync(Guid couponId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kupon kodu ile kupon al
    /// </summary>
    /// <param name="code">Kupon kodu</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kupon bilgileri</returns>
    Task<CouponDto?> GetCouponByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tüm kuponları al
    /// </summary>
    /// <param name="isActive">Aktif kuponlar filtresi (opsiyonel)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kupon listesi</returns>
    Task<List<CouponDto>> GetAllCouponsAsync(bool? isActive = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aktif kuponları al
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Aktif kupon listesi</returns>
    Task<List<CouponDto>> GetActiveCouponsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Kupon doğrula
    /// </summary>
    /// <param name="validateCouponDto">Kupon doğrulama DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Doğrulama sonucu</returns>
    Task<CouponValidationResultDto> ValidateCouponAsync(ValidateCouponDto validateCouponDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kupon kullan
    /// </summary>
    /// <param name="couponCode">Kupon kodu</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="orderId">Sipariş ID'si</param>
    /// <param name="orderAmount">Sipariş tutarı</param>
    /// <param name="userIpAddress">Kullanıcı IP adresi</param>
    /// <param name="userAgent">Kullanıcı agent bilgisi</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kullanım sonucu</returns>
    Task<CouponValidationResultDto> UseCouponAsync(string couponCode, Guid userId, Guid orderId, decimal orderAmount, string? userIpAddress = null, string? userAgent = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kupon kullanımlarını al
    /// </summary>
    /// <param name="couponId">Kupon ID'si (opsiyonel)</param>
    /// <param name="userId">Kullanıcı ID'si (opsiyonel)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kupon kullanım listesi</returns>
    Task<List<CouponUsageDto>> GetCouponUsagesAsync(Guid? couponId = null, Guid? userId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kupon istatistiklerini al
    /// </summary>
    /// <param name="couponId">Kupon ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kupon istatistikleri</returns>
    Task<CouponStatsDto> GetCouponStatsAsync(Guid couponId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kupon kodu oluştur
    /// </summary>
    /// <param name="length">Kod uzunluğu</param>
    /// <param name="prefix">Ön ek (opsiyonel)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Oluşturulan kupon kodu</returns>
    Task<string> GenerateCouponCodeAsync(int length = 8, string? prefix = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kupon kodu benzersiz mi kontrol et
    /// </summary>
    /// <param name="code">Kupon kodu</param>
    /// <param name="excludeCouponId">Hariç tutulacak kupon ID'si (opsiyonel)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Benzersiz mi?</returns>
    Task<bool> IsCouponCodeUniqueAsync(string code, Guid? excludeCouponId = null, CancellationToken cancellationToken = default);
}

/// <summary>
/// Kupon istatistikleri DTO'su
/// </summary>
public class CouponStatsDto
{
    /// <summary>
    /// Toplam kullanım sayısı
    /// </summary>
    public int TotalUsageCount { get; set; }

    /// <summary>
    /// Toplam indirim tutarı
    /// </summary>
    public decimal TotalDiscountAmount { get; set; }

    /// <summary>
    /// Benzersiz kullanıcı sayısı
    /// </summary>
    public int UniqueUserCount { get; set; }

    /// <summary>
    /// Ortalama indirim tutarı
    /// </summary>
    public decimal AverageDiscountAmount { get; set; }

    /// <summary>
    /// En yüksek indirim tutarı
    /// </summary>
    public decimal MaxDiscountAmount { get; set; }

    /// <summary>
    /// En düşük indirim tutarı
    /// </summary>
    public decimal MinDiscountAmount { get; set; }

    /// <summary>
    /// Son kullanım tarihi
    /// </summary>
    public DateTime? LastUsedAt { get; set; }

    /// <summary>
    /// İlk kullanım tarihi
    /// </summary>
    public DateTime? FirstUsedAt { get; set; }
}
