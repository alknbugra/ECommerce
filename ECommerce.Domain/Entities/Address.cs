using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Adres entity'si
/// </summary>
public class Address : BaseEntity
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Adres başlığı
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

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
    /// Şirket adı
    /// </summary>
    [MaxLength(100)]
    public string? CompanyName { get; set; }

    /// <summary>
    /// Adres satırı 1
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string AddressLine1 { get; set; } = string.Empty;

    /// <summary>
    /// Adres satırı 2
    /// </summary>
    [MaxLength(200)]
    public string? AddressLine2 { get; set; }

    /// <summary>
    /// Şehir
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// İl/İlçe
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Posta kodu
    /// </summary>
    [MaxLength(20)]
    public string? PostalCode { get; set; }

    /// <summary>
    /// Ülke
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Telefon numarası
    /// </summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Varsayılan adres mi?
    /// </summary>
    public bool IsDefault { get; set; } = false;

    /// <summary>
    /// Adres türü (Ev, İş, Diğer)
    /// </summary>
    [MaxLength(20)]
    public string AddressType { get; set; } = "Home";

    /// <summary>
    /// Kullanıcı
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Bu adrese gönderilen siparişler
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    /// <summary>
    /// Tam adı döndürür
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
}
