using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Cargo.Queries.GetCargoTrackingHistory;

/// <summary>
/// Kargo takip geçmişi getirme sorgusu
/// </summary>
public class GetCargoTrackingHistoryQuery : IQuery<IEnumerable<CargoTrackingDto>>
{
    /// <summary>
    /// Kargo ID'si
    /// </summary>
    public Guid CargoId { get; set; }
}
