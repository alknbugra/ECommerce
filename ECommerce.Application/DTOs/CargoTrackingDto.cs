using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs;

/// <summary>
/// Kargo takip DTO'su
/// </summary>
public class CargoTrackingDto
{
    /// <summary>
    /// Takip ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kargo ID'si
    /// </summary>
    public Guid CargoId { get; set; }

    /// <summary>
    /// Kargo takip numarası
    /// </summary>
    public string TrackingNumber { get; set; } = string.Empty;

    /// <summary>
    /// Takip durumu
    /// </summary>
    public CargoStatus Status { get; set; }

    /// <summary>
    /// Takip durumu açıklaması
    /// </summary>
    public string StatusDescription { get; set; } = string.Empty;

    /// <summary>
    /// Takip lokasyonu
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Takip tarihi
    /// </summary>
    public DateTime TrackingDate { get; set; }

    /// <summary>
    /// Takip notları
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Takip kaynağı
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Takip verisi
    /// </summary>
    public string? TrackingData { get; set; }

    /// <summary>
    /// Takip durumu güncel mi?
    /// </summary>
    public bool IsCurrent { get; set; }

    /// <summary>
    /// Takip durumu güncellenme tarihi
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Takip oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Kargo takip oluşturma DTO'su
/// </summary>
public class CreateCargoTrackingDto
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
    public string StatusDescription { get; set; } = string.Empty;

    /// <summary>
    /// Takip lokasyonu
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Takip tarihi
    /// </summary>
    public DateTime? TrackingDate { get; set; }

    /// <summary>
    /// Takip notları
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Takip kaynağı
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Takip verisi
    /// </summary>
    public string? TrackingData { get; set; }
}

/// <summary>
/// Kargo takip güncelleme DTO'su
/// </summary>
public class UpdateCargoTrackingDto
{
    /// <summary>
    /// Takip durumu
    /// </summary>
    public CargoStatus Status { get; set; }

    /// <summary>
    /// Takip durumu açıklaması
    /// </summary>
    public string StatusDescription { get; set; } = string.Empty;

    /// <summary>
    /// Takip lokasyonu
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Takip tarihi
    /// </summary>
    public DateTime? TrackingDate { get; set; }

    /// <summary>
    /// Takip notları
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Takip kaynağı
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Takip verisi
    /// </summary>
    public string? TrackingData { get; set; }
}
