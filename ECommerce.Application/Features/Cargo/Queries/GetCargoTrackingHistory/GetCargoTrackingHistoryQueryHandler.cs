using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Cargo.Queries.GetCargoTrackingHistory;
using ECommerce.Application.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Cargo.Queries.GetCargoTrackingHistory;

/// <summary>
/// Kargo takip geçmişi getirme sorgu işleyicisi
/// </summary>
public class GetCargoTrackingHistoryQueryHandler : IQueryHandler<GetCargoTrackingHistoryQuery, IEnumerable<CargoTrackingDto>>
{
    private readonly ICargoTrackingService _trackingService;
    private readonly ICargoService _cargoService;
    private readonly ILogger<GetCargoTrackingHistoryQueryHandler> _logger;

    public GetCargoTrackingHistoryQueryHandler(
        ICargoTrackingService trackingService,
        ICargoService cargoService,
        ILogger<GetCargoTrackingHistoryQueryHandler> logger)
    {
        _trackingService = trackingService;
        _cargoService = cargoService;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<CargoTrackingDto>>> Handle(GetCargoTrackingHistoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kargo takip geçmişi getiriliyor - Kargo ID: {CargoId}", request.CargoId);

            // Kargo var mı kontrol et
            var cargo = await _cargoService.GetCargoByIdAsync(request.CargoId, cancellationToken);
            if (cargo == null)
            {
                return Result.Failure<IEnumerable<CargoTrackingDto>>(Error.NotFound("Cargo.NotFound", "Kargo bulunamadı."));
            }

            var trackingHistory = await _trackingService.GetTrackingHistoryAsync(request.CargoId, cancellationToken);

            _logger.LogInformation("Kargo takip geçmişi başarıyla getirildi. Kargo ID: {CargoId}, Kayıt Sayısı: {Count}", 
                request.CargoId, trackingHistory.Count());

            return Result.Success(trackingHistory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo takip geçmişi getirilirken hata oluştu. Kargo ID: {CargoId}", request.CargoId);
            return Result.Failure<IEnumerable<CargoTrackingDto>>(Error.Problem("Cargo.TrackingHistoryError", "Kargo takip geçmişi alınırken bir hata oluştu."));
        }
    }
}
