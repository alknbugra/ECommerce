using ECommerce.Application.DTOs;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Kargo takip servis interface
/// </summary>
public interface ICargoTrackingService
{
    /// <summary>
    /// Kargo takip geçmişini getir
    /// </summary>
    /// <param name="cargoId">Kargo ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Takip geçmişi DTO listesi</returns>
    Task<IEnumerable<CargoTrackingDto>> GetTrackingHistoryAsync(Guid cargoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kargo güncel durumunu getir
    /// </summary>
    /// <param name="cargoId">Kargo ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncel takip durumu DTO'su veya null</returns>
    Task<CargoTrackingDto?> GetCurrentStatusAsync(Guid cargoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni takip durumu ekle
    /// </summary>
    /// <param name="createDto">Takip durumu oluşturma DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Eklenen takip durumu DTO'su</returns>
    Task<CargoTrackingDto> AddTrackingStatusAsync(CreateCargoTrackingDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Takip durumunu güncelle
    /// </summary>
    /// <param name="id">Takip durumu ID'si</param>
    /// <param name="updateDto">Takip durumu güncelleme DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncellenen takip durumu DTO'su</returns>
    Task<CargoTrackingDto> UpdateTrackingStatusAsync(Guid id, UpdateCargoTrackingDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Takip durumunu sil
    /// </summary>
    /// <param name="id">Takip durumu ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silme başarılı mı?</returns>
    Task<bool> DeleteTrackingStatusAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Duruma göre takip kayıtlarını getir
    /// </summary>
    /// <param name="status">Kargo durumu</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Takip kayıtları DTO listesi</returns>
    Task<IEnumerable<CargoTrackingDto>> GetTrackingByStatusAsync(CargoStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tarih aralığına göre takip kayıtlarını getir
    /// </summary>
    /// <param name="startDate">Başlangıç tarihi</param>
    /// <param name="endDate">Bitiş tarihi</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Takip kayıtları DTO listesi</returns>
    Task<IEnumerable<CargoTrackingDto>> GetTrackingByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kaynağa göre takip kayıtlarını getir
    /// </summary>
    /// <param name="source">Takip kaynağı</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Takip kayıtları DTO listesi</returns>
    Task<IEnumerable<CargoTrackingDto>> GetTrackingBySourceAsync(string source, CancellationToken cancellationToken = default);

    /// <summary>
    /// Eski takip kayıtlarını temizle
    /// </summary>
    /// <param name="cargoId">Kargo ID'si</param>
    /// <param name="keepLastDays">Son kaç günlük kayıtları tut</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silinen kayıt sayısı</returns>
    Task<int> CleanupOldTrackingRecordsAsync(Guid cargoId, int keepLastDays = 30, CancellationToken cancellationToken = default);

    /// <summary>
    /// Harici API'den kargo durumunu güncelle
    /// </summary>
    /// <param name="trackingNumber">Takip numarası</param>
    /// <param name="status">Kargo durumu</param>
    /// <param name="statusDescription">Durum açıklaması</param>
    /// <param name="location">Lokasyon</param>
    /// <param name="notes">Notlar</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncelleme başarılı mı?</returns>
    Task<bool> UpdateCargoStatusFromExternalAsync(string trackingNumber, CargoStatus status, string statusDescription, string? location = null, string? notes = null, CancellationToken cancellationToken = default);
}
