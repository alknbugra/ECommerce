using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

/// <summary>
/// Unit of Work pattern interface
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Kullanıcı repository
    /// </summary>
    IRepository<User> Users { get; }

    /// <summary>
    /// Rol repository
    /// </summary>
    IRepository<Role> Roles { get; }

    /// <summary>
    /// Kullanıcı rol repository
    /// </summary>
    IUserRoleRepository UserRoles { get; }

    /// <summary>
    /// Kategori repository
    /// </summary>
    IRepository<Category> Categories { get; }

    /// <summary>
    /// Ürün repository
    /// </summary>
    IRepository<Product> Products { get; }

    /// <summary>
    /// Ürün resmi repository
    /// </summary>
    IRepository<ProductImage> ProductImages { get; }

    /// <summary>
    /// Adres repository
    /// </summary>
    IRepository<Address> Addresses { get; }

    /// <summary>
    /// Sipariş repository
    /// </summary>
    IRepository<Order> Orders { get; }

    /// <summary>
    /// Sipariş detayı repository
    /// </summary>
    IRepository<OrderItem> OrderItems { get; }

    /// <summary>
    /// Sipariş durumu geçmişi repository
    /// </summary>
    IRepository<OrderStatusHistory> OrderStatusHistories { get; }

    /// <summary>
    /// Değişiklikleri kaydet
    /// </summary>
    /// <returns>Etkilenen satır sayısı</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Değişiklikleri kaydet (cancellation token ile)
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Etkilenen satır sayısı</returns>
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Transaction başlat
    /// </summary>
    /// <returns>Transaction</returns>
    Task BeginTransactionAsync();

    /// <summary>
    /// Transaction'ı commit et
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Transaction'ı rollback et
    /// </summary>
    Task RollbackTransactionAsync();
}
