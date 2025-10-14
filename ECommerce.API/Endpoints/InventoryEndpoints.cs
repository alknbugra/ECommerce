using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Inventory.Commands.StockIn;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Stok endpoint'leri
/// </summary>
public static class InventoryEndpoints
{
    /// <summary>
    /// Stok endpoint'lerini kaydet
    /// </summary>
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/inventory")
            .WithTags("Inventory")
            .WithOpenApi();

        // Stok bilgilerini al
        group.MapGet("/{productId:guid}", GetInventory)
            .WithName("GetInventory")
            .WithSummary("Stok bilgilerini al")
            .WithDescription("Belirtilen ürünün stok bilgilerini getirir")
            .Produces<InventoryDto>(200)
            .Produces(404);

        // Tüm stok bilgilerini al
        group.MapGet("/", GetAllInventories)
            .WithName("GetAllInventories")
            .WithSummary("Tüm stok bilgilerini al")
            .WithDescription("Tüm ürünlerin stok bilgilerini getirir")
            .Produces<List<InventoryDto>>(200);

        // Düşük stoklu ürünleri al
        group.MapGet("/low-stock", GetLowStockInventories)
            .WithName("GetLowStockInventories")
            .WithSummary("Düşük stoklu ürünleri al")
            .WithDescription("Stok seviyesi düşük olan ürünleri getirir")
            .Produces<List<InventoryDto>>(200);

        // Stok hareketlerini al
        group.MapGet("/movements", GetInventoryMovements)
            .WithName("GetInventoryMovements")
            .WithSummary("Stok hareketlerini al")
            .WithDescription("Stok hareketlerini getirir")
            .Produces<List<InventoryMovementDto>>(200);

        // Stok uyarılarını al
        group.MapGet("/alerts", GetStockAlerts)
            .WithName("GetStockAlerts")
            .WithSummary("Stok uyarılarını al")
            .WithDescription("Stok uyarılarını getirir")
            .Produces<List<StockAlertDto>>(200);

        // Stok girişi yap
        group.MapPost("/stock-in", StockIn)
            .WithName("StockIn")
            .WithSummary("Stok girişi yap")
            .WithDescription("Ürüne stok girişi yapar")
            .Produces<bool>(200)
            .Produces(400);

        // Stok çıkışı yap
        group.MapPost("/stock-out", StockOut)
            .WithName("StockOut")
            .WithSummary("Stok çıkışı yap")
            .WithDescription("Üründen stok çıkışı yapar")
            .Produces<bool>(200)
            .Produces(400);

        // Stok rezervasyonu yap
        group.MapPost("/reserve", ReserveStock)
            .WithName("ReserveStock")
            .WithSummary("Stok rezervasyonu yap")
            .WithDescription("Ürün için stok rezervasyonu yapar")
            .Produces<bool>(200)
            .Produces(400);

        // Stok rezervasyonunu kaldır
        group.MapPost("/release-reservation", ReleaseReservedStock)
            .WithName("ReleaseReservedStock")
            .WithSummary("Stok rezervasyonunu kaldır")
            .WithDescription("Ürün için stok rezervasyonunu kaldırır")
            .Produces<bool>(200)
            .Produces(400);

        // Stok kontrolü yap
        group.MapPost("/check-availability", CheckStockAvailability)
            .WithName("CheckStockAvailability")
            .WithSummary("Stok kontrolü yap")
            .WithDescription("Sipariş için stok yeterliliğini kontrol eder")
            .Produces<bool>(200)
            .Produces(400);

        // Sipariş stok işlemi
        group.MapPost("/process-order", ProcessOrderStock)
            .WithName("ProcessOrderStock")
            .WithSummary("Sipariş stok işlemi")
            .WithDescription("Sipariş için stok işlemlerini yapar")
            .Produces<bool>(200)
            .Produces(400);

        // Stok uyarılarını kontrol et
        group.MapPost("/check-alerts", CheckStockAlerts)
            .WithName("CheckStockAlerts")
            .WithSummary("Stok uyarılarını kontrol et")
            .WithDescription("Stok uyarılarını kontrol eder ve yeni uyarılar oluşturur")
            .Produces<int>(200);

