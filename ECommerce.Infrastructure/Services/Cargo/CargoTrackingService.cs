using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Services.Cargo;

/// <summary>
/// Kargo takip servis implementasyonu
/// </summary>
public class CargoTrackingService : ICargoTrackingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CargoTrackingService> _logger;

    public CargoTrackingService(
        IUnitOfWork unitOfWork,
        ILogger<CargoTrackingService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<CargoTrackingDto>> GetTrackingHistoryAsync(Guid cargoId, CancellationToken cancellationToken = default)
    {
        try
        {
            var trackingHistory = await _unitOfWork.CargoTrackings.GetByCargoIdAsync(cargoId);
            return trackingHistory.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo takip geçmişi getirilirken hata oluştu. Kargo ID: {CargoId}", cargoId);
            throw;
        }
    }

    public async Task<CargoTrackingDto?> GetCurrentStatusAsync(Guid cargoId, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentStatus = await _unitOfWork.CargoTrackings.GetCurrentStatusAsync(cargoId);
            if (currentStatus == null)
                return null;

            return MapToDto(currentStatus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo güncel durumu getirilirken hata oluştu. Kargo ID: {CargoId}", cargoId);
            throw;
        }
    }

    public async Task<CargoTrackingDto> AddTrackingStatusAsync(CreateCargoTrackingDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Kargonun var olup olmadığını kontrol et
            var cargo = await _unitOfWork.Cargos.GetByIdAsync(createDto.CargoId);
            if (cargo == null)
                throw new ArgumentException("Kargo bulunamadı.");

            var tracking = await _unitOfWork.CargoTrackings.AddTrackingStatusAsync(
                createDto.CargoId,
                createDto.Status,
                createDto.StatusDescription,
                createDto.Location,
                createDto.Notes,
                createDto.Source,
                createDto.TrackingData
            );

            // Kargo durumunu da güncelle
            await _unitOfWork.Cargos.UpdateStatusAsync(createDto.CargoId, createDto.Status, createDto.Notes);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo takip durumu eklendi. Kargo ID: {CargoId}, Durum: {Status}", createDto.CargoId, createDto.Status);

            return MapToDto(tracking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo takip durumu eklenirken hata oluştu. Kargo ID: {CargoId}", createDto.CargoId);
            throw;
        }
    }

    public async Task<CargoTrackingDto> UpdateTrackingStatusAsync(Guid id, UpdateCargoTrackingDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var tracking = await _unitOfWork.CargoTrackings.GetByIdAsync(id);
            if (tracking == null)
                throw new ArgumentException("Takip kaydı bulunamadı.");

            tracking.Status = updateDto.Status;
            tracking.StatusDescription = updateDto.StatusDescription;
            tracking.Location = updateDto.Location;
            tracking.Notes = updateDto.Notes;
            tracking.Source = updateDto.Source;
            tracking.TrackingData = updateDto.TrackingData;
            tracking.LastUpdated = DateTime.UtcNow;

            if (updateDto.TrackingDate.HasValue)
            {
                tracking.TrackingDate = updateDto.TrackingDate.Value;
            }

            await _unitOfWork.CargoTrackings.UpdateAsync(tracking);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo takip durumu güncellendi. ID: {Id}", id);

            return MapToDto(tracking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo takip durumu güncellenirken hata oluştu. ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteTrackingStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var tracking = await _unitOfWork.CargoTrackings.DeleteByIdAsync(id);
            if (tracking == null)
                return false;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo takip durumu silindi. ID: {Id}", id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo takip durumu silinirken hata oluştu. ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<CargoTrackingDto>> GetTrackingByStatusAsync(CargoStatus status, CancellationToken cancellationToken = default)
    {
        try
        {
            var trackings = await _unitOfWork.CargoTrackings.GetByStatusAsync(status);
            return trackings.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Duruma göre takip kayıtları getirilirken hata oluştu. Durum: {Status}", status);
            throw;
        }
    }

    public async Task<IEnumerable<CargoTrackingDto>> GetTrackingByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            var trackings = await _unitOfWork.CargoTrackings.GetByDateRangeAsync(startDate, endDate);
            return trackings.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tarih aralığına göre takip kayıtları getirilirken hata oluştu");
            throw;
        }
    }

    public async Task<IEnumerable<CargoTrackingDto>> GetTrackingBySourceAsync(string source, CancellationToken cancellationToken = default)
    {
        try
        {
            var trackings = await _unitOfWork.CargoTrackings.GetBySourceAsync(source);
            return trackings.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kaynağa göre takip kayıtları getirilirken hata oluştu. Kaynak: {Source}", source);
            throw;
        }
    }

    public async Task<int> CleanupOldTrackingRecordsAsync(Guid cargoId, int keepLastDays = 30, CancellationToken cancellationToken = default)
    {
        try
        {
            var deletedCount = await _unitOfWork.CargoTrackings.CleanupOldTrackingRecordsAsync(cargoId, keepLastDays);
            
            if (deletedCount > 0)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Eski takip kayıtları temizlendi. Kargo ID: {CargoId}, Silinen: {Count}", cargoId, deletedCount);
            }

            return deletedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Eski takip kayıtları temizlenirken hata oluştu. Kargo ID: {CargoId}", cargoId);
            throw;
        }
    }

    public async Task<bool> UpdateCargoStatusFromExternalAsync(string trackingNumber, CargoStatus status, string statusDescription, string? location = null, string? notes = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var cargo = await _unitOfWork.Cargos.GetByTrackingNumberAsync(trackingNumber);
            if (cargo == null)
                return false;

            // Takip durumunu ekle
            await _unitOfWork.CargoTrackings.AddTrackingStatusAsync(
                cargo.Id,
                status,
                statusDescription,
                location,
                notes,
                "External API"
            );

            // Kargo durumunu güncelle
            await _unitOfWork.Cargos.UpdateStatusAsync(cargo.Id, status, notes);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo durumu harici API'den güncellendi. Takip Numarası: {TrackingNumber}, Durum: {Status}", trackingNumber, status);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo durumu harici API'den güncellenirken hata oluştu. Takip Numarası: {TrackingNumber}", trackingNumber);
            throw;
        }
    }

    private static CargoTrackingDto MapToDto(CargoTracking tracking)
    {
        return new CargoTrackingDto
        {
            Id = tracking.Id,
            CargoId = tracking.CargoId,
            TrackingNumber = tracking.Cargo?.TrackingNumber ?? string.Empty,
            Status = tracking.Status,
            StatusDescription = tracking.StatusDescription,
            Location = tracking.Location,
            TrackingDate = tracking.TrackingDate,
            Notes = tracking.Notes,
            Source = tracking.Source,
            TrackingData = tracking.TrackingData,
            IsCurrent = tracking.IsCurrent,
            LastUpdated = tracking.LastUpdated,
            CreatedAt = tracking.CreatedAt
        };
    }
}
