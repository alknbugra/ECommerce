using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Interfaces;

/// <summary>
/// Kargo repository interface
/// </summary>
public interface ICargoRepository : IRepository<Cargo>
{
    /// <summary>
    /// Takip numarasına göre kargo getir
    /// </summary>
    /// <param name="trackingNumber">Takip numarası</param>
    /// <returns>Kargo veya null</returns>
    Task<Cargo?> GetByTrackingNumberAsync(string trackingNumber);

    /// <summary>
    /// Sipariş ID'sine göre kargo getir
    /// </summary>
    /// <param name="orderId">Sipariş ID'si</param>
    /// <returns>Kargo veya null</returns>
    Task<Cargo?> GetByOrderIdAsync(Guid orderId);

    /// <summary>
    /// Kargo şirketi ID'sine göre kargoları getir
    /// </summary>
    /// <param name="cargoCompanyId">Kargo şirketi ID'si</param>
    /// <returns>Kargo listesi</returns>
    Task<IEnumerable<Cargo>> GetByCargoCompanyIdAsync(Guid cargoCompanyId);

    /// <summary>
    /// Kargo durumuna göre kargoları getir
    /// </summary>
    /// <param name="status">Kargo durumu</param>
    /// <returns>Kargo listesi</returns>
    Task<IEnumerable<Cargo>> GetByStatusAsync(CargoStatus status);

    /// <summary>
    /// Kullanıcı ID'sine göre kargoları getir
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <returns>Kargo listesi</returns>
    Task<IEnumerable<Cargo>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Tarih aralığına göre kargoları getir
    /// </summary>
    /// <param name="startDate">Başlangıç tarihi</param>
    /// <param name="endDate">Bitiş tarihi</param>
    /// <returns>Kargo listesi</returns>
    Task<IEnumerable<Cargo>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Kargo durumunu güncelle
    /// </summary>
    /// <param name="cargoId">Kargo ID'si</param>
    /// <param name="status">Yeni durum</param>
    /// <param name="notes">Notlar</param>
    /// <returns>Güncellenen kargo</returns>
    Task<Cargo?> UpdateStatusAsync(Guid cargoId, CargoStatus status, string? notes = null);

    /// <summary>
    /// Kargo takip numarası var mı kontrol et
    /// </summary>
    /// <param name="trackingNumber">Takip numarası</param>
    /// <returns>Var mı?</returns>
    Task<bool> TrackingNumberExistsAsync(string trackingNumber);

    /// <summary>
    /// Kargo istatistiklerini getir
    /// </summary>
    /// <param name="startDate">Başlangıç tarihi</param>
    /// <param name="endDate">Bitiş tarihi</param>
    /// <returns>Kargo istatistikleri</returns>
    Task<CargoStatistics> GetStatisticsAsync(DateTime startDate, DateTime endDate);
}

/// <summary>
/// Kargo istatistikleri
/// </summary>
public class CargoStatistics
{
    /// <summary>
    /// Toplam kargo sayısı
    /// </summary>
    public int TotalCargos { get; set; }

    /// <summary>
    /// Duruma göre kargo sayıları
    /// </summary>
    public Dictionary<CargoStatus, int> StatusCounts { get; set; } = new();

    /// <summary>
    /// Kargo şirketine göre kargo sayıları
    /// </summary>
    public Dictionary<string, int> CompanyCounts { get; set; } = new();

    /// <summary>
    /// Toplam kargo ücreti
    /// </summary>
    public decimal TotalShippingCost { get; set; }

    /// <summary>
    /// Ortalama kargo süresi (gün)
    /// </summary>
    public double AverageDeliveryDays { get; set; }
}
