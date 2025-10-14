using ECommerce.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Inventory.Commands.StockIn;

/// <summary>
/// Stok girişi komut handler'ı
/// </summary>
public class StockInCommandHandler : ICommandHandler<StockInCommand, bool>
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<StockInCommandHandler> _logger;

    public StockInCommandHandler(
        IInventoryService inventoryService,
        ILogger<StockInCommandHandler> logger)
    {
        _inventoryService = inventoryService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(StockInCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stok girişi işlemi başlatıldı. ProductId: {ProductId}, Quantity: {Quantity}", 
            request.ProductId, request.Quantity);

        try
        {
            var result = await _inventoryService.StockInAsync(
                request.ProductId, 
                request.Quantity, 
                request.Reason, 
                request.UserId, 
                cancellationToken);

            _logger.LogInformation("Stok girişi işlemi tamamlandı. ProductId: {ProductId}, Success: {Success}", 
                request.ProductId, result);

            return Result.Success<bool>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok girişi işlemi sırasında hata oluştu. ProductId: {ProductId}", request.ProductId);
            return Result.Failure<bool>(Error.Problem("Inventory.StockInError", "Stok girişi işlemi sırasında bir hata oluştu."));
        }
    }
}
