using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

/// <summary>
/// Kargo takip repository implementasyonu
/// </summary>
public class CargoTrackingRepository : Repository<CargoTracking>, ICargoTrackingRepository
{
    public CargoTrackingRepository(ECommerceDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CargoTracking>> GetByCargoIdAsync(Guid cargoId)
    {
        return await _dbSet
            .Where(ct => ct.CargoId == cargoId && !ct.IsDeleted)
            .OrderBy(ct => ct.TrackingDate)
            .ToListAsync();
    }

    public async Task<CargoTracking?> GetCurrentStatusAsync(Guid cargoId)
    {
        return await _dbSet
            .Where(ct => ct.CargoId == cargoId && ct.IsCurrent && !ct.IsDeleted)
            .OrderByDescending(ct => ct.TrackingDate)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<CargoTracking>> GetByStatusAsync(CargoStatus status)
    {
        return await _dbSet
            .Include(ct => ct.Cargo)
            .Where(ct => ct.Status == status && !ct.IsDeleted)
            .OrderByDescending(ct => ct.TrackingDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CargoTracking>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(ct => ct.Cargo)
            .Where(ct => ct.TrackingDate >= startDate && ct.TrackingDate <= endDate && !ct.IsDeleted)
            .OrderByDescending(ct => ct.TrackingDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CargoTracking>> GetBySourceAsync(string source)
    {
        return await _dbSet
            .Include(ct => ct.Cargo)
            .Where(ct => ct.Source == source && !ct.IsDeleted)
            .OrderByDescending(ct => ct.TrackingDate)
            .ToListAsync();
    }

    public async Task<CargoTracking> AddTrackingStatusAsync(
        Guid cargoId,
        CargoStatus status,
        string statusDescription,
        string? location = null,
        string? notes = null,
        string? source = null,
        string? trackingData = null)
    {
        // Önceki güncel durumları false yap
        var currentTrackings = await _dbSet
            .Where(ct => ct.CargoId == cargoId && ct.IsCurrent && !ct.IsDeleted)
            .ToListAsync();

        foreach (var tracking in currentTrackings)
        {
            tracking.IsCurrent = false;
            tracking.LastUpdated = DateTime.UtcNow;
        }

        // Yeni takip durumu ekle
        var newTracking = new CargoTracking
        {
            CargoId = cargoId,
            Status = status,
            StatusDescription = statusDescription,
            Location = location,
            TrackingDate = DateTime.UtcNow,
            Notes = notes,
            Source = source,
            TrackingData = trackingData,
            IsCurrent = true,
            LastUpdated = DateTime.UtcNow
        };

        await _dbSet.AddAsync(newTracking);
        return newTracking;
    }

    public async Task<CargoTracking?> UpdateCurrentStatusAsync(
        Guid cargoId,
        CargoStatus status,
        string statusDescription,
        string? location = null,
        string? notes = null,
        string? source = null,
        string? trackingData = null)
    {
        var currentTracking = await GetCurrentStatusAsync(cargoId);
        
        if (currentTracking == null)
            return null;

        currentTracking.Status = status;
        currentTracking.StatusDescription = statusDescription;
        currentTracking.Location = location;
        currentTracking.Notes = notes;
        currentTracking.Source = source;
        currentTracking.TrackingData = trackingData;
        currentTracking.LastUpdated = DateTime.UtcNow;

        _dbSet.Update(currentTracking);
        return currentTracking;
    }

    public async Task<int> CleanupOldTrackingRecordsAsync(Guid cargoId, int keepLastDays = 30)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-keepLastDays);
        
        var oldRecords = await _dbSet
            .Where(ct => ct.CargoId == cargoId && 
                        ct.TrackingDate < cutoffDate && 
                        !ct.IsCurrent && 
                        !ct.IsDeleted)
            .ToListAsync();

        if (oldRecords.Any())
        {
            _dbSet.RemoveRange(oldRecords);
        }

        return oldRecords.Count;
    }
}
