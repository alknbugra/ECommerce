using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Interfaces;

/// <summary>
/// Kargo takip repository interface
/// </summary>
public interface ICargoTrackingRepository : IRepository<CargoTracking>
{
    /// <summary>
    /// Kargo ID'sine göre takip geçmişini getir
    /// </summary>
    /// <param name="cargoId">Kargo ID'si</param>
    /// <returns>Takip geçmişi listesi</returns>
    Task<IEnumerable<CargoTracking>> GetByCargoIdAsync(Guid cargoId);

    /// <summary>
    /// Kargo ID'sine göre güncel takip durumunu getir
    /// </summary>
    /// <param name="cargoId">Kargo ID'si</param>
    /// <returns>Güncel takip durumu veya null</returns>
    Task<CargoTracking?> GetCurrentStatusAsync(Guid cargoId);

    /// <summary>
    /// Kargo durumuna göre takip kayıtlarını getir
    /// </summary>
    /// <param name="status">Kargo durumu</param>
    /// <returns>Takip kayıtları listesi</returns>
    Task<IEnumerable<CargoTracking>> GetByStatusAsync(CargoStatus status);

    /// <summary>
    /// Tarih aralığına göre takip kayıtlarını getir
    /// </summary>
    /// <param name="startDate">Başlangıç tarihi</param>
    /// <param name="endDate">Bitiş tarihi</param>
    /// <returns>Takip kayıtları listesi</returns>
    Task<IEnumerable<CargoTracking>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Takip kaynağına göre takip kayıtlarını getir
    /// </summary>
    /// <param name="source">Takip kaynağı</param>
    /// <returns>Takip kayıtları listesi</returns>
    Task<IEnumerable<CargoTracking>> GetBySourceAsync(string source);

    /// <summary>
    /// Kargo için yeni takip durumu ekle
    /// </summary>
    /// <param name="cargoId">Kargo ID'si</param>
    /// <param name="status">Yeni durum</param>
    /// <param name="statusDescription">Durum açıklaması</param>
    /// <param name="location">Lokasyon</param>
    /// <param name="notes">Notlar</param>
    /// <param name="source">Kaynak</param>
    /// <param name="trackingData">Takip verisi</param>
    /// <returns>Eklenen takip kaydı</returns>
    Task<CargoTracking> AddTrackingStatusAsync(
        Guid cargoId,
        CargoStatus status,
        string statusDescription,
        string? location = null,
        string? notes = null,
        string? source = null,
        string? trackingData = null);

    /// <summary>
    /// Kargo için güncel takip durumunu güncelle
    /// </summary>
    /// <param name="cargoId">Kargo ID'si</param>
    /// <param name="status">Yeni durum</param>
    /// <param name="statusDescription">Durum açıklaması</param>
    /// <param name="location">Lokasyon</param>
    /// <param name="notes">Notlar</param>
    /// <param name="source">Kaynak</param>
    /// <param name="trackingData">Takip verisi</param>
    /// <returns>Güncellenen takip kaydı</returns>
    Task<CargoTracking?> UpdateCurrentStatusAsync(
        Guid cargoId,
        CargoStatus status,
        string statusDescription,
        string? location = null,
        string? notes = null,
        string? source = null,
        string? trackingData = null);

    /// <summary>
    /// Kargo takip geçmişini temizle (eski kayıtları sil)
    /// </summary>
    /// <param name="cargoId">Kargo ID'si</param>
    /// <param name="keepLastDays">Son kaç günlük kayıtları tut</param>
    /// <returns>Silinen kayıt sayısı</returns>
    Task<int> CleanupOldTrackingRecordsAsync(Guid cargoId, int keepLastDays = 30);
}
