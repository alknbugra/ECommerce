namespace ECommerce.Domain.Enums;

/// <summary>
/// Yetki kategorileri
/// </summary>
public enum PermissionCategory
{
    /// <summary>
    /// Kullanıcı yönetimi
    /// </summary>
    User = 1,

    /// <summary>
    /// Rol yönetimi
    /// </summary>
    Role = 2,

    /// <summary>
    /// Yetki yönetimi
    /// </summary>
    Permission = 3,

    /// <summary>
    /// Ürün yönetimi
    /// </summary>
    Product = 4,

    /// <summary>
    /// Kategori yönetimi
    /// </summary>
    Category = 5,

    /// <summary>
    /// Sipariş yönetimi
    /// </summary>
    Order = 6,

    /// <summary>
    /// Dosya yönetimi
    /// </summary>
    File = 7,

    /// <summary>
    /// Sistem yönetimi
    /// </summary>
    System = 8,

    /// <summary>
    /// Rapor yönetimi
    /// </summary>
    Report = 9,

    /// <summary>
    /// Log yönetimi
    /// </summary>
    Log = 10
}
