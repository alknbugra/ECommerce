using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

/// <summary>
/// Kargo şirketi repository implementasyonu
/// </summary>
public class CargoCompanyRepository : Repository<CargoCompany>, ICargoCompanyRepository
{
    public CargoCompanyRepository(ECommerceDbContext context) : base(context)
    {
    }

    public async Task<CargoCompany?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(cc => cc.Code == code && !cc.IsDeleted);
    }

    public async Task<IEnumerable<CargoCompany>> GetActiveCompaniesAsync()
    {
        return await _dbSet
            .Where(cc => cc.IsActive && !cc.IsDeleted)
            .OrderBy(cc => cc.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<CargoCompany>> SearchByNameAsync(string name)
    {
        return await _dbSet
            .Where(cc => cc.Name.Contains(name) && !cc.IsDeleted)
            .OrderBy(cc => cc.Name)
            .ToListAsync();
    }

    public async Task<bool> CodeExistsAsync(string code, Guid? excludeId = null)
    {
        var query = _dbSet.Where(cc => cc.Code == code && !cc.IsDeleted);
        
        if (excludeId.HasValue)
        {
            query = query.Where(cc => cc.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<CargoCompany?> SetActiveStatusAsync(Guid id, bool isActive)
    {
        var company = await _dbSet
            .FirstOrDefaultAsync(cc => cc.Id == id && !cc.IsDeleted);

        if (company == null)
            return null;

        company.IsActive = isActive;
        company.UpdatedAt = DateTime.UtcNow;

        _dbSet.Update(company);
        return company;
    }
}
