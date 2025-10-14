using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Cart.Commands.AddToCart;
using ECommerce.Application.Features.Cart.Commands.ClearCart;
using ECommerce.Application.Features.Cart.Commands.RemoveFromCart;
using ECommerce.Application.Features.Cart.Commands.UpdateCartItem;
using ECommerce.Application.Features.Cart.Queries.GetCart;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Sepet endpoint'leri
/// </summary>
public static class CartEndpoints
{
    /// <summary>
    /// Sepet endpoint'lerini kaydet
    /// </summary>
    public static void MapCartEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/cart")
            .WithTags("Cart")
            .WithOpenApi();

        // Sepet getir
        group.MapGet("/", GetCart)
            .WithName("GetCart")
            .WithSummary("Sepeti getir")
            .WithDescription("Kullanıcının veya session'ın sepetini getirir")
            .Produces<CartDto>(200)
            .Produces(404);

        // Sepete ürün ekle
        group.MapPost("/add", AddToCart)
            .WithName("AddToCart")
            .WithSummary("Sepete ürün ekle")
            .WithDescription("Sepete yeni ürün ekler veya mevcut ürünün miktarını artırır")
            .Produces<CartDto>(200)
            .Produces(400)
            .Produces(404);

        // Sepet ürünü güncelle
        group.MapPut("/update", UpdateCartItem)
            .WithName("UpdateCartItem")
            .WithSummary("Sepet ürünü güncelle")
            .WithDescription("Sepetteki ürünün miktarını günceller")
            .Produces<CartDto>(200)
            .Produces(400)
            .Produces(404);

        // Sepetten ürün çıkar
        group.MapDelete("/remove/{cartItemId:guid}", RemoveFromCart)
            .WithName("RemoveFromCart")
            .WithSummary("Sepetten ürün çıkar")
            .WithDescription("Sepetten belirtilen ürünü çıkarır")
            .Produces<CartDto>(200)
            .Produces(404);

        // Sepeti temizle
        group.MapDelete("/clear", ClearCart)
            .WithName("ClearCart")
            .WithSummary("Sepeti temizle")
            .WithDescription("Sepetteki tüm ürünleri temizler")
            .Produces<CartDto>(200)
            .Produces(404);
    }

    /// <summary>
    /// Sepeti getir
    /// </summary>
    private static async Task<IResult> GetCart(
        [FromQuery] Guid? userId,
        [FromQuery] string? sessionId,
        [FromServices] IQueryHandler<GetCartQuery, CartDto> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCartQuery
        {
            UserId = userId,
            SessionId = sessionId
        };

        var result = await handler.Handle(query, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Sepete ürün ekle
    /// </summary>
    private static async Task<IResult> AddToCart(
        AddToCartDto dto,
        [FromQuery] Guid? userId,
        [FromQuery] string? sessionId,
        [FromServices] ICommandHandler<AddToCartCommand, CartDto> handler,
        CancellationToken cancellationToken)
    {
        var command = new AddToCartCommand
        {
            UserId = userId,
            SessionId = sessionId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Sepet ürünü güncelle
    /// </summary>
    private static async Task<IResult> UpdateCartItem(
        UpdateCartItemDto dto,
        [FromQuery] Guid? userId,
        [FromQuery] string? sessionId,
        [FromServices] ICommandHandler<UpdateCartItemCommand, CartDto> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCartItemCommand
        {
            UserId = userId,
            SessionId = sessionId,
            CartItemId = dto.CartItemId,
            Quantity = dto.Quantity
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Sepetten ürün çıkar
    /// </summary>
    private static async Task<IResult> RemoveFromCart(
        Guid cartItemId,
        [FromQuery] Guid? userId,
        [FromQuery] string? sessionId,
        [FromServices] ICommandHandler<RemoveFromCartCommand, CartDto> handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveFromCartCommand
        {
            UserId = userId,
            SessionId = sessionId,
            CartItemId = cartItemId
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Sepeti temizle
    /// </summary>
    private static async Task<IResult> ClearCart(
        [FromQuery] Guid? userId,
        [FromQuery] string? sessionId,
        [FromServices] ICommandHandler<ClearCartCommand, CartDto> handler,
        CancellationToken cancellationToken)
    {
        var command = new ClearCartCommand
        {
            UserId = userId,
            SessionId = sessionId
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }
}
