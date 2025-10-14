using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Services.Cargo;

/// <summary>
/// Kargo servis implementasyonu
/// </summary>
public class CargoService : ICargoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CargoService> _logger;

    public CargoService(
        IUnitOfWork unitOfWork,
        ILogger<CargoService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CargoDto?> GetCargoByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var cargo = await _unitOfWork.Cargos.GetByIdAsync(id);
            if (cargo == null)
                return null;

            return MapToDto(cargo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo getirilirken hata oluştu. ID: {Id}", id);
            throw;
        }
    }

    public async Task<CargoDto?> GetCargoByTrackingNumberAsync(string trackingNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var cargo = await _unitOfWork.Cargos.GetByTrackingNumberAsync(trackingNumber);
            if (cargo == null)
                return null;

            return MapToDto(cargo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo getirilirken hata oluştu. Takip Numarası: {TrackingNumber}", trackingNumber);
            throw;
        }
    }

    public async Task<CargoDto?> GetCargoByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        try
        {
            var cargo = await _unitOfWork.Cargos.GetByOrderIdAsync(orderId);
            if (cargo == null)
                return null;

            return MapToDto(cargo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo getirilirken hata oluştu. Sipariş ID: {OrderId}", orderId);
            throw;
        }
    }

    public async Task<IEnumerable<CargoDto>> GetCargosByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var cargos = await _unitOfWork.Cargos.GetByUserIdAsync(userId);
            return cargos.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı kargoları getirilirken hata oluştu. Kullanıcı ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<IEnumerable<CargoDto>> GetCargosByStatusAsync(CargoStatus status, CancellationToken cancellationToken = default)
    {
        try
        {
            var cargos = await _unitOfWork.Cargos.GetByStatusAsync(status);
            return cargos.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Duruma göre kargolar getirilirken hata oluştu. Durum: {Status}", status);
            throw;
        }
    }

    public async Task<CargoDto> CreateCargoAsync(CreateCargoDto createCargoDto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Siparişin var olup olmadığını kontrol et
            var order = await _unitOfWork.Orders.GetByIdAsync(createCargoDto.OrderId);
            if (order == null)
                throw new ArgumentException("Sipariş bulunamadı.");

            // Kargo şirketinin var olup olmadığını kontrol et
            var cargoCompany = await _unitOfWork.CargoCompanies.GetByIdAsync(createCargoDto.CargoCompanyId);
            if (cargoCompany == null)
                throw new ArgumentException("Kargo şirketi bulunamadı.");

            // Takip numarası oluştur
            var trackingNumber = await GenerateTrackingNumberAsync();

            var cargo = new Domain.Entities.Cargo
            {
                TrackingNumber = trackingNumber,
                OrderId = createCargoDto.OrderId,
                CargoCompanyId = createCargoDto.CargoCompanyId,
                Status = CargoStatus.Created,
                Type = createCargoDto.Type,
                Weight = createCargoDto.Weight,
                Dimensions = createCargoDto.Dimensions,
                ShippingCost = createCargoDto.ShippingCost,
                CustomerShippingCost = createCargoDto.CustomerShippingCost,
                ContentDescription = createCargoDto.ContentDescription,
                DeclaredValue = createCargoDto.DeclaredValue,
                EstimatedDeliveryDate = createCargoDto.EstimatedDeliveryDate,
                SenderName = createCargoDto.Sender.Name,
                SenderPhone = createCargoDto.Sender.Phone,
                SenderAddress = createCargoDto.Sender.Address,
                ReceiverName = createCargoDto.Receiver.Name,
                ReceiverPhone = createCargoDto.Receiver.Phone,
                ReceiverAddress = createCargoDto.Receiver.Address,
                Notes = createCargoDto.Notes,
                CompanyReferenceNumber = createCargoDto.CompanyReferenceNumber,
                InsuranceAmount = createCargoDto.InsuranceAmount,
                SpecialInstructions = createCargoDto.SpecialInstructions
            };

            await _unitOfWork.Cargos.AddAsync(cargo);

            // İlk takip durumunu ekle
            await _unitOfWork.CargoTrackings.AddTrackingStatusAsync(
                cargo.Id,
                CargoStatus.Created,
                "Kargo oluşturuldu",
                source: "System"
            );

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo oluşturuldu. ID: {Id}, Takip Numarası: {TrackingNumber}", cargo.Id, trackingNumber);

            return MapToDto(cargo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo oluşturulurken hata oluştu");
            throw;
        }
    }

    public async Task<CargoDto> UpdateCargoAsync(Guid id, UpdateCargoDto updateCargoDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var cargo = await _unitOfWork.Cargos.GetByIdAsync(id);
            if (cargo == null)
                throw new ArgumentException("Kargo bulunamadı.");

            // Kargo şirketinin var olup olmadığını kontrol et
            var cargoCompany = await _unitOfWork.CargoCompanies.GetByIdAsync(updateCargoDto.CargoCompanyId);
            if (cargoCompany == null)
                throw new ArgumentException("Kargo şirketi bulunamadı.");

            cargo.CargoCompanyId = updateCargoDto.CargoCompanyId;
            cargo.Status = updateCargoDto.Status;
            cargo.Type = updateCargoDto.Type;
            cargo.Weight = updateCargoDto.Weight;
            cargo.Dimensions = updateCargoDto.Dimensions;
            cargo.ShippingCost = updateCargoDto.ShippingCost;
            cargo.CustomerShippingCost = updateCargoDto.CustomerShippingCost;
            cargo.ContentDescription = updateCargoDto.ContentDescription;
            cargo.DeclaredValue = updateCargoDto.DeclaredValue;
            cargo.ShippedDate = updateCargoDto.ShippedDate;
            cargo.DeliveredDate = updateCargoDto.DeliveredDate;
            cargo.EstimatedDeliveryDate = updateCargoDto.EstimatedDeliveryDate;
            cargo.SenderName = updateCargoDto.Sender.Name;
            cargo.SenderPhone = updateCargoDto.Sender.Phone;
            cargo.SenderAddress = updateCargoDto.Sender.Address;
            cargo.ReceiverName = updateCargoDto.Receiver.Name;
            cargo.ReceiverPhone = updateCargoDto.Receiver.Phone;
            cargo.ReceiverAddress = updateCargoDto.Receiver.Address;
            cargo.Notes = updateCargoDto.Notes;
            cargo.CompanyReferenceNumber = updateCargoDto.CompanyReferenceNumber;
            cargo.TrackingUrl = updateCargoDto.TrackingUrl;
            cargo.InsuranceAmount = updateCargoDto.InsuranceAmount;
            cargo.SpecialInstructions = updateCargoDto.SpecialInstructions;
            cargo.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Cargos.UpdateAsync(cargo);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo güncellendi. ID: {Id}", id);

            return MapToDto(cargo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo güncellenirken hata oluştu. ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> UpdateCargoStatusAsync(Guid id, CargoStatus status, string? notes = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var cargo = await _unitOfWork.Cargos.UpdateStatusAsync(id, status, notes);
            if (cargo == null)
                return false;

            // Takip durumunu güncelle
            await _unitOfWork.CargoTrackings.AddTrackingStatusAsync(
                id,
                status,
                GetStatusDescription(status),
                notes: notes,
                source: "System"
            );

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo durumu güncellendi. ID: {Id}, Durum: {Status}", id, status);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo durumu güncellenirken hata oluştu. ID: {Id}, Durum: {Status}", id, status);
            throw;
        }
    }

    public async Task<bool> DeleteCargoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var cargo = await _unitOfWork.Cargos.DeleteByIdAsync(id);
            if (cargo == null)
                return false;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Kargo silindi. ID: {Id}", id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo silinirken hata oluştu. ID: {Id}", id);
            throw;
        }
    }

    public async Task<CargoStatisticsDto> GetCargoStatisticsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            var statistics = await _unitOfWork.Cargos.GetStatisticsAsync(startDate, endDate);
            return new CargoStatisticsDto
            {
                TotalCargos = statistics.TotalCargos,
                StatusCounts = statistics.StatusCounts,
                CompanyCounts = statistics.CompanyCounts,
                TotalShippingCost = statistics.TotalShippingCost,
                AverageDeliveryDays = statistics.AverageDeliveryDays
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo istatistikleri getirilirken hata oluştu");
            throw;
        }
    }

    private async Task<string> GenerateTrackingNumberAsync()
    {
        string trackingNumber;
        do
        {
            trackingNumber = $"TRK{DateTime.UtcNow:yyyyMMdd}{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        } while (await _unitOfWork.Cargos.TrackingNumberExistsAsync(trackingNumber));

        return trackingNumber;
    }

    private static CargoDto MapToDto(Domain.Entities.Cargo cargo)
    {
        return new CargoDto
        {
            Id = cargo.Id,
            TrackingNumber = cargo.TrackingNumber,
            OrderId = cargo.OrderId,
            OrderNumber = cargo.Order?.OrderNumber ?? string.Empty,
            CargoCompanyId = cargo.CargoCompanyId,
            CargoCompanyName = cargo.CargoCompany?.Name ?? string.Empty,
            CargoCompanyCode = cargo.CargoCompany?.Code ?? string.Empty,
            Status = cargo.Status,
            StatusDescription = GetStatusDescription(cargo.Status),
            Type = cargo.Type,
            TypeDescription = GetTypeDescription(cargo.Type),
            Weight = cargo.Weight,
            Dimensions = cargo.Dimensions,
            ShippingCost = cargo.ShippingCost,
            CustomerShippingCost = cargo.CustomerShippingCost,
            ContentDescription = cargo.ContentDescription,
            DeclaredValue = cargo.DeclaredValue,
            ShippedDate = cargo.ShippedDate,
            DeliveredDate = cargo.DeliveredDate,
            EstimatedDeliveryDate = cargo.EstimatedDeliveryDate,
            Sender = new CargoSenderDto
            {
                Name = cargo.SenderName,
                Phone = cargo.SenderPhone,
                Address = cargo.SenderAddress
            },
            Receiver = new CargoReceiverDto
            {
                Name = cargo.ReceiverName,
                Phone = cargo.ReceiverPhone,
                Address = cargo.ReceiverAddress
            },
            Notes = cargo.Notes,
            CompanyReferenceNumber = cargo.CompanyReferenceNumber,
            TrackingUrl = cargo.TrackingUrl,
            InsuranceAmount = cargo.InsuranceAmount,
            SpecialInstructions = cargo.SpecialInstructions,
            CreatedAt = cargo.CreatedAt,
            UpdatedAt = cargo.UpdatedAt,
            TrackingHistory = cargo.TrackingHistory?.Select(MapTrackingToDto).ToList() ?? new List<CargoTrackingDto>()
        };
    }

    private static CargoTrackingDto MapTrackingToDto(Domain.Entities.CargoTracking tracking)
    {
        return new CargoTrackingDto
        {
            Id = tracking.Id,
            CargoId = tracking.CargoId,
            TrackingNumber = tracking.Cargo?.TrackingNumber ?? string.Empty,
            Status = tracking.Status,
            StatusDescription = tracking.StatusDescription,
            Location = tracking.Location,
            TrackingDate = tracking.TrackingDate,
            Notes = tracking.Notes,
            Source = tracking.Source,
            TrackingData = tracking.TrackingData,
            IsCurrent = tracking.IsCurrent,
            LastUpdated = tracking.LastUpdated,
            CreatedAt = tracking.CreatedAt
        };
    }

    private static string GetStatusDescription(CargoStatus status)
    {
        return status switch
        {
            CargoStatus.Created => "Kargo oluşturuldu",
            CargoStatus.Preparing => "Kargo hazırlanıyor",
            CargoStatus.PickedUp => "Kargo şirketine teslim edildi",
            CargoStatus.InTransit => "Kargo yolda",
            CargoStatus.AtDistributionCenter => "Kargo dağıtım merkezinde",
            CargoStatus.OutForDelivery => "Kargo teslim için hazır",
            CargoStatus.Delivered => "Kargo teslim edildi",
            CargoStatus.DeliveryFailed => "Kargo teslim edilemedi",
            CargoStatus.Returned => "Kargo iade edildi",
            CargoStatus.Lost => "Kargo kayboldu",
            CargoStatus.Damaged => "Kargo hasarlı",
            _ => "Bilinmeyen durum"
        };
    }

    private static string GetTypeDescription(CargoType type)
    {
        return type switch
        {
            CargoType.Standard => "Standart kargo",
            CargoType.Express => "Hızlı kargo",
            CargoType.SameDay => "Aynı gün teslimat",
            CargoType.Special => "Özel kargo",
            CargoType.ColdChain => "Soğuk zincir kargo",
            CargoType.Hazardous => "Tehlikeli madde kargo",
            CargoType.Oversized => "Büyük eşya kargo",
            _ => "Bilinmeyen tür"
        };
    }
}
