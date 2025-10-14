namespace ECommerce.Application.DTOs;

/// <summary>
/// Kargo şirketi DTO'su
/// </summary>
public class CargoCompanyDto
{
    /// <summary>
    /// Kargo şirketi ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kargo şirketi adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kargo şirketi kodu
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Kargo şirketi logosu URL'si
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Kargo şirketi web sitesi
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Kargo şirketi telefon numarası
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Kargo şirketi e-posta adresi
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Kargo şirketi adresi
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Kargo şirketi aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Kargo şirketi takip URL şablonu
    /// </summary>
    public string? TrackingUrlTemplate { get; set; }

    /// <summary>
    /// Kargo şirketi açıklaması
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Kargo şirketi oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Kargo şirketi güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Kargo şirketi oluşturma DTO'su
/// </summary>
public class CreateCargoCompanyDto
{
    /// <summary>
    /// Kargo şirketi adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kargo şirketi kodu
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Kargo şirketi logosu URL'si
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Kargo şirketi web sitesi
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Kargo şirketi telefon numarası
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Kargo şirketi e-posta adresi
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Kargo şirketi adresi
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Kargo şirketi API endpoint'i
    /// </summary>
    public string? ApiEndpoint { get; set; }

    /// <summary>
    /// Kargo şirketi API anahtarı
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Kargo şirketi takip URL şablonu
    /// </summary>
    public string? TrackingUrlTemplate { get; set; }

    /// <summary>
    /// Kargo şirketi açıklaması
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// Kargo şirketi güncelleme DTO'su
/// </summary>
public class UpdateCargoCompanyDto
{
    /// <summary>
    /// Kargo şirketi adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kargo şirketi kodu
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Kargo şirketi logosu URL'si
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Kargo şirketi web sitesi
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Kargo şirketi telefon numarası
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Kargo şirketi e-posta adresi
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Kargo şirketi adresi
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Kargo şirketi aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Kargo şirketi API endpoint'i
    /// </summary>
    public string? ApiEndpoint { get; set; }

    /// <summary>
    /// Kargo şirketi API anahtarı
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Kargo şirketi takip URL şablonu
    /// </summary>
    public string? TrackingUrlTemplate { get; set; }

    /// <summary>
    /// Kargo şirketi açıklaması
    /// </summary>
    public string? Description { get; set; }
}
