using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Kullanıcı entity'si
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// E-posta adresi
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Şifre hash'i
    /// </summary>
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Ad
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Soyad
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Telefon numarası
    /// </summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// E-posta doğrulandı mı?
    /// </summary>
    public bool EmailConfirmed { get; set; } = false;

    /// <summary>
    /// Telefon doğrulandı mı?
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; } = false;

    /// <summary>
    /// Hesap kilitli mi?
    /// </summary>
    public bool IsLocked { get; set; } = false;

    /// <summary>
    /// Kilitlenme tarihi
    /// </summary>
    public DateTime? LockoutEnd { get; set; }

    /// <summary>
    /// Başarısız giriş denemesi sayısı
    /// </summary>
    public int AccessFailedCount { get; set; } = 0;

    /// <summary>
    /// Kullanıcı aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Kullanıcı rolleri
    /// </summary>
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// Kullanıcının siparişleri
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    /// <summary>
    /// Kullanıcının adresleri
    /// </summary>
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    /// <summary>
    /// Tam adı döndürür
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
}
