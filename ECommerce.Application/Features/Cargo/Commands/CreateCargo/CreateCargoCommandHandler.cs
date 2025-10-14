using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Cargo.Commands.CreateCargo;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Cargo.Commands.CreateCargo;

/// <summary>
/// Kargo oluşturma komut işleyicisi
/// </summary>
public class CreateCargoCommandHandler : ICommandHandler<CreateCargoCommand, CargoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICargoService _cargoService;
    private readonly ILogger<CreateCargoCommandHandler> _logger;

    public CreateCargoCommandHandler(
        IUnitOfWork unitOfWork,
        ICargoService cargoService,
        ILogger<CreateCargoCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _cargoService = cargoService;
        _logger = logger;
    }

    public async Task<Result<CargoDto>> Handle(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Yeni kargo oluşturuluyor - Sipariş: {OrderId}", request.OrderId);

            // Sipariş kontrolü
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                return Result.Failure<CargoDto>(Error.NotFound("Cargo.OrderNotFound", "Sipariş bulunamadı."));
            }

            // Kargo şirketi kontrolü
            var cargoCompany = await _unitOfWork.CargoCompanies.GetByIdAsync(request.CargoCompanyId);
            if (cargoCompany == null)
            {
                return Result.Failure<CargoDto>(Error.NotFound("Cargo.CompanyNotFound", "Kargo şirketi bulunamadı."));
            }

            if (!cargoCompany.IsActive)
            {
                return Result.Failure<CargoDto>(Error.Validation("Cargo.CompanyInactive", "Kargo şirketi aktif değil."));
            }

            var createDto = new CreateCargoDto
            {
                OrderId = request.OrderId,
                CargoCompanyId = request.CargoCompanyId,
                Type = request.Type,
                Weight = request.Weight,
                Dimensions = request.Dimensions,
                ShippingCost = request.ShippingCost,
                CustomerShippingCost = request.CustomerShippingCost,
                ContentDescription = request.ContentDescription,
                DeclaredValue = request.DeclaredValue,
                EstimatedDeliveryDate = request.EstimatedDeliveryDate,
                Sender = request.Sender,
                Receiver = request.Receiver,
                Notes = request.Notes,
                CompanyReferenceNumber = request.CompanyReferenceNumber,
                InsuranceAmount = request.InsuranceAmount,
                SpecialInstructions = request.SpecialInstructions
            };

            var result = await _cargoService.CreateCargoAsync(createDto, cancellationToken);

            _logger.LogInformation("Kargo başarıyla oluşturuldu. ID: {Id}, Takip Numarası: {TrackingNumber}", result.Id, result.TrackingNumber);

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kargo oluşturulurken hata oluştu. Sipariş ID: {OrderId}", request.OrderId);
            return Result.Failure<CargoDto>(Error.Problem("Cargo.CreateError", "Kargo oluşturma sırasında bir hata oluştu."));
        }
    }
}
