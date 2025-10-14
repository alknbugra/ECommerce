using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Cargo.Commands.UpdateCargoStatus;
using ECommerce.Application.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Cargo.Commands.UpdateCargoStatus;

/// <summary>
/// Kargo durumu güncelleme komut işleyicisi
/// </summary>
public class UpdateCargoStatusCommandHandler : ICommandHandler<UpdateCargoStatusCommand, CargoDto>
{
    private readonly ICargoService _cargoService;
    private readonly ICargoTrackingService _trackingService;
    private readonly ILogger<UpdateCargoStatusCommandHandler> _logger;

    public UpdateCargoStatusCommandHandler(
        ICargoService cargoService,
        ICargoTrackingService trackingService,
        ILogger<UpdateCargoStatusCommandHandler> logger)
    {
        _cargoService = cargoService;
        _trackingService = trackingService;
        _logger = logger;
    }

    public async Task<Result<CargoDto>> Handle(UpdateCargoStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kargo durumu güncelleniyor - ID: {Id}, Durum: {Status}", request.Id, request.Status);

            // Kargo var mı kontrol et
            var existingCargo = await _cargoService.GetCargoByIdAsync(request.Id, cancellationToken);
            if (existingCargo == null)
            {
                return Result.Failure<CargoDto>(Error.NotFound("Cargo.NotFound", "Kargo bulunamadı."));
            }

            // Durum güncelle
            var success = await _cargoService.UpdateCargoStatusAsync(request.Id, request.Status, request.Notes, cancellationToken);
            if (!success)
            {
                return Result.Failure<CargoDto>(Error.Problem("Cargo.UpdateFailed", "Kargo durumu güncellenemedi."));
            }

            // Takip durumu ekle
            var trackingDto = new CreateCargoTrackingDto
            {
                CargoId = request.Id,
                Status = request.Status,
                StatusDescription = GetStatusDescription(request.Status),
                Location = request.Location,
                Notes = request.Notes,
                Source = request.Source
            };

            await _trackingService.AddTrackingStatusAsync(trackingDto, cancellationToken);

            // Güncellenmiş kargo bilgilerini getir
            var updatedCargo = await _cargoService.GetCargoByIdAsync(request.Id, cancellationToken);
            if (updatedCargo == null)
            {
                return Result.Failure<CargoDto>(Error.Problem("Cargo.RetrieveFailed", "Güncellenmiş kargo bilgileri alınamadı."));
            }

            _logger.LogInformation("Kargo durumu başarıyla güncellendi. ID: {Id}, Yeni Durum: {Status}", request.Id, request.Status);

            return Result.Success(updatedCargo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo durumu güncellenirken hata oluştu. ID: {Id}", request.Id);
            return Result.Failure<CargoDto>(Error.Problem("Cargo.UpdateError", "Kargo durumu güncelleme sırasında bir hata oluştu."));
        }
    }

    private static string GetStatusDescription(Domain.Enums.CargoStatus status)
    {
        return status switch
        {
            Domain.Enums.CargoStatus.Created => "Kargo oluşturuldu",
            Domain.Enums.CargoStatus.Preparing => "Kargo hazırlanıyor",
            Domain.Enums.CargoStatus.PickedUp => "Kargo şirketine teslim edildi",
            Domain.Enums.CargoStatus.InTransit => "Kargo yolda",
            Domain.Enums.CargoStatus.AtDistributionCenter => "Kargo dağıtım merkezinde",
            Domain.Enums.CargoStatus.OutForDelivery => "Kargo teslim için hazır",
            Domain.Enums.CargoStatus.Delivered => "Kargo teslim edildi",
            Domain.Enums.CargoStatus.DeliveryFailed => "Kargo teslim edilemedi",
            Domain.Enums.CargoStatus.Returned => "Kargo iade edildi",
            Domain.Enums.CargoStatus.Lost => "Kargo kayboldu",
            Domain.Enums.CargoStatus.Damaged => "Kargo hasarlı",
            _ => "Bilinmeyen durum"
        };
    }
}
