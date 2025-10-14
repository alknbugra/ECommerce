using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementasyonu
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ECommerceDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ECommerceDbContext context)
    {
        _context = context;
        
        // Repository'leri başlat
        Users = new Repository<User>(_context);
        Roles = new Repository<Role>(_context);
        UserRoles = new UserRoleRepository(_context);
        Categories = new Repository<Category>(_context);
        Products = new Repository<Product>(_context);
        ProductImages = new Repository<ProductImage>(_context);
        Addresses = new Repository<Address>(_context);
        Orders = new Repository<Order>(_context);
        OrderItems = new Repository<OrderItem>(_context);
        OrderStatusHistories = new Repository<OrderStatusHistory>(_context);
        Carts = new Repository<Cart>(_context);
        CartItems = new Repository<CartItem>(_context);
        Payments = new Repository<Payment>(_context);
        EmailTemplates = new Repository<EmailTemplate>(_context);
        EmailLogs = new Repository<EmailLog>(_context);
        Inventories = new Repository<Inventory>(_context);
        InventoryMovements = new Repository<InventoryMovement>(_context);
        StockAlerts = new Repository<StockAlert>(_context);
        Coupons = new Repository<Coupon>(_context);
        CouponUsages = new Repository<CouponUsage>(_context);
        CouponCategories = new Repository<CouponCategory>(_context);
        CouponProducts = new Repository<CouponProduct>(_context);
        CouponUsers = new Repository<CouponUser>(_context);
        ProductReviews = new Repository<ProductReview>(_context);
        ReviewResponses = new Repository<ReviewResponse>(_context);
        ReviewVotes = new Repository<ReviewVote>(_context);
        ReviewImages = new Repository<ReviewImage>(_context);
        Wishlists = new Repository<Wishlist>(_context);
        WishlistItems = new Repository<WishlistItem>(_context);
        WishlistShares = new Repository<WishlistShare>(_context);
        WishlistItemPriceHistories = new Repository<WishlistItemPriceHistory>(_context);
        WishlistItemStockHistories = new Repository<WishlistItemStockHistory>(_context);
    }

    public IRepository<User> Users { get; }
    public IRepository<Role> Roles { get; }
    public IUserRoleRepository UserRoles { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<Product> Products { get; }
    public IRepository<ProductImage> ProductImages { get; }
    public IRepository<Address> Addresses { get; }
    public IRepository<Order> Orders { get; }
    public IRepository<OrderItem> OrderItems { get; }
    public IRepository<OrderStatusHistory> OrderStatusHistories { get; }
    public IRepository<Cart> Carts { get; }
    public IRepository<CartItem> CartItems { get; }
    public IRepository<Payment> Payments { get; }
    public IRepository<EmailTemplate> EmailTemplates { get; }
    public IRepository<EmailLog> EmailLogs { get; }
    public IRepository<Inventory> Inventories { get; }
    public IRepository<InventoryMovement> InventoryMovements { get; }
    public IRepository<StockAlert> StockAlerts { get; }
    public IRepository<Coupon> Coupons { get; }
    public IRepository<CouponUsage> CouponUsages { get; }
    public IRepository<CouponCategory> CouponCategories { get; }
    public IRepository<CouponProduct> CouponProducts { get; }
    public IRepository<CouponUser> CouponUsers { get; }
    public IRepository<ProductReview> ProductReviews { get; }
    public IRepository<ReviewResponse> ReviewResponses { get; }
    public IRepository<ReviewVote> ReviewVotes { get; }
    public IRepository<ReviewImage> ReviewImages { get; }
    public IRepository<Wishlist> Wishlists { get; }
    public IRepository<WishlistItem> WishlistItems { get; }
    public IRepository<WishlistShare> WishlistShares { get; }
    public IRepository<WishlistItemPriceHistory> WishlistItemPriceHistories { get; }
    public IRepository<WishlistItemStockHistory> WishlistItemStockHistories { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
