using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Cargo.Commands.UpdateCargoStatus;

/// <summary>
/// Kargo durumu güncelleme komutu
/// </summary>
public class UpdateCargoStatusCommand : ICommand<CargoDto>
{
    /// <summary>
    /// Kargo ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Yeni kargo durumu
    /// </summary>
    public CargoStatus Status { get; set; }

    /// <summary>
    /// Durum değişikliği notları
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Lokasyon bilgisi
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Takip kaynağı
    /// </summary>
    public string? Source { get; set; } = "System";
}
