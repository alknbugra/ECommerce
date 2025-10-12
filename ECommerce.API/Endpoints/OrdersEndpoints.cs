using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Orders.Commands.CreateOrder;
using ECommerce.Application.Features.Orders.Commands.UpdateOrderStatus;
using ECommerce.Application.Features.Orders.Queries.GetOrderById;
using ECommerce.Application.Features.Orders.Queries.GetOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Sipariş endpoint'leri
/// </summary>
public static class OrdersEndpoints
{
    /// <summary>
    /// Sipariş endpoint'lerini kaydet
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapOrdersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/orders")
            .WithTags("Orders")
            .WithOpenApi();

        // Tüm siparişleri getir (Admin)
        group.MapGet("/", GetOrders)
            .WithName("GetOrders")
            .WithSummary("Siparişleri getir")
            .WithDescription("Filtreleme, arama ve sayfalama ile siparişleri getirir")
            .RequireAuthorization()
            .Produces<List<OrderDto>>(200)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // Kullanıcının siparişlerini getir
        group.MapGet("/my-orders", GetMyOrders)
            .WithName("GetMyOrders")
            .WithSummary("Kullanıcının siparişlerini getir")
            .WithDescription("Giriş yapmış kullanıcının siparişlerini getirir")
            .RequireAuthorization()
            .Produces<List<OrderDto>>(200)
            .Produces(401)
            .Produces(500);

        // ID'ye göre sipariş getir
        group.MapGet("/{id:guid}", GetOrderById)
            .WithName("GetOrderById")
            .WithSummary("ID'ye göre sipariş getir")
            .WithDescription("Belirtilen ID'ye sahip siparişi getirir")
            .RequireAuthorization()
            .Produces<OrderDto>(200)
            .Produces(401)
            .Produces(404)
            .Produces(500);

        // Yeni sipariş oluştur
        group.MapPost("/", CreateOrder)
            .WithName("CreateOrder")
            .WithSummary("Yeni sipariş oluştur")
            .WithDescription("Yeni bir sipariş oluşturur")
            .RequireAuthorization()
            .Produces<OrderDto>(201)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // Sipariş durumu güncelle (Admin)
        group.MapPut("/{id:guid}/status", UpdateOrderStatus)
            .WithName("UpdateOrderStatus")
            .WithSummary("Sipariş durumu güncelle")
            .WithDescription("Sipariş durumunu günceller")
            .RequireAuthorization()
            .Produces<OrderDto>(200)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);

        // Sipariş iptal et
        group.MapPut("/{id:guid}/cancel", CancelOrder)
            .WithName("CancelOrder")
            .WithSummary("Sipariş iptal et")
            .WithDescription("Siparişi iptal eder")
            .RequireAuthorization()
            .Produces<OrderDto>(200)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);
    }

    /// <summary>
    /// Siparişleri getir (Admin)
    /// </summary>
    private static async Task<IResult> GetOrders(
        [AsParameters] GetOrdersRequest request,
        [FromServices] IQueryHandler<GetOrdersQuery, List<OrderDto>> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetOrdersQuery
        {
            UserId = request.UserId,
            Status = request.Status,
            PaymentStatus = request.PaymentStatus,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            SearchTerm = request.SearchTerm,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var orders = await handler.HandleAsync(query, cancellationToken);
        return Results.Ok(orders);
    }

    /// <summary>
    /// Kullanıcının siparişlerini getir
    /// </summary>
    private static async Task<IResult> GetMyOrders(
        [AsParameters] GetOrdersRequest request,
        [FromServices] IQueryHandler<GetOrdersQuery, List<OrderDto>> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        var query = new GetOrdersQuery
        {
            UserId = userId,
            Status = request.Status,
            PaymentStatus = request.PaymentStatus,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            SearchTerm = request.SearchTerm,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var orders = await handler.HandleAsync(query, cancellationToken);
        return Results.Ok(orders);
    }

    /// <summary>
    /// ID'ye göre sipariş getir
    /// </summary>
    private static async Task<IResult> GetOrderById(
        Guid id,
        [FromServices] IQueryHandler<GetOrderByIdQuery, OrderDto?> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        var query = new GetOrderByIdQuery 
        { 
            Id = id,
            UserId = userId // Güvenlik için kullanıcı ID'si kontrol edilir
        };

        var order = await handler.HandleAsync(query, cancellationToken);
        if (order == null)
        {
            return Results.NotFound($"ID'si {id} olan sipariş bulunamadı.");
        }

        return Results.Ok(order);
    }

    /// <summary>
    /// Yeni sipariş oluştur
    /// </summary>
    private static async Task<IResult> CreateOrder(
        [FromBody] CreateOrderCommand command,
        [FromServices] ICommandHandler<CreateOrderCommand, OrderDto> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        command.UserId = userId.Value;
        var order = await handler.HandleAsync(command, cancellationToken);
        return Results.CreatedAtRoute("GetOrderById", new { id = order.Id }, order);
    }

    /// <summary>
    /// Sipariş durumu güncelle
    /// </summary>
    private static async Task<IResult> UpdateOrderStatus(
        Guid id,
        [FromBody] UpdateOrderStatusRequest request,
        [FromServices] ICommandHandler<UpdateOrderStatusCommand, OrderDto> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        var command = new UpdateOrderStatusCommand
        {
            OrderId = id,
            NewStatus = request.NewStatus,
            Notes = request.Notes,
            ChangedByUserId = userId.Value,
            TrackingNumber = request.TrackingNumber,
            ShippingCompany = request.ShippingCompany
        };

        var order = await handler.HandleAsync(command, cancellationToken);
        return Results.Ok(order);
    }

    /// <summary>
    /// Sipariş iptal et
    /// </summary>
    private static async Task<IResult> CancelOrder(
        Guid id,
        [FromBody] CancelOrderRequest request,
        [FromServices] ICommandHandler<UpdateOrderStatusCommand, OrderDto> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        var command = new UpdateOrderStatusCommand
        {
            OrderId = id,
            NewStatus = "Cancelled",
            Notes = request.Reason,
            ChangedByUserId = userId.Value
        };

        var order = await handler.HandleAsync(command, cancellationToken);
        return Results.Ok(order);
    }

    /// <summary>
    /// HttpContext'ten kullanıcı ID'sini al
    /// </summary>
    private static Guid? GetUserIdFromContext(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        return null;
    }

    /// <summary>
    /// GetOrders request modeli
    /// </summary>
    public record GetOrdersRequest(
        Guid? UserId = null,
        string? Status = null,
        string? PaymentStatus = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        string? SearchTerm = null,
        int PageNumber = 1,
        int PageSize = 10,
        string? SortBy = "OrderDate",
        string SortDirection = "desc");

    /// <summary>
    /// UpdateOrderStatus request modeli
    /// </summary>
    public record UpdateOrderStatusRequest(
        string NewStatus,
        string? Notes = null,
        string? TrackingNumber = null,
        string? ShippingCompany = null);

    /// <summary>
    /// CancelOrder request modeli
    /// </summary>
    public record CancelOrderRequest(
        string? Reason = null);
}
