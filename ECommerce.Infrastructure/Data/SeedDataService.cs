using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Data;

/// <summary>
/// Veritabanı seed data servisi
/// </summary>
public class SeedDataService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SeedDataService> _logger;

    public SeedDataService(IServiceProvider serviceProvider, ILogger<SeedDataService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

        try
        {
            // Veritabanının oluşturulduğundan emin ol
            await context.Database.EnsureCreatedAsync(cancellationToken);

            // Migration'ları uygula
            await context.Database.MigrateAsync(cancellationToken);

            // Seed data'yı ekle
            await SeedDataAsync(context, cancellationToken);

            _logger.LogInformation("Seed data başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Seed data eklenirken hata oluştu.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task SeedDataAsync(ECommerceDbContext context, CancellationToken cancellationToken)
    {
        // Rolleri ekle
        await SeedRolesAsync(context, cancellationToken);

        // Yetkileri ekle
        await SeedPermissionsAsync(context, cancellationToken);

        // Kategorileri ekle
        await SeedCategoriesAsync(context, cancellationToken);

        // Admin kullanıcısını ekle
        await SeedAdminUserAsync(context, cancellationToken);

        // Örnek ürünleri ekle
        await SeedProductsAsync(context, cancellationToken);
    }

    private async Task SeedRolesAsync(ECommerceDbContext context, CancellationToken cancellationToken)
    {
        if (await context.Roles.AnyAsync(cancellationToken))
            return;

        var roles = new List<Role>
        {
            new() { Name = "Admin", Description = "Sistem yöneticisi" },
            new() { Name = "Customer", Description = "Müşteri" },
            new() { Name = "Seller", Description = "Satıcı" },
            new() { Name = "Moderator", Description = "Moderatör" }
        };

        context.Roles.AddRange(roles);
        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Count} rol eklendi.", roles.Count);
    }

    private async Task SeedPermissionsAsync(ECommerceDbContext context, CancellationToken cancellationToken)
    {
        if (await context.Permissions.AnyAsync(cancellationToken))
            return;

        var permissions = new List<Permission>
        {
            // User Management Permissions
            new() { Name = "Users.Read", Description = "Kullanıcıları görüntüleme", Category = "User", Action = "Read", Resource = "Users", IsActive = true },
            new() { Name = "Users.Create", Description = "Kullanıcı oluşturma", Category = "User", Action = "Create", Resource = "Users", IsActive = true },
            new() { Name = "Users.Update", Description = "Kullanıcı güncelleme", Category = "User", Action = "Update", Resource = "Users", IsActive = true },
            new() { Name = "Users.Delete", Description = "Kullanıcı silme", Category = "User", Action = "Delete", Resource = "Users", IsActive = true },
            new() { Name = "Users.Manage", Description = "Kullanıcı yönetimi", Category = "User", Action = "Manage", Resource = "Users", IsActive = true },

            // Role Management Permissions
            new() { Name = "Roles.Read", Description = "Rolleri görüntüleme", Category = "Role", Action = "Read", Resource = "Roles", IsActive = true },
            new() { Name = "Roles.Create", Description = "Rol oluşturma", Category = "Role", Action = "Create", Resource = "Roles", IsActive = true },
            new() { Name = "Roles.Update", Description = "Rol güncelleme", Category = "Role", Action = "Update", Resource = "Roles", IsActive = true },
            new() { Name = "Roles.Delete", Description = "Rol silme", Category = "Role", Action = "Delete", Resource = "Roles", IsActive = true },
            new() { Name = "Roles.Manage", Description = "Rol yönetimi", Category = "Role", Action = "Manage", Resource = "Roles", IsActive = true },

            // Permission Management Permissions
            new() { Name = "Permissions.Read", Description = "Yetkileri görüntüleme", Category = "Permission", Action = "Read", Resource = "Permissions", IsActive = true },
            new() { Name = "Permissions.Create", Description = "Yetki oluşturma", Category = "Permission", Action = "Create", Resource = "Permissions", IsActive = true },
            new() { Name = "Permissions.Update", Description = "Yetki güncelleme", Category = "Permission", Action = "Update", Resource = "Permissions", IsActive = true },
            new() { Name = "Permissions.Delete", Description = "Yetki silme", Category = "Permission", Action = "Delete", Resource = "Permissions", IsActive = true },
            new() { Name = "Permissions.Manage", Description = "Yetki yönetimi", Category = "Permission", Action = "Manage", Resource = "Permissions", IsActive = true },

            // Product Management Permissions
            new() { Name = "Products.Read", Description = "Ürünleri görüntüleme", Category = "Product", Action = "Read", Resource = "Products", IsActive = true },
            new() { Name = "Products.Create", Description = "Ürün oluşturma", Category = "Product", Action = "Create", Resource = "Products", IsActive = true },
            new() { Name = "Products.Update", Description = "Ürün güncelleme", Category = "Product", Action = "Update", Resource = "Products", IsActive = true },
            new() { Name = "Products.Delete", Description = "Ürün silme", Category = "Product", Action = "Delete", Resource = "Products", IsActive = true },
            new() { Name = "Products.Manage", Description = "Ürün yönetimi", Category = "Product", Action = "Manage", Resource = "Products", IsActive = true },

            // Category Management Permissions
            new() { Name = "Categories.Read", Description = "Kategorileri görüntüleme", Category = "Category", Action = "Read", Resource = "Categories", IsActive = true },
            new() { Name = "Categories.Create", Description = "Kategori oluşturma", Category = "Category", Action = "Create", Resource = "Categories", IsActive = true },
            new() { Name = "Categories.Update", Description = "Kategori güncelleme", Category = "Category", Action = "Update", Resource = "Categories", IsActive = true },
            new() { Name = "Categories.Delete", Description = "Kategori silme", Category = "Category", Action = "Delete", Resource = "Categories", IsActive = true },
            new() { Name = "Categories.Manage", Description = "Kategori yönetimi", Category = "Category", Action = "Manage", Resource = "Categories", IsActive = true },

            // Order Management Permissions
            new() { Name = "Orders.Read", Description = "Siparişleri görüntüleme", Category = "Order", Action = "Read", Resource = "Orders", IsActive = true },
            new() { Name = "Orders.Create", Description = "Sipariş oluşturma", Category = "Order", Action = "Create", Resource = "Orders", IsActive = true },
            new() { Name = "Orders.Update", Description = "Sipariş güncelleme", Category = "Order", Action = "Update", Resource = "Orders", IsActive = true },
            new() { Name = "Orders.Delete", Description = "Sipariş silme", Category = "Order", Action = "Delete", Resource = "Orders", IsActive = true },
            new() { Name = "Orders.Manage", Description = "Sipariş yönetimi", Category = "Order", Action = "Manage", Resource = "Orders", IsActive = true },
            new() { Name = "Orders.Approve", Description = "Sipariş onaylama", Category = "Order", Action = "Approve", Resource = "Orders", IsActive = true },

            // File Management Permissions
            new() { Name = "Files.Read", Description = "Dosyaları görüntüleme", Category = "File", Action = "Read", Resource = "Files", IsActive = true },
            new() { Name = "Files.Upload", Description = "Dosya yükleme", Category = "File", Action = "Create", Resource = "Files", IsActive = true },
            new() { Name = "Files.Delete", Description = "Dosya silme", Category = "File", Action = "Delete", Resource = "Files", IsActive = true },
            new() { Name = "Files.Manage", Description = "Dosya yönetimi", Category = "File", Action = "Manage", Resource = "Files", IsActive = true },

            // System Management Permissions
            new() { Name = "System.Read", Description = "Sistem bilgilerini görüntüleme", Category = "System", Action = "Read", Resource = "System", IsActive = true },
            new() { Name = "System.Configure", Description = "Sistem yapılandırma", Category = "System", Action = "Configure", Resource = "System", IsActive = true },
            new() { Name = "System.Manage", Description = "Sistem yönetimi", Category = "System", Action = "Manage", Resource = "System", IsActive = true },

            // Report Management Permissions
            new() { Name = "Reports.Read", Description = "Raporları görüntüleme", Category = "Report", Action = "Read", Resource = "Reports", IsActive = true },
            new() { Name = "Reports.Export", Description = "Rapor dışa aktarma", Category = "Report", Action = "Export", Resource = "Reports", IsActive = true },
            new() { Name = "Reports.Manage", Description = "Rapor yönetimi", Category = "Report", Action = "Manage", Resource = "Reports", IsActive = true },

            // Log Management Permissions
            new() { Name = "Logs.Read", Description = "Logları görüntüleme", Category = "Log", Action = "Read", Resource = "Logs", IsActive = true },
            new() { Name = "Logs.Manage", Description = "Log yönetimi", Category = "Log", Action = "Manage", Resource = "Logs", IsActive = true }
        };

        context.Permissions.AddRange(permissions);
        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Count} yetki eklendi.", permissions.Count);
    }

    private async Task SeedCategoriesAsync(ECommerceDbContext context, CancellationToken cancellationToken)
    {
        if (await context.Categories.AnyAsync(cancellationToken))
            return;

        var categories = new List<Category>
        {
            new() { Name = "Elektronik", Description = "Elektronik ürünler", IsActive = true, SortOrder = 1 },
            new() { Name = "Giyim", Description = "Giyim ve aksesuar", IsActive = true, SortOrder = 2 },
            new() { Name = "Ev & Yaşam", Description = "Ev ve yaşam ürünleri", IsActive = true, SortOrder = 3 },
            new() { Name = "Kitap", Description = "Kitap ve dergi", IsActive = true, SortOrder = 4 },
            new() { Name = "Spor", Description = "Spor ve outdoor", IsActive = true, SortOrder = 5 }
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Count} kategori eklendi.", categories.Count);
    }

    private async Task SeedAdminUserAsync(ECommerceDbContext context, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Email == "admin@ecommerce.com", cancellationToken))
            return;

        // Password service'i kullanarak şifreyi hash'le
        using var scope = _serviceProvider.CreateScope();
        var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        var adminUser = new User
        {
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@ecommerce.com",
            PasswordHash = passwordService.HashPassword("Admin123!"),
            PhoneNumber = "+905551234567",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            IsActive = true
        };

        context.Users.Add(adminUser);
        await context.SaveChangesAsync(cancellationToken);

        // Admin rolünü ata
        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin", cancellationToken);
        if (adminRole != null)
        {
            var userRole = new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id,
                AssignedDate = DateTime.UtcNow
            };

            context.UserRoles.Add(userRole);
            await context.SaveChangesAsync(cancellationToken);
        }

        _logger.LogInformation("Admin kullanıcısı oluşturuldu: {Email}", adminUser.Email);
    }

    private async Task SeedProductsAsync(ECommerceDbContext context, CancellationToken cancellationToken)
    {
        if (await context.Products.AnyAsync(cancellationToken))
            return;

        var electronicsCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Elektronik", cancellationToken);
        if (electronicsCategory == null)
            return;

        var products = new List<Product>
        {
            new()
            {
                Name = "iPhone 15 Pro",
                Description = "Apple iPhone 15 Pro - En yeni teknoloji ile donatılmış akıllı telefon",
                ShortDescription = "Apple'ın en yeni iPhone modeli",
                Sku = "IPHONE15PRO-128",
                Price = 45999.99m,
                StockQuantity = 50,
                MinStockLevel = 5,
                Weight = 187,
                Length = 14.67m,
                Width = 7.15m,
                Height = 0.83m,
                MainImageUrl = "https://example.com/images/iphone15pro.jpg",
                CategoryId = electronicsCategory.Id,
                IsActive = true
            },
            new()
            {
                Name = "Samsung Galaxy S24 Ultra",
                Description = "Samsung Galaxy S24 Ultra - AI destekli kamera sistemi",
                ShortDescription = "Samsung'un flagship telefonu",
                Sku = "GALAXYS24U-256",
                Price = 42999.99m,
                StockQuantity = 30,
                MinStockLevel = 5,
                Weight = 232,
                Length = 16.24m,
                Width = 7.9m,
                Height = 0.88m,
                MainImageUrl = "https://example.com/images/galaxys24ultra.jpg",
                CategoryId = electronicsCategory.Id,
                IsActive = true
            },
            new()
            {
                Name = "MacBook Pro M3",
                Description = "Apple MacBook Pro 14\" M3 çip - Profesyonel performans",
                ShortDescription = "Apple'ın en güçlü laptop'u",
                Sku = "MBP14-M3-512",
                Price = 89999.99m,
                StockQuantity = 20,
                MinStockLevel = 3,
                Weight = 1600,
                Length = 31.26m,
                Width = 22.12m,
                Height = 1.55m,
                MainImageUrl = "https://example.com/images/macbookprom3.jpg",
                CategoryId = electronicsCategory.Id,
                IsActive = true
            }
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Count} ürün eklendi.", products.Count);
    }
}
