using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Wishlists.Commands.CreateWishlist;
using ECommerce.Application.Features.Wishlists.Commands.AddToWishlist;
using ECommerce.Application.Features.Wishlists.Commands.RemoveFromWishlist;
using ECommerce.Application.Features.Wishlists.Queries.GetUserWishlists;
using ECommerce.Application.Features.Wishlists.Queries.GetWishlistStats;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Favori listeler endpoint'leri
/// </summary>
public static class WishlistsEndpoints
{
    /// <summary>
    /// Favori liste endpoint'lerini map et
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapWishlistsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/wishlists")
            .WithTags("Favori Listeler")
            .WithOpenApi();

        // Favori liste oluştur
        group.MapPost("/", CreateWishlist)
            .WithName("CreateWishlist")
            .WithSummary("Favori liste oluştur")
            .WithDescription("Yeni bir favori liste oluşturur")
            .RequireAuthorization();

        // Kullanıcı favori listelerini getir
        group.MapGet("/user/{userId:guid}", GetUserWishlists)
            .WithName("GetUserWishlists")
            .WithSummary("Kullanıcı favori listelerini getir")
            .WithDescription("Belirtilen kullanıcının tüm favori listelerini getirir")
            .RequireAuthorization();

        // Favori liste detayını getir
        group.MapGet("/{wishlistId:guid}", GetWishlist)
            .WithName("GetWishlist")
            .WithSummary("Favori liste detayını getir")
            .WithDescription("Belirtilen favori listenin detaylarını getirir")
            .RequireAuthorization();

        // Varsayılan favori listeyi getir
        group.MapGet("/user/{userId:guid}/default", GetDefaultWishlist)
            .WithName("GetDefaultWishlist")
            .WithSummary("Varsayılan favori listeyi getir")
            .WithDescription("Kullanıcının varsayılan favori listesini getirir")
            .RequireAuthorization();

        // Favori listeyi güncelle
        group.MapPut("/{wishlistId:guid}", UpdateWishlist)
            .WithName("UpdateWishlist")
            .WithSummary("Favori listeyi güncelle")
            .WithDescription("Mevcut favori listeyi günceller")
            .RequireAuthorization();

        // Favori listeyi sil
        group.MapDelete("/{wishlistId:guid}", DeleteWishlist)
            .WithName("DeleteWishlist")
            .WithSummary("Favori listeyi sil")
            .WithDescription("Belirtilen favori listeyi siler")
            .RequireAuthorization();

        // Ürünü favorilere ekle
        group.MapPost("/items", AddToWishlist)
            .WithName("AddToWishlist")
            .WithSummary("Ürünü favorilere ekle")
            .WithDescription("Belirtilen ürünü favori listesine ekler")
            .RequireAuthorization();

        // Ürünü favorilerden çıkar
        group.MapDelete("/items/{wishlistItemId:guid}", RemoveFromWishlist)
            .WithName("RemoveFromWishlist")
            .WithSummary("Ürünü favorilerden çıkar")
            .WithDescription("Belirtilen ürünü favori listesinden çıkarır")
            .RequireAuthorization();

        // Favori ürünü güncelle
        group.MapPut("/items/{wishlistItemId:guid}", UpdateWishlistItem)
            .WithName("UpdateWishlistItem")
            .WithSummary("Favori ürünü güncelle")
            .WithDescription("Favori listedeki ürün bilgilerini günceller")
            .RequireAuthorization();

        // Favori liste paylaş
        group.MapPost("/{wishlistId:guid}/share", ShareWishlist)
            .WithName("ShareWishlist")
            .WithSummary("Favori listeyi paylaş")
            .WithDescription("Favori listeyi paylaşım kodu ile paylaşır")
            .RequireAuthorization();

        // Paylaşılan favori listeyi getir
        group.MapGet("/shared/{shareCode}", GetSharedWishlist)
            .WithName("GetSharedWishlist")
            .WithSummary("Paylaşılan favori listeyi getir")
            .WithDescription("Paylaşım kodu ile favori listeyi getirir");

        // Paylaşımı iptal et
        group.MapDelete("/shares/{shareId:guid}", CancelShare)
            .WithName("CancelShare")
            .WithSummary("Paylaşımı iptal et")
            .WithDescription("Aktif paylaşımı iptal eder")
            .RequireAuthorization();

        // Ürün favorilerde var mı kontrol et
        group.MapGet("/check/{productId:guid}/user/{userId:guid}", CheckProductInWishlist)
            .WithName("CheckProductInWishlist")
            .WithSummary("Ürün favorilerde var mı kontrol et")
            .WithDescription("Belirtilen ürünün kullanıcının favorilerinde olup olmadığını kontrol eder")
            .RequireAuthorization();

