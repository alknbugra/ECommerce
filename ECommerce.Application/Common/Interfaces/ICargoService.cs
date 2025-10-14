using ECommerce.Application.DTOs;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Kargo servis interface
/// </summary>
public interface ICargoService
{
    /// <summary>
    /// ID'ye göre kargo getir
    /// </summary>
    /// <param name="id">Kargo ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kargo DTO'su veya null</returns>
    Task<CargoDto?> GetCargoByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Takip numarasına göre kargo getir
    /// </summary>
    /// <param name="trackingNumber">Takip numarası</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kargo DTO'su veya null</returns>
    Task<CargoDto?> GetCargoByTrackingNumberAsync(string trackingNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sipariş ID'sine göre kargo getir
    /// </summary>
    /// <param name="orderId">Sipariş ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kargo DTO'su veya null</returns>
    Task<CargoDto?> GetCargoByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcı ID'sine göre kargoları getir
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kargo DTO listesi</returns>
    Task<IEnumerable<CargoDto>> GetCargosByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Duruma göre kargoları getir
    /// </summary>
    /// <param name="status">Kargo durumu</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kargo DTO listesi</returns>
    Task<IEnumerable<CargoDto>> GetCargosByStatusAsync(CargoStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni kargo oluştur
    /// </summary>
    /// <param name="createCargoDto">Kargo oluşturma DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Oluşturulan kargo DTO'su</returns>
    Task<CargoDto> CreateCargoAsync(CreateCargoDto createCargoDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kargo güncelle
    /// </summary>
    /// <param name="id">Kargo ID'si</param>
    /// <param name="updateCargoDto">Kargo güncelleme DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncellenen kargo DTO'su</returns>
    Task<CargoDto> UpdateCargoAsync(Guid id, UpdateCargoDto updateCargoDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kargo durumunu güncelle
    /// </summary>
    /// <param name="id">Kargo ID'si</param>
    /// <param name="status">Yeni durum</param>
    /// <param name="notes">Notlar</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncelleme başarılı mı?</returns>
    Task<bool> UpdateCargoStatusAsync(Guid id, CargoStatus status, string? notes = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kargo sil
    /// </summary>
    /// <param name="id">Kargo ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silme başarılı mı?</returns>
    Task<bool> DeleteCargoAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kargo istatistiklerini getir
    /// </summary>
    /// <param name="startDate">Başlangıç tarihi</param>
    /// <param name="endDate">Bitiş tarihi</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kargo istatistikleri</returns>
    Task<CargoStatisticsDto> GetCargoStatisticsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
