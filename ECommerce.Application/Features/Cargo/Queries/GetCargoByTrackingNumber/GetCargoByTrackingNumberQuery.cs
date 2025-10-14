using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Cargo.Queries.GetCargoByTrackingNumber;

/// <summary>
/// Takip numarasına göre kargo getirme sorgusu
/// </summary>
public class GetCargoByTrackingNumberQuery : IQuery<CargoDto>
{
    /// <summary>
    /// Kargo takip numarası
    /// </summary>
    public string TrackingNumber { get; set; } = string.Empty;
}
