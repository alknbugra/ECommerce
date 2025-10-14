using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data;

/// <summary>
/// E-Ticaret veritabanı context'i
/// </summary>
public class ECommerceDbContext : DbContext
{
    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Kullanıcılar
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Roller
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Kullanıcı rolleri
    /// </summary>
    public DbSet<UserRole> UserRoles { get; set; }

    /// <summary>
    /// Kategoriler
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>
    /// Ürünler
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Ürün resimleri
    /// </summary>
    public DbSet<ProductImage> ProductImages { get; set; }

    /// <summary>
    /// Adresler
    /// </summary>
    public DbSet<Address> Addresses { get; set; }

    /// <summary>
    /// Siparişler
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Sipariş detayları
    /// </summary>
    public DbSet<OrderItem> OrderItems { get; set; }

    /// <summary>
    /// Sipariş durumu geçmişi
    /// </summary>
    public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    /// <summary>
    /// Yetkiler
    /// </summary>
    public DbSet<Permission> Permissions { get; set; }

    /// <summary>
    /// Rol-yetki ilişkileri
    /// </summary>
    public DbSet<RolePermission> RolePermissions { get; set; }

    /// <summary>
    /// Alışveriş sepetleri
    /// </summary>
    public DbSet<Cart> Carts { get; set; }

    /// <summary>
    /// Sepet ürünleri
    /// </summary>
    public DbSet<CartItem> CartItems { get; set; }

    /// <summary>
    /// Ödemeler
    /// </summary>
    public DbSet<Payment> Payments { get; set; }

    /// <summary>
    /// Email şablonları
    /// </summary>
    public DbSet<EmailTemplate> EmailTemplates { get; set; }

    /// <summary>
    /// Email logları
    /// </summary>
    public DbSet<EmailLog> EmailLogs { get; set; }

    /// <summary>
    /// Stok bilgileri
    /// </summary>
    public DbSet<Inventory> Inventories { get; set; }

    /// <summary>
    /// Stok hareketleri
    /// </summary>
    public DbSet<InventoryMovement> InventoryMovements { get; set; }

    /// <summary>
    /// Stok uyarıları
    /// </summary>
    public DbSet<StockAlert> StockAlerts { get; set; }

    /// <summary>
    /// Kuponlar
    /// </summary>
    public DbSet<Coupon> Coupons { get; set; }

    /// <summary>
    /// Kupon kullanımları
    /// </summary>
    public DbSet<CouponUsage> CouponUsages { get; set; }

    /// <summary>
    /// Kupon kategorileri
    /// </summary>
    public DbSet<CouponCategory> CouponCategories { get; set; }

    /// <summary>
    /// Kupon ürünleri
    /// </summary>
    public DbSet<CouponProduct> CouponProducts { get; set; }

    /// <summary>
    /// Kupon kullanıcıları
    /// </summary>
    public DbSet<CouponUser> CouponUsers { get; set; }

    /// <summary>
    /// Ürün değerlendirmeleri
    /// </summary>
    public DbSet<ProductReview> ProductReviews { get; set; }

    /// <summary>
    /// Değerlendirme yanıtları
    /// </summary>
    public DbSet<ReviewResponse> ReviewResponses { get; set; }

    /// <summary>
    /// Değerlendirme oyları
    /// </summary>
    public DbSet<ReviewVote> ReviewVotes { get; set; }

    /// <summary>
    /// Değerlendirme resimleri
    /// </summary>
    public DbSet<ReviewImage> ReviewImages { get; set; }

    /// <summary>
    /// Favori listeler
    /// </summary>
    public DbSet<Wishlist> Wishlists { get; set; }

    /// <summary>
    /// Favori liste ürünleri
    /// </summary>
    public DbSet<WishlistItem> WishlistItems { get; set; }

    /// <summary>
    /// Favori liste paylaşımları
    /// </summary>
    public DbSet<WishlistShare> WishlistShares { get; set; }

    /// <summary>
    /// Favori ürün fiyat geçmişi
    /// </summary>
    public DbSet<WishlistItemPriceHistory> WishlistItemPriceHistories { get; set; }

    /// <summary>
    /// Favori ürün stok geçmişi
    /// </summary>
    public DbSet<WishlistItemStockHistory> WishlistItemStockHistories { get; set; }

    /// <summary>
    /// Ürün varyantları
    /// </summary>
    public DbSet<ProductVariant> ProductVariants { get; set; }

