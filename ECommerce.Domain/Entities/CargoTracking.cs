using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Kargo takip geçmişi entity'si
/// </summary>
public class CargoTracking : BaseEntity
{
    /// <summary>
    /// Kargo ID'si
    /// </summary>
    public Guid CargoId { get; set; }

    /// <summary>
    /// Takip durumu
    /// </summary>
    public CargoStatus Status { get; set; }

    /// <summary>
    /// Takip durumu açıklaması
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string StatusDescription { get; set; } = string.Empty;

    /// <summary>
    /// Takip lokasyonu
    /// </summary>
    [MaxLength(200)]
    public string? Location { get; set; }

    /// <summary>
    /// Takip tarihi
    /// </summary>
    public DateTime TrackingDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Takip notları
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Takip kaynağı (API, Manuel, vs.)
    /// </summary>
    [MaxLength(50)]
    public string? Source { get; set; }

    /// <summary>
    /// Takip verisi (JSON formatında ek bilgiler)
    /// </summary>
    [MaxLength(2000)]
    public string? TrackingData { get; set; }

    /// <summary>
    /// Takip durumu güncel mi?
    /// </summary>
    public bool IsCurrent { get; set; } = true;

    /// <summary>
    /// Takip durumu güncellenme tarihi
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// İlgili kargo
    /// </summary>
    public virtual Cargo Cargo { get; set; } = null!;
}
