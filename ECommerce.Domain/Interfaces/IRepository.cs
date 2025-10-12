using System.Linq.Expressions;

namespace ECommerce.Domain.Interfaces;

/// <summary>
/// Generic repository interface
/// </summary>
/// <typeparam name="T">Entity tipi</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// ID'ye göre entity getir
    /// </summary>
    /// <param name="id">Entity ID'si</param>
    /// <returns>Entity veya null</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Tüm entity'leri getir
    /// </summary>
    /// <returns>Entity listesi</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Koşula göre entity'leri getir
    /// </summary>
    /// <param name="predicate">Filtre koşulu</param>
    /// <returns>Entity listesi</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Koşula göre tek entity getir
    /// </summary>
    /// <param name="predicate">Filtre koşulu</param>
    /// <returns>Entity veya null</returns>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Entity ekle
    /// </summary>
    /// <param name="entity">Eklenecek entity</param>
    /// <returns>Eklenen entity</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Birden fazla entity ekle
    /// </summary>
    /// <param name="entities">Eklenecek entity'ler</param>
    /// <returns>Eklenen entity'ler</returns>
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Entity güncelle
    /// </summary>
    /// <param name="entity">Güncellenecek entity</param>
    /// <returns>Güncellenen entity</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Entity sil (soft delete)
    /// </summary>
    /// <param name="entity">Silinecek entity</param>
    /// <returns>Silinen entity</returns>
    Task<T> DeleteAsync(T entity);

    /// <summary>
    /// ID'ye göre entity sil
    /// </summary>
    /// <param name="id">Silinecek entity ID'si</param>
    /// <returns>Silinen entity</returns>
    Task<T?> DeleteByIdAsync(Guid id);

    /// <summary>
    /// Koşula göre entity sayısını getir
    /// </summary>
    /// <param name="predicate">Filtre koşulu</param>
    /// <returns>Entity sayısı</returns>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

    /// <summary>
    /// Koşula göre entity var mı kontrol et
    /// </summary>
    /// <param name="predicate">Filtre koşulu</param>
    /// <returns>Var mı?</returns>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Koşula göre entity var mı kontrol et (AnyAsync alias)
    /// </summary>
    /// <param name="predicate">Filtre koşulu</param>
    /// <returns>Var mı?</returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
}
