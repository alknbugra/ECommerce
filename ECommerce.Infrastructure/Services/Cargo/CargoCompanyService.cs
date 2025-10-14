using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Services.Cargo;

/// <summary>
/// Kargo şirketi servis implementasyonu
/// </summary>
public class CargoCompanyService : ICargoCompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CargoCompanyService> _logger;

    public CargoCompanyService(
        IUnitOfWork unitOfWork,
        ILogger<CargoCompanyService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CargoCompanyDto?> GetCargoCompanyByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var company = await _unitOfWork.CargoCompanies.GetByIdAsync(id);
            if (company == null)
                return null;

            return MapToDto(company);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo şirketi getirilirken hata oluştu. ID: {Id}", id);
            throw;
        }
    }

    public async Task<CargoCompanyDto?> GetCargoCompanyByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        try
        {
            var company = await _unitOfWork.CargoCompanies.GetByCodeAsync(code);
            if (company == null)
                return null;

            return MapToDto(company);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo şirketi getirilirken hata oluştu. Kod: {Code}", code);
            throw;
        }
    }

    public async Task<IEnumerable<CargoCompanyDto>> GetAllCargoCompaniesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var companies = await _unitOfWork.CargoCompanies.GetAllAsync();
            return companies.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo şirketleri getirilirken hata oluştu");
            throw;
        }
    }

    public async Task<IEnumerable<CargoCompanyDto>> GetActiveCargoCompaniesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var companies = await _unitOfWork.CargoCompanies.GetActiveCompaniesAsync();
            return companies.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Aktif kargo şirketleri getirilirken hata oluştu");
            throw;
        }
    }

    public async Task<IEnumerable<CargoCompanyDto>> SearchCargoCompaniesAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            var companies = await _unitOfWork.CargoCompanies.SearchByNameAsync(name);
            return companies.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo şirketleri aranırken hata oluştu. Arama: {Name}", name);
            throw;
        }
    }

    public async Task<CargoCompanyDto> CreateCargoCompanyAsync(CreateCargoCompanyDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Kodun benzersiz olup olmadığını kontrol et
            if (await _unitOfWork.CargoCompanies.CodeExistsAsync(createDto.Code))
                throw new ArgumentException("Bu kod zaten kullanılıyor.");

            var company = new CargoCompany
            {
                Name = createDto.Name,
                Code = createDto.Code,
                LogoUrl = createDto.LogoUrl,
                Website = createDto.Website,
                PhoneNumber = createDto.PhoneNumber,
                Email = createDto.Email,
                Address = createDto.Address,
                IsActive = true,
                ApiEndpoint = createDto.ApiEndpoint,
                ApiKey = createDto.ApiKey,
                TrackingUrlTemplate = createDto.TrackingUrlTemplate,
                Description = createDto.Description
            };

            await _unitOfWork.CargoCompanies.AddAsync(company);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo şirketi oluşturuldu. ID: {Id}, Ad: {Name}", company.Id, company.Name);

            return MapToDto(company);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo şirketi oluşturulurken hata oluştu");
            throw;
        }
    }

    public async Task<CargoCompanyDto> UpdateCargoCompanyAsync(Guid id, UpdateCargoCompanyDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var company = await _unitOfWork.CargoCompanies.GetByIdAsync(id);
            if (company == null)
                throw new ArgumentException("Kargo şirketi bulunamadı.");

            // Kodun benzersiz olup olmadığını kontrol et (kendisi hariç)
            if (await _unitOfWork.CargoCompanies.CodeExistsAsync(updateDto.Code, id))
                throw new ArgumentException("Bu kod zaten kullanılıyor.");

            company.Name = updateDto.Name;
            company.Code = updateDto.Code;
            company.LogoUrl = updateDto.LogoUrl;
            company.Website = updateDto.Website;
            company.PhoneNumber = updateDto.PhoneNumber;
            company.Email = updateDto.Email;
            company.Address = updateDto.Address;
            company.IsActive = updateDto.IsActive;
            company.ApiEndpoint = updateDto.ApiEndpoint;
            company.ApiKey = updateDto.ApiKey;
            company.TrackingUrlTemplate = updateDto.TrackingUrlTemplate;
            company.Description = updateDto.Description;
            company.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CargoCompanies.UpdateAsync(company);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo şirketi güncellendi. ID: {Id}", id);

            return MapToDto(company);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo şirketi güncellenirken hata oluştu. ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> SetCargoCompanyActiveStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default)
    {
        try
        {
            var company = await _unitOfWork.CargoCompanies.SetActiveStatusAsync(id, isActive);
            if (company == null)
                return false;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo şirketi aktif durumu güncellendi. ID: {Id}, Aktif: {IsActive}", id, isActive);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo şirketi aktif durumu güncellenirken hata oluştu. ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteCargoCompanyAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var company = await _unitOfWork.CargoCompanies.DeleteByIdAsync(id);
            if (company == null)
                return false;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo şirketi silindi. ID: {Id}", id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo şirketi silinirken hata oluştu. ID: {Id}", id);
            throw;
        }
    }

    private static CargoCompanyDto MapToDto(CargoCompany company)
    {
        return new CargoCompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            Code = company.Code,
            LogoUrl = company.LogoUrl,
            Website = company.Website,
            PhoneNumber = company.PhoneNumber,
            Email = company.Email,
            Address = company.Address,
            IsActive = company.IsActive,
            TrackingUrlTemplate = company.TrackingUrlTemplate,
            Description = company.Description,
            CreatedAt = company.CreatedAt,
            UpdatedAt = company.UpdatedAt
        };
    }
}
