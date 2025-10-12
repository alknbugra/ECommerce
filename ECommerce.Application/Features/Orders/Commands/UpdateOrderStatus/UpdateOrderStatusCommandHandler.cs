using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Orders.Commands.UpdateOrderStatus;

/// <summary>
/// Sipariş durumu güncelleme komut handler'ı
/// </summary>
public class UpdateOrderStatusCommandHandler : ICommandHandler<UpdateOrderStatusCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateOrderStatusCommandHandler> _logger;

    public UpdateOrderStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateOrderStatusCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<OrderDto> HandleAsync(UpdateOrderStatusCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sipariş durumu güncelleniyor: {OrderId} -> {NewStatus}", request.OrderId, request.NewStatus);

        // Siparişi bul
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException("Sipariş bulunamadı.");
        }

        // Geçerli durum kontrolü
        var validStatuses = new[] { "Pending", "Confirmed", "Processing", "Shipped", "Delivered", "Cancelled", "Returned" };
        if (!validStatuses.Contains(request.NewStatus))
        {
            throw new BadRequestException($"Geçersiz sipariş durumu: {request.NewStatus}");
        }

        // Durum geçiş kontrolü
        if (!IsValidStatusTransition(order.Status, request.NewStatus))
        {
            throw new BadRequestException($"Geçersiz durum geçişi: {order.Status} -> {request.NewStatus}");
        }

        var previousStatus = order.Status;

        // Sipariş durumunu güncelle
        order.Status = request.NewStatus;
        order.UpdatedAt = DateTime.UtcNow;

        // Duruma göre özel alanları güncelle
        switch (request.NewStatus)
        {
            case "Shipped":
                order.ShippedDate = DateTime.UtcNow;
                if (!string.IsNullOrWhiteSpace(request.TrackingNumber))
                    order.TrackingNumber = request.TrackingNumber;
                if (!string.IsNullOrWhiteSpace(request.ShippingCompany))
                    order.ShippingCompany = request.ShippingCompany;
                break;
            case "Delivered":
                order.DeliveredDate = DateTime.UtcNow;
                break;
            case "Cancelled":
                // İptal edilen sipariş için stokları geri ver
                await RestoreProductStock(order.Id);
                break;
        }

        await _unitOfWork.Orders.UpdateAsync(order);

        // Durum geçmişini ekle
        var statusHistory = new OrderStatusHistory
        {
            OrderId = order.Id,
            PreviousStatus = previousStatus,
            NewStatus = request.NewStatus,
            StatusChangeDate = DateTime.UtcNow,
            Notes = request.Notes,
            ChangedByUserId = request.ChangedByUserId
        };

        await _unitOfWork.OrderStatusHistories.AddAsync(statusHistory);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Sipariş durumu başarıyla güncellendi: {OrderNumber} -> {NewStatus}", order.OrderNumber, request.NewStatus);

        // DTO'ya dönüştür (basitleştirilmiş)
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            UserId = order.UserId,
            Status = order.Status,
            PaymentStatus = order.PaymentStatus,
            PaymentMethod = order.PaymentMethod,
            SubTotal = order.SubTotal,
            ShippingCost = order.ShippingCost,
            TaxAmount = order.TaxAmount,
            DiscountAmount = order.DiscountAmount,
            TotalAmount = order.TotalAmount,
            TrackingNumber = order.TrackingNumber,
            ShippingCompany = order.ShippingCompany,
            Notes = order.Notes,
            OrderDate = order.OrderDate,
            ShippedDate = order.ShippedDate,
            DeliveredDate = order.DeliveredDate
        };
    }

    /// <summary>
    /// Geçerli durum geçişi kontrolü
    /// </summary>
    private static bool IsValidStatusTransition(string currentStatus, string newStatus)
    {
        var validTransitions = new Dictionary<string, string[]>
        {
            ["Pending"] = new[] { "Confirmed", "Cancelled" },
            ["Confirmed"] = new[] { "Processing", "Cancelled" },
            ["Processing"] = new[] { "Shipped", "Cancelled" },
            ["Shipped"] = new[] { "Delivered", "Returned" },
            ["Delivered"] = new[] { "Returned" },
            ["Cancelled"] = new string[0], // İptal edilen sipariş durumu değiştirilemez
            ["Returned"] = new string[0]   // İade edilen sipariş durumu değiştirilemez
        };

        return validTransitions.ContainsKey(currentStatus) && 
               validTransitions[currentStatus].Contains(newStatus);
    }

    /// <summary>
    /// İptal edilen sipariş için ürün stoklarını geri ver
    /// </summary>
    private async Task RestoreProductStock(Guid orderId)
    {
        var orderItems = await _unitOfWork.OrderItems.FindAsync(oi => oi.OrderId == orderId);
        
        foreach (var orderItem in orderItems)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(orderItem.ProductId);
            if (product != null)
            {
                product.StockQuantity += orderItem.Quantity;
                await _unitOfWork.Products.UpdateAsync(product);
            }
        }
    }
}
