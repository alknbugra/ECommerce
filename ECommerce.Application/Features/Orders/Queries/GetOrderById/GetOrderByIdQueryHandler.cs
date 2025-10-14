using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Orders.Queries.GetOrderById;

/// <summary>
/// ID'ye göre sipariş getirme sorgu handler'ı
/// </summary>
public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetOrderByIdQueryHandler> _logger;

    public GetOrderByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetOrderByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sipariş getiriliyor: {Id}", request.Id);

            var order = await _unitOfWork.Orders.GetByIdAsync(request.Id);
            if (order == null)
            {
                _logger.LogWarning("Sipariş bulunamadı: {Id}", request.Id);
                return Result.Failure<OrderDto>(Error.NotFound("Order.NotFound", "Sipariş bulunamadı."));
            }

            // Kullanıcı kontrolü (sadece kendi siparişini görebilir veya admin)
            if (request.UserId.HasValue && order.UserId != request.UserId.Value)
            {
                _logger.LogWarning("Sipariş erişim yetkisi yok: {Id}, UserId: {UserId}", request.Id, request.UserId);
                return Result.Failure<OrderDto>(Error.Problem("Order.AccessDenied", "Bu siparişe erişim yetkiniz yok."));
            }

        // Kullanıcı bilgilerini al
        var user = await _unitOfWork.Users.GetByIdAsync(order.UserId);
        if (user == null)
        {
            throw new NotFoundException("Sipariş sahibi bulunamadı.");
        }

        // Teslimat adresini al
        var shippingAddress = await _unitOfWork.Addresses.GetByIdAsync(order.ShippingAddressId);
        if (shippingAddress == null)
        {
            throw new NotFoundException("Teslimat adresi bulunamadı.");
        }

        // Fatura adresini al
        AddressDto? billingAddressDto = null;
        if (order.BillingAddressId.HasValue)
        {
            var billingAddress = await _unitOfWork.Addresses.GetByIdAsync(order.BillingAddressId.Value);
            if (billingAddress != null)
            {
                billingAddressDto = new AddressDto
                {
                    Id = billingAddress.Id,
                    Title = billingAddress.Title,
                    FirstName = billingAddress.FirstName,
                    LastName = billingAddress.LastName,
                    CompanyName = billingAddress.CompanyName,
                    AddressLine1 = billingAddress.AddressLine1,
                    AddressLine2 = billingAddress.AddressLine2,
                    City = billingAddress.City,
                    State = billingAddress.State,
                    PostalCode = billingAddress.PostalCode,
                    Country = billingAddress.Country,
                    PhoneNumber = billingAddress.PhoneNumber,
                    IsDefault = billingAddress.IsDefault,
                    AddressType = billingAddress.AddressType,
                    CreatedAt = billingAddress.CreatedAt
                };
            }
        }

        // Sipariş detaylarını al
        var orderItems = await _unitOfWork.OrderItems.FindAsync(oi => oi.OrderId == order.Id);
        var orderItemDtos = orderItems.Select(oi => new OrderItemDto
        {
            Id = oi.Id,
            OrderId = oi.OrderId,
            ProductId = oi.ProductId,
            ProductName = oi.ProductName,
            ProductSku = oi.ProductSku,
            Quantity = oi.Quantity,
            UnitPrice = oi.UnitPrice,
            TotalPrice = oi.TotalPrice,
            DiscountAmount = oi.DiscountAmount,
            ProductImageUrl = oi.ProductImageUrl
        }).ToList();

        // Sipariş durumu geçmişini al
        var statusHistory = await _unitOfWork.OrderStatusHistories.FindAsync(osh => osh.OrderId == order.Id);
        var statusHistoryDtos = statusHistory.Select(osh => new OrderStatusHistoryDto
        {
            Id = osh.Id,
            OrderId = osh.OrderId,
            PreviousStatus = osh.PreviousStatus,
            NewStatus = osh.NewStatus,
            StatusChangeDate = osh.StatusChangeDate,
            Notes = osh.Notes,
            ChangedByUserId = osh.ChangedByUserId
        }).OrderBy(osh => osh.StatusChangeDate).ToList();

        _logger.LogInformation("Sipariş başarıyla getirildi: {OrderNumber} (ID: {Id})", order.OrderNumber, request.Id);

        var orderDto = new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            UserId = order.UserId,
            UserName = user.FullName,
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
            DeliveredDate = order.DeliveredDate,
            ShippingAddress = new AddressDto
            {
                Id = shippingAddress.Id,
                Title = shippingAddress.Title,
                FirstName = shippingAddress.FirstName,
                LastName = shippingAddress.LastName,
                CompanyName = shippingAddress.CompanyName,
                AddressLine1 = shippingAddress.AddressLine1,
                AddressLine2 = shippingAddress.AddressLine2,
                City = shippingAddress.City,
                State = shippingAddress.State,
                PostalCode = shippingAddress.PostalCode,
                Country = shippingAddress.Country,
                PhoneNumber = shippingAddress.PhoneNumber,
                IsDefault = shippingAddress.IsDefault,
                AddressType = shippingAddress.AddressType,
                CreatedAt = shippingAddress.CreatedAt
            },
            BillingAddress = billingAddressDto,
            OrderItems = orderItemDtos,
            StatusHistory = statusHistoryDtos
        };

        return Result.Success(orderDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş getirme sırasında hata oluştu: {Id}", request.Id);
            return Result.Failure<OrderDto>(Error.Problem("Order.GetOrderByIdError", "Sipariş getirme sırasında bir hata oluştu."));
        }
    }
}
