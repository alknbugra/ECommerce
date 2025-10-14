using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Cargo.Queries.GetCargoByTrackingNumber;
using ECommerce.Application.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Cargo.Queries.GetCargoByTrackingNumber;

/// <summary>
/// Takip numarasına göre kargo getirme sorgu işleyicisi
/// </summary>
public class GetCargoByTrackingNumberQueryHandler : IQueryHandler<GetCargoByTrackingNumberQuery, CargoDto>
{
    private readonly ICargoService _cargoService;
    private readonly ILogger<GetCargoByTrackingNumberQueryHandler> _logger;

    public GetCargoByTrackingNumberQueryHandler(
        ICargoService cargoService,
        ILogger<GetCargoByTrackingNumberQueryHandler> logger)
    {
        _cargoService = cargoService;
        _logger = logger;
    }

    public async Task<Result<CargoDto>> Handle(GetCargoByTrackingNumberQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Takip numarasına göre kargo getiriliyor: {TrackingNumber}", request.TrackingNumber);

            if (string.IsNullOrWhiteSpace(request.TrackingNumber))
            {
                return Result.Failure<CargoDto>(Error.Validation("Cargo.InvalidTrackingNumber", "Takip numarası geçersiz."));
            }

            var cargo = await _cargoService.GetCargoByTrackingNumberAsync(request.TrackingNumber, cancellationToken);
            if (cargo == null)
            {
                return Result.Failure<CargoDto>(Error.NotFound("Cargo.NotFound", "Bu takip numarasına ait kargo bulunamadı."));
            }

            _logger.LogInformation("Kargo başarıyla getirildi. ID: {Id}, Takip Numarası: {TrackingNumber}", cargo.Id, cargo.TrackingNumber);

            return Result.Success(cargo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Takip numarasına göre kargo getirilirken hata oluştu. Takip Numarası: {TrackingNumber}", request.TrackingNumber);
            return Result.Failure<CargoDto>(Error.Problem("Cargo.RetrieveError", "Kargo bilgileri alınırken bir hata oluştu."));
        }
    }
}
