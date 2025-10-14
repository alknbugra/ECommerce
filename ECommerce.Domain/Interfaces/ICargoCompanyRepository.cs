using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

/// <summary>
/// Kargo şirketi repository interface
/// </summary>
public interface ICargoCompanyRepository : IRepository<CargoCompany>
{
    /// <summary>
    /// Kargo şirketi koduna göre getir
    /// </summary>
    /// <param name="code">Kargo şirketi kodu</param>
    /// <returns>Kargo şirketi veya null</returns>
    Task<CargoCompany?> GetByCodeAsync(string code);

    /// <summary>
    /// Aktif kargo şirketlerini getir
    /// </summary>
    /// <returns>Kargo şirketi listesi</returns>
    Task<IEnumerable<CargoCompany>> GetActiveCompaniesAsync();

    /// <summary>
    /// Kargo şirketi adına göre ara
    /// </summary>
    /// <param name="name">Kargo şirketi adı</param>
    /// <returns>Kargo şirketi listesi</returns>
    Task<IEnumerable<CargoCompany>> SearchByNameAsync(string name);

    /// <summary>
    /// Kargo şirketi kodunun var olup olmadığını kontrol et
    /// </summary>
    /// <param name="code">Kargo şirketi kodu</param>
    /// <param name="excludeId">Hariç tutulacak ID</param>
    /// <returns>Var mı?</returns>
    Task<bool> CodeExistsAsync(string code, Guid? excludeId = null);

    /// <summary>
    /// Kargo şirketini aktif/pasif yap
    /// </summary>
    /// <param name="id">Kargo şirketi ID'si</param>
    /// <param name="isActive">Aktif mi?</param>
    /// <returns>Güncellenen kargo şirketi</returns>
    Task<CargoCompany?> SetActiveStatusAsync(Guid id, bool isActive);
}
