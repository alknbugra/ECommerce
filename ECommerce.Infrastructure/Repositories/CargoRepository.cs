using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

/// <summary>
/// Kargo repository implementasyonu
/// </summary>
public class CargoRepository : Repository<Cargo>, ICargoRepository
{
    public CargoRepository(ECommerceDbContext context) : base(context)
    {
    }

    public async Task<Cargo?> GetByTrackingNumberAsync(string trackingNumber)
    {
        return await _dbSet
            .Include(c => c.CargoCompany)
            .Include(c => c.Order)
            .Include(c => c.TrackingHistory)
            .FirstOrDefaultAsync(c => c.TrackingNumber == trackingNumber && !c.IsDeleted);
    }

    public async Task<Cargo?> GetByOrderIdAsync(Guid orderId)
    {
        return await _dbSet
            .Include(c => c.CargoCompany)
            .Include(c => c.Order)
            .Include(c => c.TrackingHistory)
            .FirstOrDefaultAsync(c => c.OrderId == orderId && !c.IsDeleted);
    }

    public async Task<IEnumerable<Cargo>> GetByCargoCompanyIdAsync(Guid cargoCompanyId)
    {
        return await _dbSet
            .Include(c => c.CargoCompany)
            .Include(c => c.Order)
            .Where(c => c.CargoCompanyId == cargoCompanyId && !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cargo>> GetByStatusAsync(CargoStatus status)
    {
        return await _dbSet
            .Include(c => c.CargoCompany)
            .Include(c => c.Order)
            .Where(c => c.Status == status && !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cargo>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(c => c.CargoCompany)
            .Include(c => c.Order)
            .Where(c => c.Order.UserId == userId && !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cargo>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(c => c.CargoCompany)
            .Include(c => c.Order)
            .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate && !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Cargo?> UpdateStatusAsync(Guid cargoId, CargoStatus status, string? notes = null)
    {
        var cargo = await _dbSet
            .Include(c => c.CargoCompany)
            .Include(c => c.Order)
            .FirstOrDefaultAsync(c => c.Id == cargoId && !c.IsDeleted);

        if (cargo == null)
            return null;

        cargo.Status = status;
        cargo.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(notes))
        {
            cargo.Notes = notes;
        }

        // Durum değişikliklerine göre tarihleri güncelle
        switch (status)
        {
            case CargoStatus.PickedUp:
                cargo.ShippedDate = DateTime.UtcNow;
                break;
            case CargoStatus.Delivered:
                cargo.DeliveredDate = DateTime.UtcNow;
                break;
        }

        _dbSet.Update(cargo);
        return cargo;
    }

    public async Task<bool> TrackingNumberExistsAsync(string trackingNumber)
    {
        return await _dbSet.AnyAsync(c => c.TrackingNumber == trackingNumber && !c.IsDeleted);
    }

    public async Task<CargoStatistics> GetStatisticsAsync(DateTime startDate, DateTime endDate)
    {
        var cargos = await _dbSet
            .Include(c => c.CargoCompany)
            .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate && !c.IsDeleted)
            .ToListAsync();

        var statistics = new CargoStatistics
        {
            TotalCargos = cargos.Count,
            TotalShippingCost = cargos.Sum(c => c.ShippingCost)
        };

        // Duruma göre sayıları hesapla
        foreach (CargoStatus status in Enum.GetValues<CargoStatus>())
        {
            statistics.StatusCounts[status] = cargos.Count(c => c.Status == status);
        }

        // Kargo şirketine göre sayıları hesapla
        statistics.CompanyCounts = cargos
            .GroupBy(c => c.CargoCompany.Name)
            .ToDictionary(g => g.Key, g => g.Count());

        // Ortalama teslim süresini hesapla
        var deliveredCargos = cargos.Where(c => c.DeliveredDate.HasValue && c.ShippedDate.HasValue);
        if (deliveredCargos.Any())
        {
            statistics.AverageDeliveryDays = deliveredCargos
                .Average(c => (c.DeliveredDate!.Value - c.ShippedDate!.Value).TotalDays);
        }

        return statistics;
    }
}
