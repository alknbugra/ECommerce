namespace ECommerce.Application.DTOs;

/// <summary>
/// Adres DTO'su
/// </summary>
public class AddressDto
{
    /// <summary>
    /// Adres ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Adres başlığı
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Ad
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Soyad
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Şirket adı
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// Adres satırı 1
    /// </summary>
    public string AddressLine1 { get; set; } = string.Empty;

    /// <summary>
    /// Adres satırı 2
    /// </summary>
    public string? AddressLine2 { get; set; }

    /// <summary>
    /// Şehir
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// İl/İlçe
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Posta kodu
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Ülke
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Telefon numarası
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Varsayılan adres mi?
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Adres türü
    /// </summary>
    public string AddressType { get; set; } = string.Empty;

    /// <summary>
    /// Tam adı
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
