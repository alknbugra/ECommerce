using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Orders.Queries.GetOrders;

/// <summary>
/// Siparişleri getirme sorgu handler'ı
/// </summary>
public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, List<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetOrdersQueryHandler> _logger;

    public GetOrdersQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetOrdersQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Siparişler getiriliyor - UserId: {UserId}, Status: {Status}", request.UserId, request.Status);

        var orders = await _unitOfWork.Orders.GetAllAsync();

        // Filtreleme
        var filteredOrders = orders.AsQueryable();

        if (request.UserId.HasValue)
        {
            filteredOrders = filteredOrders.Where(o => o.UserId == request.UserId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            filteredOrders = filteredOrders.Where(o => o.Status == request.Status);
        }

        if (!string.IsNullOrWhiteSpace(request.PaymentStatus))
        {
            filteredOrders = filteredOrders.Where(o => o.PaymentStatus == request.PaymentStatus);
        }

        if (request.StartDate.HasValue)
        {
            filteredOrders = filteredOrders.Where(o => o.OrderDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            filteredOrders = filteredOrders.Where(o => o.OrderDate <= request.EndDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            filteredOrders = filteredOrders.Where(o => 
                o.OrderNumber.ToLower().Contains(searchTerm) ||
                o.Notes != null && o.Notes.ToLower().Contains(searchTerm));
        }

        // Sıralama
        filteredOrders = request.SortBy?.ToLower() switch
        {
            "ordernumber" => request.SortDirection.ToLower() == "asc"
                ? filteredOrders.OrderBy(o => o.OrderNumber)
                : filteredOrders.OrderByDescending(o => o.OrderNumber),
            "status" => request.SortDirection.ToLower() == "asc"
                ? filteredOrders.OrderBy(o => o.Status)
                : filteredOrders.OrderByDescending(o => o.Status),
            "totalamount" => request.SortDirection.ToLower() == "asc"
                ? filteredOrders.OrderBy(o => o.TotalAmount)
                : filteredOrders.OrderByDescending(o => o.TotalAmount),
            _ => request.SortDirection.ToLower() == "asc"
                ? filteredOrders.OrderBy(o => o.OrderDate)
                : filteredOrders.OrderByDescending(o => o.OrderDate)
        };

        // Sayfalama
        var pagedOrders = filteredOrders
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // DTO'ya dönüştür
        var orderDtos = new List<OrderDto>();
        foreach (var order in pagedOrders)
        {
            // Kullanıcı bilgilerini al
            var user = await _unitOfWork.Users.GetByIdAsync(order.UserId);
            var userName = user?.FullName ?? "Bilinmeyen Kullanıcı";

            orderDtos.Add(new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                UserName = userName,
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
            });
        }

        _logger.LogInformation("{Count} sipariş getirildi", orderDtos.Count);

        return Result.Success<List<OrderDto>>(orderDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Siparişler getirme sırasında hata oluştu");
            return Result.Failure<List<OrderDto>>(Error.Problem("Order.GetOrdersError", "Siparişler getirme sırasında bir hata oluştu."));
        }
    }
}
