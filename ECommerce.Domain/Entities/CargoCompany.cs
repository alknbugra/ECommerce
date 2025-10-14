using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Kargo şirketi entity'si
/// </summary>
public class CargoCompany : BaseEntity
{
    /// <summary>
    /// Kargo şirketi adı
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kargo şirketi kodu
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Kargo şirketi logosu URL'si
    /// </summary>
    [MaxLength(500)]
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Kargo şirketi web sitesi
    /// </summary>
    [MaxLength(200)]
    public string? Website { get; set; }

    /// <summary>
    /// Kargo şirketi telefon numarası
    /// </summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Kargo şirketi e-posta adresi
    /// </summary>
    [MaxLength(100)]
    public string? Email { get; set; }

    /// <summary>
    /// Kargo şirketi adresi
    /// </summary>
    [MaxLength(500)]
    public string? Address { get; set; }

    /// <summary>
    /// Kargo şirketi aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Kargo şirketi API endpoint'i
    /// </summary>
    [MaxLength(200)]
    public string? ApiEndpoint { get; set; }

    /// <summary>
    /// Kargo şirketi API anahtarı
    /// </summary>
    [MaxLength(200)]
    public string? ApiKey { get; set; }

    /// <summary>
    /// Kargo şirketi takip URL şablonu
    /// </summary>
    [MaxLength(300)]
    public string? TrackingUrlTemplate { get; set; }

    /// <summary>
    /// Kargo şirketi açıklaması
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Kargo şirketi ile ilgili kargolar
    /// </summary>
    public virtual ICollection<Cargo> Cargos { get; set; } = new List<Cargo>();
}
