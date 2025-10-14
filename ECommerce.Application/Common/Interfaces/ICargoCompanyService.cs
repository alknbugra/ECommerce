using ECommerce.Application.DTOs;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Kargo şirketi servis interface
/// </summary>
public interface ICargoCompanyService
{
    /// <summary>
    /// ID'ye göre kargo şirketi getir
    /// </summary>
    /// <param name="id">Kargo şirketi ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kargo şirketi DTO'su veya null</returns>
    Task<CargoCompanyDto?> GetCargoCompanyByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Koda göre kargo şirketi getir
    /// </summary>
    /// <param name="code">Kargo şirketi kodu</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kargo şirketi DTO'su veya null</returns>
    Task<CargoCompanyDto?> GetCargoCompanyByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tüm kargo şirketlerini getir
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kargo şirketi DTO listesi</returns>
    Task<IEnumerable<CargoCompanyDto>> GetAllCargoCompaniesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Aktif kargo şirketlerini getir
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kargo şirketi DTO listesi</returns>
    Task<IEnumerable<CargoCompanyDto>> GetActiveCargoCompaniesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Kargo şirketlerini ara
    /// </summary>
    /// <param name="name">Arama terimi</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kargo şirketi DTO listesi</returns>
    Task<IEnumerable<CargoCompanyDto>> SearchCargoCompaniesAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni kargo şirketi oluştur
    /// </summary>
    /// <param name="createDto">Kargo şirketi oluşturma DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Oluşturulan kargo şirketi DTO'su</returns>
    Task<CargoCompanyDto> CreateCargoCompanyAsync(CreateCargoCompanyDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kargo şirketi güncelle
    /// </summary>
    /// <param name="id">Kargo şirketi ID'si</param>
    /// <param name="updateDto">Kargo şirketi güncelleme DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncellenen kargo şirketi DTO'su</returns>
    Task<CargoCompanyDto> UpdateCargoCompanyAsync(Guid id, UpdateCargoCompanyDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kargo şirketi aktif durumunu güncelle
    /// </summary>
    /// <param name="id">Kargo şirketi ID'si</param>
    /// <param name="isActive">Aktif mi?</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncelleme başarılı mı?</returns>
    Task<bool> SetCargoCompanyActiveStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kargo şirketi sil
    /// </summary>
    /// <param name="id">Kargo şirketi ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silme başarılı mı?</returns>
    Task<bool> DeleteCargoCompanyAsync(Guid id, CancellationToken cancellationToken = default);
}
