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
    /// Sepet repository
    /// </summary>
    IRepository<Cart> Carts { get; }

    /// <summary>
    /// Sepet ürünü repository
    /// </summary>
    IRepository<CartItem> CartItems { get; }

    /// <summary>
    /// Ödeme repository
    /// </summary>
    IRepository<Payment> Payments { get; }

    /// <summary>
    /// Email şablonu repository
    /// </summary>
    IRepository<EmailTemplate> EmailTemplates { get; }

    /// <summary>
    /// Email log repository
    /// </summary>
    IRepository<EmailLog> EmailLogs { get; }

    /// <summary>
    /// Stok repository
    /// </summary>
    IRepository<Inventory> Inventories { get; }

    /// <summary>
    /// Stok hareketi repository
    /// </summary>
    IRepository<InventoryMovement> InventoryMovements { get; }

    /// <summary>
    /// Stok uyarısı repository
    /// </summary>
    IRepository<StockAlert> StockAlerts { get; }

    /// <summary>
    /// Kuponlar repository
    /// </summary>
    IRepository<Coupon> Coupons { get; }

    /// <summary>
    /// Kupon kullanımları repository
    /// </summary>
    IRepository<CouponUsage> CouponUsages { get; }

    /// <summary>
    /// Kupon kategorileri repository
    /// </summary>
    IRepository<CouponCategory> CouponCategories { get; }

    /// <summary>
    /// Kupon ürünleri repository
    /// </summary>
    IRepository<CouponProduct> CouponProducts { get; }

    /// <summary>
    /// Kupon kullanıcıları repository
    /// </summary>
    IRepository<CouponUser> CouponUsers { get; }

    /// <summary>
    /// Ürün değerlendirmeleri repository
    /// </summary>
    IRepository<ProductReview> ProductReviews { get; }

    /// <summary>
    /// Değerlendirme yanıtları repository
    /// </summary>
    IRepository<ReviewResponse> ReviewResponses { get; }

    /// <summary>
    /// Değerlendirme oyları repository
    /// </summary>
    IRepository<ReviewVote> ReviewVotes { get; }

    /// <summary>
    /// Değerlendirme resimleri repository
    /// </summary>
    IRepository<ReviewImage> ReviewImages { get; }

    /// <summary>
    /// Favori listeler repository
    /// </summary>
    IRepository<Wishlist> Wishlists { get; }

    /// <summary>
    /// Favori liste ürünleri repository
    /// </summary>
    IRepository<WishlistItem> WishlistItems { get; }

    /// <summary>
    /// Favori liste paylaşımları repository
    /// </summary>
    IRepository<WishlistShare> WishlistShares { get; }

    /// <summary>
    /// Favori ürün fiyat geçmişi repository
    /// </summary>
    IRepository<WishlistItemPriceHistory> WishlistItemPriceHistories { get; }

    /// <summary>
    /// Favori ürün stok geçmişi repository
    /// </summary>
    IRepository<WishlistItemStockHistory> WishlistItemStockHistories { get; }

    /// <summary>
    /// Ürün varyantları repository
    /// </summary>
    IRepository<ProductVariant> ProductVariants { get; }

    /// <summary>
    /// Ürün özellikleri repository
    /// </summary>
    IRepository<ProductAttribute> ProductAttributes { get; }

    /// <summary>
    /// Ürün varyant özellikleri repository
    /// </summary>
    IRepository<ProductVariantAttribute> ProductVariantAttributes { get; }

    /// <summary>
    /// Ürün-özellik ilişkisi repository
    /// </summary>
    IRepository<ProductProductAttribute> ProductProductAttributes { get; }

    /// <summary>
    /// Ürün markaları repository
    /// </summary>
    IRepository<ProductBrand> ProductBrands { get; }

    /// <summary>
    /// Arama geçmişi repository
    /// </summary>
    IRepository<SearchHistory> SearchHistories { get; }

    /// <summary>
    /// Bildirimler repository
    /// </summary>
    IRepository<Notification> Notifications { get; }

    /// <summary>
    /// Bildirim şablonları repository
    /// </summary>
    IRepository<NotificationTemplate> NotificationTemplates { get; }

    /// <summary>
    /// Kargo repository
    /// </summary>
    ICargoRepository Cargos { get; }

    /// <summary>
    /// Kargo şirketi repository
    /// </summary>
    ICargoCompanyRepository CargoCompanies { get; }

    /// <summary>
    /// Kargo takip repository
    /// </summary>
    ICargoTrackingRepository CargoTrackings { get; }

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
