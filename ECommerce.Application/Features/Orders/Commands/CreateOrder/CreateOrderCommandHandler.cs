using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder;

/// <summary>
/// Sipariş oluşturma komut handler'ı
/// </summary>
public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateOrderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Yeni sipariş oluşturuluyor - Kullanıcı: {UserId}", request.UserId);

            // Kullanıcı kontrolü
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure<OrderDto>(Error.NotFound("Order.UserNotFound", "Kullanıcı bulunamadı."));
            }

            // Teslimat adresi kontrolü
            var shippingAddress = await _unitOfWork.Addresses.GetByIdAsync(request.ShippingAddressId);
            if (shippingAddress == null || shippingAddress.UserId != request.UserId)
            {
                return Result.Failure<OrderDto>(Error.NotFound("Order.ShippingAddressNotFound", "Teslimat adresi bulunamadı veya kullanıcıya ait değil."));
            }

            // Fatura adresi kontrolü
            Address? billingAddress = null;
            if (request.BillingAddressId.HasValue)
            {
                billingAddress = await _unitOfWork.Addresses.GetByIdAsync(request.BillingAddressId.Value);
                if (billingAddress == null || billingAddress.UserId != request.UserId)
                {
                    return Result.Failure<OrderDto>(Error.NotFound("Order.BillingAddressNotFound", "Fatura adresi bulunamadı veya kullanıcıya ait değil."));
                }
            }

        // Sipariş detaylarını kontrol et ve ürün bilgilerini al
        var orderItems = new List<OrderItem>();
        decimal subTotal = 0;

        foreach (var itemDto in request.OrderItems)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Ürün bulunamadı: {itemDto.ProductId}");
            }

            if (!product.IsActive)
            {
                throw new BadRequestException($"Ürün aktif değil: {product.Name}");
            }

            if (product.StockQuantity < itemDto.Quantity)
            {
                throw new BadRequestException($"Yetersiz stok: {product.Name} (Mevcut: {product.StockQuantity}, İstenen: {itemDto.Quantity})");
            }

            var unitPrice = itemDto.UnitPrice ?? product.Price;
            var totalPrice = unitPrice * itemDto.Quantity - itemDto.DiscountAmount;

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSku = product.Sku,
                Quantity = itemDto.Quantity,
                UnitPrice = unitPrice,
                TotalPrice = totalPrice,
                DiscountAmount = itemDto.DiscountAmount,
                ProductImageUrl = product.MainImageUrl
            };

            orderItems.Add(orderItem);
            subTotal += totalPrice;

            // Stok güncelle
            product.StockQuantity -= itemDto.Quantity;
            await _unitOfWork.Products.UpdateAsync(product);
        }

        // Sipariş numarası oluştur
        var orderNumber = GenerateOrderNumber();

        // Toplam tutarı hesapla
        var totalAmount = subTotal + request.ShippingCost + request.TaxAmount - request.DiscountAmount;

        // Sipariş oluştur
        var order = new Order
        {
            OrderNumber = orderNumber,
            UserId = request.UserId,
            ShippingAddressId = request.ShippingAddressId,
            BillingAddressId = request.BillingAddressId,
            Status = "Pending",
            PaymentStatus = "Pending",
            PaymentMethod = request.PaymentMethod,
            SubTotal = subTotal,
            ShippingCost = request.ShippingCost,
            TaxAmount = request.TaxAmount,
            DiscountAmount = request.DiscountAmount,
            TotalAmount = totalAmount,
            Notes = request.Notes,
            OrderDate = DateTime.UtcNow
        };

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Sipariş detaylarını ekle
        foreach (var orderItem in orderItems)
        {
            orderItem.OrderId = order.Id;
            await _unitOfWork.OrderItems.AddAsync(orderItem);
        }

        // Sipariş durumu geçmişini ekle
        var statusHistory = new OrderStatusHistory
        {
            OrderId = order.Id,
            PreviousStatus = null,
            NewStatus = "Pending",
            StatusChangeDate = DateTime.UtcNow,
            Notes = "Sipariş oluşturuldu",
            ChangedByUserId = request.UserId
        };

        await _unitOfWork.OrderStatusHistories.AddAsync(statusHistory);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Sipariş başarıyla oluşturuldu: {OrderNumber} (ID: {Id})", orderNumber, order.Id);

        // DTO'ya dönüştür
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
            BillingAddress = billingAddress != null ? new AddressDto
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
            } : null,
            OrderItems = orderItems.Select(oi => new OrderItemDto
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
            }).ToList(),
            StatusHistory = new List<OrderStatusHistoryDto>
            {
                new()
                {
                    Id = statusHistory.Id,
                    OrderId = statusHistory.OrderId,
                    PreviousStatus = statusHistory.PreviousStatus,
                    NewStatus = statusHistory.NewStatus,
                    StatusChangeDate = statusHistory.StatusChangeDate,
                    Notes = statusHistory.Notes,
                    ChangedByUserId = statusHistory.ChangedByUserId
                }
            }
        };

        return Result.Success(orderDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş oluşturma sırasında hata oluştu. UserId: {UserId}", request.UserId);
            return Result.Failure<OrderDto>(Error.Problem("Order.CreateError", "Sipariş oluşturma sırasında bir hata oluştu."));
        }
    }

    /// <summary>
    /// Sipariş numarası oluştur
    /// </summary>
    private static string GenerateOrderNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        return $"ORD-{timestamp}-{random}";
    }
}