        // Ürünün hangi listelerde olduğunu getir
        group.MapGet("/product/{productId:guid}/user/{userId:guid}", GetWishlistsContainingProduct)
            .WithName("GetWishlistsContainingProduct")
            .WithSummary("Ürünün hangi listelerde olduğunu getir")
            .WithDescription("Belirtilen ürünün hangi favori listelerinde olduğunu getirir")
            .RequireAuthorization();

        // Favori liste istatistiklerini getir
        group.MapGet("/user/{userId:guid}/stats", GetWishlistStats)
            .WithName("GetWishlistStats")
            .WithSummary("Favori liste istatistiklerini getir")
            .WithDescription("Kullanıcının favori liste istatistiklerini getirir")
            .RequireAuthorization();

        // Fiyat değişikliklerini kontrol et
        group.MapPost("/check-price-changes", CheckPriceChanges)
            .WithName("CheckPriceChanges")
            .WithSummary("Fiyat değişikliklerini kontrol et")
            .WithDescription("Favori ürünlerdeki fiyat değişikliklerini kontrol eder ve bildirim gönderir")
            .RequireAuthorization();

        // Stok değişikliklerini kontrol et
        group.MapPost("/check-stock-changes", CheckStockChanges)
            .WithName("CheckStockChanges")
            .WithSummary("Stok değişikliklerini kontrol et")
            .WithDescription("Favori ürünlerdeki stok değişikliklerini kontrol eder ve bildirim gönderir")
            .RequireAuthorization();
    }

    private static async Task<IResult> CreateWishlist(
        [FromBody] CreateWishlistDto createWishlistDto,
        [FromServices] ICommandHandler<CreateWishlistCommand, WishlistDto> handler,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(httpContextAccessor);
        if (userId == Guid.Empty)
            return Results.Unauthorized();

        var command = new CreateWishlistCommand
        {
            CreateWishlistDto = createWishlistDto,
            UserId = userId
        };

        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsSuccess)
            return Results.Created($"/api/wishlists/{result.Value.Id}", result.Value);
        
        return Results.Problem(
            title: "Favori liste oluşturulamadı",
            detail: result.Error.Description,
            statusCode: 400);
    }

    private static async Task<IResult> GetUserWishlists(
        Guid userId,
        [FromServices] IQueryHandler<GetUserWishlistsQuery, List<WishlistDto>> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUserWishlistsQuery { UserId = userId };
        var result = await handler.Handle(query, cancellationToken);
        
        if (result.IsSuccess)
            return Results.Ok(result.Value);
        
        return Results.Problem(
            title: "Favori listeler alınamadı",
            detail: result.Error.Description,
            statusCode: 400);
    }

    private static async Task<IResult> GetWishlist(
        Guid wishlistId,
        [FromServices] IWishlistService wishlistService,
        CancellationToken cancellationToken = default)
    {
        var result = await wishlistService.GetWishlistAsync(wishlistId, cancellationToken);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }

    private static async Task<IResult> GetDefaultWishlist(
        Guid userId,
        [FromServices] IWishlistService wishlistService,
        CancellationToken cancellationToken = default)
    {
        var result = await wishlistService.GetDefaultWishlistAsync(userId, cancellationToken);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }

    private static async Task<IResult> UpdateWishlist(
        Guid wishlistId,
        [FromBody] CreateWishlistDto updateWishlistDto,
        [FromServices] IWishlistService wishlistService,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(httpContextAccessor);
        if (userId == Guid.Empty)
            return Results.Unauthorized();

        var result = await wishlistService.UpdateWishlistAsync(wishlistId, updateWishlistDto, userId, cancellationToken);
        return result != null ? Results.Ok(result) : Results.BadRequest();
    }

    private static async Task<IResult> DeleteWishlist(
        Guid wishlistId,
        [FromServices] IWishlistService wishlistService,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(httpContextAccessor);
        if (userId == Guid.Empty)
            return Results.Unauthorized();

        var result = await wishlistService.DeleteWishlistAsync(wishlistId, userId, cancellationToken);
        return result ? Results.NoContent() : Results.BadRequest();
    }

    private static async Task<IResult> AddToWishlist(
        [FromBody] AddToWishlistDto addToWishlistDto,
        [FromServices] ICommandHandler<AddToWishlistCommand, WishlistItemDto> handler,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(httpContextAccessor);
        if (userId == Guid.Empty)
            return Results.Unauthorized();

        var command = new AddToWishlistCommand
        {
            AddToWishlistDto = addToWishlistDto,
            UserId = userId
        };

        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsSuccess)
            return Results.Created($"/api/wishlists/items/{result.Value.Id}", result.Value);
        
        return Results.Problem(
            title: "Ürün favorilere eklenemedi",
            detail: result.Error.Description,
            statusCode: 400);
    }

    private static async Task<IResult> RemoveFromWishlist(
        Guid wishlistItemId,
        [FromServices] ICommandHandler<RemoveFromWishlistCommand, bool> handler,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(httpContextAccessor);
        if (userId == Guid.Empty)
            return Results.Unauthorized();

        var command = new RemoveFromWishlistCommand
        {
            WishlistItemId = wishlistItemId,
            UserId = userId
        };

        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsSuccess)
            return Results.NoContent();
        
        return Results.Problem(
            title: "Ürün favorilerden çıkarılamadı",
            detail: result.Error.Description,
            statusCode: 400);
    }

    private static async Task<IResult> UpdateWishlistItem(
        Guid wishlistItemId,
        [FromBody] AddToWishlistDto updateWishlistItemDto,
        [FromServices] IWishlistService wishlistService,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(httpContextAccessor);
        if (userId == Guid.Empty)
            return Results.Unauthorized();

        var result = await wishlistService.UpdateWishlistItemAsync(wishlistItemId, updateWishlistItemDto, userId, cancellationToken);
        return result != null ? Results.Ok(result) : Results.BadRequest();
    }

    private static async Task<IResult> ShareWishlist(
        Guid wishlistId,
        [FromBody] ShareWishlistRequest request,
        [FromServices] IWishlistService wishlistService,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(httpContextAccessor);
        if (userId == Guid.Empty)
            return Results.Unauthorized();

        var result = await wishlistService.ShareWishlistAsync(
            wishlistId, 
            request.ShareType, 
            request.EmailAddress, 
            request.Message, 
            request.ExpirationDays, 
            userId, 
            cancellationToken);

        return result != null ? Results.Ok(result) : Results.BadRequest();
    }

    private static async Task<IResult> GetSharedWishlist(
        string shareCode,
        [FromServices] IWishlistService wishlistService,
        CancellationToken cancellationToken = default)
    {
        var result = await wishlistService.GetSharedWishlistAsync(shareCode, cancellationToken);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }

    private static async Task<IResult> CancelShare(
        Guid shareId,
        [FromServices] IWishlistService wishlistService,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(httpContextAccessor);
        if (userId == Guid.Empty)
            return Results.Unauthorized();

        var result = await wishlistService.CancelShareAsync(shareId, userId, cancellationToken);
        return result ? Results.NoContent() : Results.BadRequest();
    }

    private static async Task<IResult> CheckProductInWishlist(
        Guid productId,
        Guid userId,
        [FromServices] IWishlistService wishlistService,
        CancellationToken cancellationToken = default)
    {
        var result = await wishlistService.IsProductInWishlistAsync(productId, userId, cancellationToken);
        return Results.Ok(new { IsInWishlist = result });
    }

    private static async Task<IResult> GetWishlistsContainingProduct(
        Guid productId,
        Guid userId,
        [FromServices] IWishlistService wishlistService,
        CancellationToken cancellationToken = default)
    {
        var result = await wishlistService.GetWishlistsContainingProductAsync(productId, userId, cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetWishlistStats(
        Guid userId,
        [FromServices] IQueryHandler<GetWishlistStatsQuery, WishlistStatsDto> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetWishlistStatsQuery { UserId = userId };
        var result = await handler.Handle(query, cancellationToken);
        
        if (result.IsSuccess)
            return Results.Ok(result.Value);
        
        return Results.Problem(
            title: "Favori liste istatistikleri alınamadı",
            detail: result.Error.Description,
            statusCode: 400);
    }

    private static async Task<IResult> CheckPriceChanges(
        [FromServices] IWishlistService wishlistService,
        CancellationToken cancellationToken = default)
    {
        var result = await wishlistService.CheckPriceChangesAsync(cancellationToken);
        return result ? Results.Ok(new { Message = "Fiyat değişiklikleri kontrol edildi" }) : Results.BadRequest();
    }

    private static async Task<IResult> CheckStockChanges(
        [FromServices] IWishlistService wishlistService,
        CancellationToken cancellationToken = default)
    {
        var result = await wishlistService.CheckStockChangesAsync(cancellationToken);
        return result ? Results.Ok(new { Message = "Stok değişiklikleri kontrol edildi" }) : Results.BadRequest();
    }

    private static Guid GetUserIdFromContext(IHttpContextAccessor httpContextAccessor)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}

/// <summary>
/// Favori liste paylaşım isteği
/// </summary>
public class ShareWishlistRequest
{
    /// <summary>
    /// Paylaşım türü
    /// </summary>
    public string ShareType { get; set; } = string.Empty;

    /// <summary>
    /// E-posta adresi
    /// </summary>
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Paylaşım mesajı
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Süre (gün)
    /// </summary>
    public int? ExpirationDays { get; set; }
}
