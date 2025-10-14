using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Cargo.Commands.CreateCargo;
using ECommerce.Application.Features.Cargo.Commands.UpdateCargoStatus;
using ECommerce.Application.Features.Cargo.Queries.GetCargoByTrackingNumber;
using ECommerce.Application.Features.Cargo.Queries.GetCargoTrackingHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Kargo endpoint'leri
/// </summary>
public static class CargoEndpoints
{
    /// <summary>
    /// Kargo endpoint'lerini kaydet
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapCargoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/cargo")
            .WithTags("Cargo")
            .WithOpenApi();

        // Takip numarasına göre kargo getir (Public)
        group.MapGet("/track/{trackingNumber}", GetCargoByTrackingNumber)
            .WithName("GetCargoByTrackingNumber")
            .WithSummary("Takip numarasına göre kargo getir")
            .WithDescription("Takip numarası ile kargo bilgilerini ve takip geçmişini getirir")
            .Produces<CargoDto>(200)
            .Produces(404)
            .Produces(500);

        // Kargo takip geçmişi getir (Public)
        group.MapGet("/track/{trackingNumber}/history", GetCargoTrackingHistory)
            .WithName("GetCargoTrackingHistory")
            .WithSummary("Kargo takip geçmişi getir")
            .WithDescription("Takip numarası ile kargo takip geçmişini getirir")
            .Produces<List<CargoTrackingDto>>(200)
            .Produces(404)
            .Produces(500);

        // Kargo oluştur (Admin/Staff)
        group.MapPost("/", CreateCargo)
            .WithName("CreateCargo")
            .WithSummary("Kargo oluştur")
            .WithDescription("Yeni kargo kaydı oluşturur")
            .RequireAuthorization()
            .Produces<CargoDto>(201)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // Kargo durumu güncelle (Admin/Staff)
        group.MapPut("/{id}/status", UpdateCargoStatus)
            .WithName("UpdateCargoStatus")
            .WithSummary("Kargo durumu güncelle")
            .WithDescription("Kargo durumunu günceller ve takip geçmişine ekler")
            .RequireAuthorization()
            .Produces<CargoDto>(200)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);
    }

    /// <summary>
    /// Takip numarasına göre kargo getir
    /// </summary>
    private static async Task<IResult> GetCargoByTrackingNumber(
        string trackingNumber,
        [FromServices] IQueryHandler<GetCargoByTrackingNumberQuery, CargoDto?> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCargoByTrackingNumberQuery { TrackingNumber = trackingNumber };
        var result = await handler.Handle(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Error);
    }

    /// <summary>
    /// Kargo takip geçmişi getir
    /// </summary>
    private static async Task<IResult> GetCargoTrackingHistory(
        string trackingNumber,
        [FromServices] IQueryHandler<GetCargoByTrackingNumberQuery, CargoDto?> cargoHandler,
        [FromServices] IQueryHandler<GetCargoTrackingHistoryQuery, IEnumerable<CargoTrackingDto>> historyHandler,
        CancellationToken cancellationToken)
    {
        // Önce kargo bilgilerini al
        var cargoQuery = new GetCargoByTrackingNumberQuery { TrackingNumber = trackingNumber };
        var cargoResult = await cargoHandler.Handle(cargoQuery, cancellationToken);

        if (!cargoResult.IsSuccess)
        {
            return Results.NotFound(cargoResult.Error);
        }

        // Takip geçmişini al
        var historyQuery = new GetCargoTrackingHistoryQuery { CargoId = cargoResult.Value!.Id };
        var historyResult = await historyHandler.Handle(historyQuery, cancellationToken);

        return historyResult.IsSuccess
            ? Results.Ok(historyResult.Value)
            : Results.NotFound(historyResult.Error);
    }

    /// <summary>
    /// Kargo oluştur
    /// </summary>
    private static async Task<IResult> CreateCargo(
        CreateCargoCommand command,
        [FromServices] ICommandHandler<CreateCargoCommand, CargoDto> handler,
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/cargo/track/{result.Value.TrackingNumber}", result.Value)
            : Results.BadRequest(result.Error);
    }

    /// <summary>
    /// Kargo durumu güncelle
    /// </summary>
    private static async Task<IResult> UpdateCargoStatus(
        Guid id,
        UpdateCargoStatusCommand command,
        [FromServices] ICommandHandler<UpdateCargoStatusCommand, CargoDto> handler,
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await handler.Handle(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }
}