    /// <summary>
    /// Ürün özellikleri
    /// </summary>
    public DbSet<ProductAttribute> ProductAttributes { get; set; }

    /// <summary>
    /// Ürün varyant özellikleri
    /// </summary>
    public DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }

    /// <summary>
    /// Ürün-özellik ilişkisi
    /// </summary>
    public DbSet<ProductProductAttribute> ProductProductAttributes { get; set; }

    /// <summary>
    /// Ürün markaları
    /// </summary>
    public DbSet<ProductBrand> ProductBrands { get; set; }

    /// <summary>
    /// Arama geçmişi
    /// </summary>
    public DbSet<SearchHistory> SearchHistories { get; set; }

    /// <summary>
    /// Bildirimler
    /// </summary>
    public DbSet<Notification> Notifications { get; set; }

    /// <summary>
    /// Bildirim şablonları
    /// </summary>
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity yapılandırması
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Role entity yapılandırması
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // UserRole entity yapılandırması
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();
        });

        // Category entity yapılandırması
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            
            entity.HasOne(e => e.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(e => e.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Product entity yapılandırması
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.ShortDescription).HasMaxLength(500);
            entity.Property(e => e.Sku).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.DiscountedPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.MainImageUrl).HasMaxLength(500);
            
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasIndex(e => e.Sku).IsUnique();
        });

        // ProductImage entity yapılandırması
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.AltText).HasMaxLength(200);
            
            entity.HasOne(e => e.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Address entity yapılandırması
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CompanyName).HasMaxLength(100);
            entity.Property(e => e.AddressLine1).IsRequired().HasMaxLength(200);
            entity.Property(e => e.AddressLine2).HasMaxLength(200);
            entity.Property(e => e.City).IsRequired().HasMaxLength(50);
            entity.Property(e => e.State).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.AddressType).HasMaxLength(20);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Order entity yapılandırması
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.PaymentStatus).IsRequired().HasMaxLength(20);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentId).HasMaxLength(100);
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ShippingCost).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TrackingNumber).HasMaxLength(100);
            entity.Property(e => e.ShippingCompany).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.ShippingAddress)
                .WithMany()
                .HasForeignKey(e => e.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.BillingAddress)
                .WithMany()
                .HasForeignKey(e => e.BillingAddressId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasIndex(e => e.OrderNumber).IsUnique();
        });

        // OrderItem entity yapılandırması
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ProductSku).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ProductImageUrl).HasMaxLength(500);
            
            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // OrderStatusHistory entity yapılandırması
        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PreviousStatus).HasMaxLength(20);
            entity.Property(e => e.NewStatus).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Notes).HasMaxLength(500);
            
            entity.HasOne(e => e.Order)
                .WithMany(o => o.StatusHistory)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Permission entity yapılandırması
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Resource).IsRequired().HasMaxLength(100);
            
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => new { e.Category, e.Action, e.Resource }).IsUnique();
        });

        // RolePermission entity yapılandırması
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasIndex(e => new { e.RoleId, e.PermissionId }).IsUnique();
        });

        // Cart entity yapılandırması
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SessionId).HasMaxLength(100);
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
                
            entity.HasIndex(e => e.SessionId);
            entity.HasIndex(e => e.UserId);
        });

        // CartItem entity yapılandırması
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(e => e.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.CartId, e.ProductId }).IsUnique();
        });

        // Payment entity yapılandırması
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Currency).IsRequired().HasMaxLength(3);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            entity.Property(e => e.GatewayPaymentId).HasMaxLength(100);
            entity.Property(e => e.GatewayTransactionId).HasMaxLength(100);
            entity.Property(e => e.ErrorMessage).HasMaxLength(500);
            entity.Property(e => e.RefundAmount).HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.GatewayPaymentId);
            entity.HasIndex(e => e.GatewayTransactionId);
            entity.HasIndex(e => e.Status);
        });

        // EmailTemplate entity yapılandırması
        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Subject).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Variables).HasMaxLength(1000);
            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => e.Category);
        });

        // EmailLog entity yapılandırması
        modelBuilder.Entity<EmailLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ToEmail).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ToName).HasMaxLength(100);
            entity.Property(e => e.FromEmail).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FromName).HasMaxLength(100);
            entity.Property(e => e.Subject).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.EmailType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.RelatedEntityType).HasMaxLength(50);
            entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
            entity.Property(e => e.ProviderResponse).HasMaxLength(1000);
            entity.Property(e => e.ProviderEmailId).HasMaxLength(100);

            entity.HasIndex(e => e.ToEmail);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.EmailType);
            entity.HasIndex(e => e.RelatedEntityId);
            entity.HasIndex(e => e.SentAt);
        });

        // Inventory entity yapılandırması
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CurrentStock).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ReservedStock).HasColumnType("decimal(18,2)");
            entity.Property(e => e.MinimumStock).HasColumnType("decimal(18,2)");
            entity.Property(e => e.MaximumStock).HasColumnType("decimal(18,2)");
            entity.Property(e => e.AlertStock).HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ProductId).IsUnique();
            entity.HasIndex(e => e.CurrentStock);
            entity.HasIndex(e => e.IsActive);
        });

        // InventoryMovement entity yapılandırması
        modelBuilder.Entity<InventoryMovement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MovementType).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
            entity.Property(e => e.PreviousStock).HasColumnType("decimal(18,2)");
            entity.Property(e => e.NewStock).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.RelatedEntityType).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.ProductId);
            entity.HasIndex(e => e.MovementType);
            entity.HasIndex(e => e.MovementDate);
            entity.HasIndex(e => e.RelatedEntityId);
        });

        // StockAlert entity yapılandırması
        modelBuilder.Entity<StockAlert>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AlertType).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CurrentStock).HasColumnType("decimal(18,2)");
            entity.Property(e => e.AlertLevel).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Message).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ReadByUser)
                .WithMany()
                .HasForeignKey(e => e.ReadByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.ProductId);
            entity.HasIndex(e => e.AlertType);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.AlertDate);
            entity.HasIndex(e => e.IsRead);
        });

        // Coupon entity yapılandırması
        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.DiscountType).IsRequired().HasMaxLength(20);
            entity.Property(e => e.DiscountValue).HasColumnType("decimal(18,2)");
            entity.Property(e => e.MinimumOrderAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.MaximumDiscountAmount).HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.StartDate);
            entity.HasIndex(e => e.EndDate);
        });

        // CouponUsage entity yapılandırması
        modelBuilder.Entity<CouponUsage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OrderAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.UserIpAddress).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(500);

            entity.HasOne(e => e.Coupon)
                .WithMany(c => c.CouponUsages)
                .HasForeignKey(e => e.CouponId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.CouponId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.OrderId);
            entity.HasIndex(e => e.UsedAt);
        });

        // CouponCategory entity yapılandırması
        modelBuilder.Entity<CouponCategory>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Coupon)
                .WithMany(c => c.CouponCategories)
                .HasForeignKey(e => e.CouponId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.CouponId);
            entity.HasIndex(e => e.CategoryId);
        });

        // CouponProduct entity yapılandırması
        modelBuilder.Entity<CouponProduct>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Coupon)
                .WithMany(c => c.CouponProducts)
                .HasForeignKey(e => e.CouponId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.CouponId);
            entity.HasIndex(e => e.ProductId);
        });

        // CouponUser entity yapılandırması
        modelBuilder.Entity<CouponUser>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Coupon)
                .WithMany(c => c.CouponUsers)
                .HasForeignKey(e => e.CouponId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.CouponId);
            entity.HasIndex(e => e.UserId);
        });

        // ProductReview entity yapılandırması
        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Content).HasMaxLength(2000);
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.ReviewType).HasMaxLength(20);
            entity.Property(e => e.UserIpAddress).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(500);

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.OrderItem)
                .WithMany()
                .HasForeignKey(e => e.OrderItemId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.ApprovedByUser)
                .WithMany()
                .HasForeignKey(e => e.ApprovedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.ProductId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Rating);
            entity.HasIndex(e => e.IsApproved);
            entity.HasIndex(e => e.CreatedAt);
        });

        // ReviewResponse entity yapılandırması
        modelBuilder.Entity<ReviewResponse>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.ResponseType).HasMaxLength(20);

            entity.HasOne(e => e.Review)
                .WithMany(r => r.ReviewResponses)
                .HasForeignKey(e => e.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.RespondedByUser)
                .WithMany()
                .HasForeignKey(e => e.RespondedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.ApprovedByUser)
                .WithMany()
                .HasForeignKey(e => e.ApprovedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.ReviewId);
            entity.HasIndex(e => e.RespondedByUserId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // ReviewVote entity yapılandırması
        modelBuilder.Entity<ReviewVote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VoteType).HasMaxLength(10);
            entity.Property(e => e.UserIpAddress).HasMaxLength(45);

            entity.HasOne(e => e.Review)
                .WithMany(r => r.ReviewVotes)
                .HasForeignKey(e => e.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.ReviewId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // ReviewImage entity yapılandırması
        modelBuilder.Entity<ReviewImage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.ContentType).HasMaxLength(50);

            entity.HasOne(e => e.Review)
                .WithMany(r => r.ReviewImages)
                .HasForeignKey(e => e.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.ReviewId);
            entity.HasIndex(e => e.SortOrder);
        });

        // Wishlist entity yapılandırması
        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ListType).HasMaxLength(20);
            entity.Property(e => e.ShareCode).HasMaxLength(50);
            entity.Property(e => e.Color).HasMaxLength(7);
            entity.Property(e => e.Icon).HasMaxLength(50);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.IsDefault);
            entity.HasIndex(e => e.ShareCode);
            entity.HasIndex(e => e.CreatedAt);
        });

        // WishlistItem entity yapılandırması
        modelBuilder.Entity<WishlistItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PriceAtTime).HasColumnType("decimal(18,2)");
            entity.Property(e => e.DiscountedPriceAtTime).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.TargetPrice).HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Wishlist)
                .WithMany(w => w.WishlistItems)
                .HasForeignKey(e => e.WishlistId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.WishlistId);
            entity.HasIndex(e => e.ProductId);
            entity.HasIndex(e => e.Priority);
            entity.HasIndex(e => e.CreatedAt);
        });

        // WishlistShare entity yapılandırması
        modelBuilder.Entity<WishlistShare>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ShareType).HasMaxLength(20);
            entity.Property(e => e.ShareCode).IsRequired().HasMaxLength(50);
            entity.Property(e => e.EmailAddress).HasMaxLength(255);
            entity.Property(e => e.Message).HasMaxLength(500);

            entity.HasOne(e => e.Wishlist)
                .WithMany(w => w.WishlistShares)
                .HasForeignKey(e => e.WishlistId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.SharedWithUser)
                .WithMany()
                .HasForeignKey(e => e.SharedWithUserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.WishlistId);
            entity.HasIndex(e => e.ShareCode);
            entity.HasIndex(e => e.SharedWithUserId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // WishlistItemPriceHistory entity yapılandırması
        modelBuilder.Entity<WishlistItemPriceHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OldPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.NewPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.PriceChangePercentage).HasColumnType("decimal(5,2)");
            entity.Property(e => e.ChangeType).HasMaxLength(10);
            entity.Property(e => e.ChangeReason).HasMaxLength(100);

            entity.HasOne(e => e.WishlistItem)
                .WithMany(wi => wi.PriceHistory)
                .HasForeignKey(e => e.WishlistItemId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.WishlistItemId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // WishlistItemStockHistory entity yapılandırması
        modelBuilder.Entity<WishlistItemStockHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ChangeType).HasMaxLength(20);
            entity.Property(e => e.ChangeReason).HasMaxLength(100);

            entity.HasOne(e => e.WishlistItem)
                .WithMany(wi => wi.StockHistory)
                .HasForeignKey(e => e.WishlistItemId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.WishlistItemId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // Notification entity yapılandırması
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Data).HasMaxLength(2000);
            entity.Property(e => e.RelatedEntityType).HasMaxLength(50);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.Priority);
            entity.HasIndex(e => e.IsRead);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.ExpiresAt);
        });

        // NotificationTemplate entity yapılandırması
        modelBuilder.Entity<NotificationTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TitleTemplate).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ContentTemplate).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Variables).HasMaxLength(1000);

            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.IsActive);
        });

        // Global query filters (soft delete)
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<UserRole>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ProductImage>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Address>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<OrderItem>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<OrderStatusHistory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Permission>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<RolePermission>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Cart>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<CartItem>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Payment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<EmailTemplate>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<EmailLog>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Inventory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<InventoryMovement>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<StockAlert>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Coupon>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<CouponUsage>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<CouponCategory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<CouponProduct>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<CouponUser>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ProductReview>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ReviewResponse>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ReviewVote>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ReviewImage>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Wishlist>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<WishlistItem>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<WishlistShare>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<WishlistItemPriceHistory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<WishlistItemStockHistory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Notification>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<NotificationTemplate>().HasQueryFilter(e => !e.IsDeleted);
    }
}