        // Stok uyarısını okundu işaretle
        group.MapPost("/alerts/{alertId:guid}/mark-read", MarkAlertAsRead)
            .WithName("MarkAlertAsRead")
            .WithSummary("Stok uyarısını okundu işaretle")
            .WithDescription("Stok uyarısını okundu olarak işaretler")
            .Produces<bool>(200)
            .Produces(404);
    }

    /// <summary>
    /// Stok bilgilerini al
    /// </summary>
    private static async Task<IResult> GetInventory(
        Guid productId,
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var inventory = await inventoryService.GetInventoryAsync(productId, cancellationToken);
        if (inventory == null)
            return Results.NotFound();

        return Results.Ok(inventory);
    }

    /// <summary>
    /// Tüm stok bilgilerini al
    /// </summary>
    private static async Task<IResult> GetAllInventories(
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var inventories = await inventoryService.GetAllInventoriesAsync(cancellationToken);
        return Results.Ok(inventories);
    }

    /// <summary>
    /// Düşük stoklu ürünleri al
    /// </summary>
    private static async Task<IResult> GetLowStockInventories(
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var inventories = await inventoryService.GetLowStockInventoriesAsync(cancellationToken);
        return Results.Ok(inventories);
    }

    /// <summary>
    /// Stok hareketlerini al
    /// </summary>
    private static async Task<IResult> GetInventoryMovements(
        Guid? productId,
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var movements = await inventoryService.GetInventoryMovementsAsync(productId, cancellationToken);
        return Results.Ok(movements);
    }

    /// <summary>
    /// Stok uyarılarını al
    /// </summary>
    private static async Task<IResult> GetStockAlerts(
        bool? isRead,
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var alerts = await inventoryService.GetStockAlertsAsync(isRead, cancellationToken);
        return Results.Ok(alerts);
    }

    /// <summary>
    /// Stok girişi yap
    /// </summary>
    private static async Task<IResult> StockIn(
        StockInDto dto,
        [FromServices] ICommandHandler<StockInCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        var command = new StockInCommand
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            Reason = dto.Reason,
            UserId = dto.UserId
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Stok çıkışı yap
    /// </summary>
    private static async Task<IResult> StockOut(
        StockOutDto dto,
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.StockOutAsync(
            dto.ProductId, dto.Quantity, dto.Reason, dto.UserId, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Stok rezervasyonu yap
    /// </summary>
    private static async Task<IResult> ReserveStock(
        ReserveStockDto dto,
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.ReserveStockAsync(
            dto.ProductId, dto.Quantity, dto.RelatedEntityId, dto.RelatedEntityType, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Stok rezervasyonunu kaldır
    /// </summary>
    private static async Task<IResult> ReleaseReservedStock(
        ReleaseReservedStockDto dto,
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.ReleaseReservedStockAsync(
            dto.ProductId, dto.Quantity, dto.RelatedEntityId, dto.RelatedEntityType, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Stok kontrolü yap
    /// </summary>
    private static async Task<IResult> CheckStockAvailability(
        List<OrderItemDto> orderItems,
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.CheckStockAvailabilityAsync(orderItems, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Sipariş stok işlemi
    /// </summary>
    private static async Task<IResult> ProcessOrderStock(
        ProcessOrderStockDto dto,
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.ProcessOrderStockAsync(
            dto.OrderId, dto.OrderItems, dto.UserId, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Stok uyarılarını kontrol et
    /// </summary>
    private static async Task<IResult> CheckStockAlerts(
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var alertCount = await inventoryService.CheckStockAlertsAsync(cancellationToken);
        return Results.Ok(new { alertCount });
    }

    /// <summary>
    /// Stok uyarısını okundu işaretle
    /// </summary>
    private static async Task<IResult> MarkAlertAsRead(
        Guid alertId,
        MarkAlertAsReadDto dto,
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.MarkAlertAsReadAsync(alertId, dto.UserId, cancellationToken);
        return Results.Ok(result);
    }
}

/// <summary>
/// Stok girişi DTO'su
/// </summary>
public class StockInDto
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
}

/// <summary>
/// Stok çıkışı DTO'su
/// </summary>
public class StockOutDto
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
}

/// <summary>
/// Stok rezervasyonu DTO'su
/// </summary>
public class ReserveStockDto
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public Guid RelatedEntityId { get; set; }
    public string RelatedEntityType { get; set; } = string.Empty;
}

/// <summary>
/// Stok rezervasyonu kaldırma DTO'su
/// </summary>
public class ReleaseReservedStockDto
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public Guid RelatedEntityId { get; set; }
    public string RelatedEntityType { get; set; } = string.Empty;
}

/// <summary>
/// Sipariş stok işlemi DTO'su
/// </summary>
public class ProcessOrderStockDto
{
    public Guid OrderId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
    public Guid? UserId { get; set; }
}

/// <summary>
/// Stok uyarısı okundu işaretleme DTO'su
/// </summary>
public class MarkAlertAsReadDto
{
    public Guid UserId { get; set; }
}
