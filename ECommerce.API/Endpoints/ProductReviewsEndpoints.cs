using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.ProductReviews.Commands.CreateProductReview;
using ECommerce.Application.Features.ProductReviews.Commands.ApproveProductReview;
using ECommerce.Application.Features.ProductReviews.Commands.RejectProductReview;
using ECommerce.Application.Features.ProductReviews.Commands.VoteProductReview;
using ECommerce.Application.Features.ProductReviews.Queries.GetProductReviews;
using ECommerce.Application.Features.ProductReviews.Queries.GetProductReviewStats;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Ürün değerlendirme endpoint'leri
/// </summary>
public static class ProductReviewsEndpoints
{
    /// <summary>
    /// Ürün değerlendirme endpoint'lerini map et
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapProductReviewsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/product-reviews")
            .WithTags("Ürün Değerlendirmeleri")
            .WithOpenApi();

        // Ürün değerlendirmesi oluştur
        group.MapPost("/", CreateProductReview)
            .WithName("CreateProductReview")
            .WithSummary("Ürün değerlendirmesi oluştur")
            .WithDescription("Yeni bir ürün değerlendirmesi oluşturur")
            .RequireAuthorization()
            .Produces<ProductReviewDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        // Ürün değerlendirmesi güncelle
        group.MapPut("/{reviewId:guid}", UpdateProductReview)
            .WithName("UpdateProductReview")
            .WithSummary("Ürün değerlendirmesi güncelle")
            .WithDescription("Mevcut bir ürün değerlendirmesini günceller")
            .RequireAuthorization()
            .Produces<ProductReviewDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

        // Ürün değerlendirmesi sil
        group.MapDelete("/{reviewId:guid}", DeleteProductReview)
            .WithName("DeleteProductReview")
            .WithSummary("Ürün değerlendirmesi sil")
            .WithDescription("Mevcut bir ürün değerlendirmesini siler")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

        // Ürün değerlendirmesi onayla
        group.MapPost("/{reviewId:guid}/approve", ApproveProductReview)
            .WithName("ApproveProductReview")
            .WithSummary("Ürün değerlendirmesi onayla")
            .WithDescription("Bekleyen bir ürün değerlendirmesini onaylar")
            .RequireAuthorization("AdminPolicy")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        // Ürün değerlendirmesi reddet
        group.MapPost("/{reviewId:guid}/reject", RejectProductReview)
            .WithName("RejectProductReview")
            .WithSummary("Ürün değerlendirmesi reddet")
            .WithDescription("Bekleyen bir ürün değerlendirmesini reddeder")
            .RequireAuthorization("AdminPolicy")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        // Ürün değerlendirmesi oyla
        group.MapPost("/{reviewId:guid}/vote", VoteProductReview)
            .WithName("VoteProductReview")
            .WithSummary("Ürün değerlendirmesi oyla")
            .WithDescription("Bir ürün değerlendirmesine yararlı/yararsız oyu verir")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        // Ürün değerlendirmesi oyunu kaldır
        group.MapDelete("/{reviewId:guid}/vote", RemoveVote)
            .WithName("RemoveVote")
            .WithSummary("Ürün değerlendirmesi oyunu kaldır")
            .WithDescription("Bir ürün değerlendirmesine verilen oyu kaldırır")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        // Ürün değerlendirmesi getir
        group.MapGet("/{reviewId:guid}", GetProductReview)
            .WithName("GetProductReview")
            .WithSummary("Ürün değerlendirmesi getir")
            .WithDescription("ID ile ürün değerlendirmesi bilgilerini getirir")
            .RequireAuthorization()
            .Produces<ProductReviewDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

        // Ürün değerlendirmelerini getir
        group.MapGet("/product/{productId:guid}", GetProductReviews)
            .WithName("GetProductReviews")
            .WithSummary("Ürün değerlendirmelerini getir")
            .WithDescription("Belirli bir ürünün değerlendirmelerini listeler")
            .RequireAuthorization()
            .Produces<List<ProductReviewDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        // Kullanıcı değerlendirmelerini getir
        group.MapGet("/user/{userId:guid}", GetUserReviews)
            .WithName("GetUserReviews")
            .WithSummary("Kullanıcı değerlendirmelerini getir")
            .WithDescription("Belirli bir kullanıcının değerlendirmelerini listeler")
            .RequireAuthorization("AdminPolicy")
            .Produces<List<ProductReviewDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        // Bekleyen değerlendirmeleri getir
        group.MapGet("/pending", GetPendingReviews)
            .WithName("GetPendingReviews")
            .WithSummary("Bekleyen değerlendirmeleri getir")
            .WithDescription("Onay bekleyen değerlendirmeleri listeler")
            .RequireAuthorization("AdminPolicy")
            .Produces<List<ProductReviewDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        // Ürün değerlendirme istatistiklerini getir
        group.MapGet("/product/{productId:guid}/stats", GetProductReviewStats)
            .WithName("GetProductReviewStats")
            .WithSummary("Ürün değerlendirme istatistiklerini getir")
            .WithDescription("Belirli bir ürünün değerlendirme istatistiklerini getirir")
            .RequireAuthorization()
            .Produces<ProductReviewStatsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        // Değerlendirme yanıtı ekle
        group.MapPost("/{reviewId:guid}/response", AddReviewResponse)
            .WithName("AddReviewResponse")
            .WithSummary("Değerlendirme yanıtı ekle")
            .WithDescription("Bir değerlendirmeye yanıt ekler")
            .RequireAuthorization()
            .Produces<ReviewResponseDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        // Değerlendirme yanıtını güncelle
        group.MapPut("/response/{responseId:guid}", UpdateReviewResponse)
            .WithName("UpdateReviewResponse")
            .WithSummary("Değerlendirme yanıtını güncelle")
            .WithDescription("Mevcut bir değerlendirme yanıtını günceller")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

        // Değerlendirme yanıtını sil
        group.MapDelete("/response/{responseId:guid}", DeleteReviewResponse)
            .WithName("DeleteReviewResponse")
            .WithSummary("Değerlendirme yanıtını sil")
            .WithDescription("Mevcut bir değerlendirme yanıtını siler")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Ürün değerlendirmesi oluştur
    /// </summary>
    private static async Task<IResult> CreateProductReview(
        [FromBody] CreateProductReviewDto createReviewDto,
        [FromServices] ICommandHandler<CreateProductReviewCommand, ProductReviewDto?> handler,
        ClaimsPrincipal user,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        var userIpAddress = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers.UserAgent.ToString();

        var command = new CreateProductReviewCommand
        {
            CreateReviewDto = createReviewDto,
            UserId = userIdGuid,
            UserIpAddress = userIpAddress,
            UserAgent = userAgent
        };

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);
        
        return Results.Created($"/api/product-reviews/{result.Value.Id}", result.Value);
    }

    /// <summary>
    /// Ürün değerlendirmesi güncelle
    /// </summary>
    private static async Task<IResult> UpdateProductReview(
        Guid reviewId,
        [FromBody] CreateProductReviewDto updateReviewDto,
        [FromServices] IProductReviewService productReviewService,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        var result = await productReviewService.UpdateReviewAsync(reviewId, updateReviewDto, userIdGuid, cancellationToken);
        return result != null ? Results.Ok(result) : Results.NotFound("Değerlendirme bulunamadı");
    }

    /// <summary>
    /// Ürün değerlendirmesi sil
    /// </summary>
    private static async Task<IResult> DeleteProductReview(
        Guid reviewId,
        [FromServices] IProductReviewService productReviewService,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        var result = await productReviewService.DeleteReviewAsync(reviewId, userIdGuid, cancellationToken);
        return result ? Results.NoContent() : Results.NotFound("Değerlendirme bulunamadı");
    }

    /// <summary>
    /// Ürün değerlendirmesi onayla
    /// </summary>
    private static async Task<IResult> ApproveProductReview(
        Guid reviewId,
        [FromServices] ICommandHandler<ApproveProductReviewCommand, bool> handler,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        var command = new ApproveProductReviewCommand
        {
            ReviewId = reviewId,
            ApprovedByUserId = userIdGuid
        };

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);
        
        return Results.Ok(new { Message = "Değerlendirme onaylandı", Success = result.Value });
    }

    /// <summary>
    /// Ürün değerlendirmesi reddet
    /// </summary>
    private static async Task<IResult> RejectProductReview(
        Guid reviewId,
        [FromBody] RejectReviewDto rejectDto,
        [FromServices] ICommandHandler<RejectProductReviewCommand, bool> handler,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        var command = new RejectProductReviewCommand
        {
            ReviewId = reviewId,
            RejectionReason = rejectDto.RejectionReason,
            RejectedByUserId = userIdGuid
        };

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);
        
        return Results.Ok(new { Message = "Değerlendirme reddedildi", Success = result.Value });
    }

    /// <summary>
    /// Ürün değerlendirmesi oyla
    /// </summary>
    private static async Task<IResult> VoteProductReview(
        Guid reviewId,
        [FromBody] VoteReviewDto voteDto,
        [FromServices] ICommandHandler<VoteProductReviewCommand, bool> handler,
        ClaimsPrincipal user,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        var userIpAddress = context.Connection.RemoteIpAddress?.ToString();

        var command = new VoteProductReviewCommand
        {
            ReviewId = reviewId,
            UserId = userIdGuid,
            VoteType = voteDto.VoteType,
            UserIpAddress = userIpAddress
        };

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);
        
        return Results.Ok(new { Message = "Oy verildi", Success = result.Value });
    }

    /// <summary>
    /// Ürün değerlendirmesi oyunu kaldır
    /// </summary>
    private static async Task<IResult> RemoveVote(
        Guid reviewId,
        [FromServices] IProductReviewService productReviewService,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        var result = await productReviewService.RemoveVoteAsync(reviewId, userIdGuid, cancellationToken);
        return result ? Results.Ok(new { Message = "Oy kaldırıldı" }) : Results.BadRequest("Oy kaldırılamadı");
    }

    /// <summary>
    /// Ürün değerlendirmesi getir
    /// </summary>
    private static async Task<IResult> GetProductReview(
        Guid reviewId,
        [FromServices] IProductReviewService productReviewService,
        CancellationToken cancellationToken = default)
    {
        var result = await productReviewService.GetReviewAsync(reviewId, cancellationToken);
        return result != null ? Results.Ok(result) : Results.NotFound("Değerlendirme bulunamadı");
    }

    /// <summary>
    /// Ürün değerlendirmelerini getir
    /// </summary>
    private static async Task<IResult> GetProductReviews(
        Guid productId,
        [AsParameters] GetProductReviewsRequest request,
        [FromServices] IQueryHandler<GetProductReviewsQuery, List<ProductReviewDto>> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProductReviewsQuery
        {
            ProductId = productId,
            IsApproved = request.IsApproved,
            Rating = request.Rating,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var result = await handler.Handle(query, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Kullanıcı değerlendirmelerini getir
    /// </summary>
    private static async Task<IResult> GetUserReviews(
        Guid userId,
        [FromServices] IProductReviewService productReviewService,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await productReviewService.GetUserReviewsAsync(userId, pageNumber, pageSize, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Bekleyen değerlendirmeleri getir
    /// </summary>
    private static async Task<IResult> GetPendingReviews(
        [FromServices] IProductReviewService productReviewService,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await productReviewService.GetPendingReviewsAsync(pageNumber, pageSize, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Ürün değerlendirme istatistiklerini getir
    /// </summary>
    private static async Task<IResult> GetProductReviewStats(
        Guid productId,
        [FromServices] IQueryHandler<GetProductReviewStatsQuery, ProductReviewStatsDto> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProductReviewStatsQuery { ProductId = productId };
        var result = await handler.Handle(query, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Değerlendirme yanıtı ekle
    /// </summary>
    private static async Task<IResult> AddReviewResponse(
        Guid reviewId,
        [FromBody] AddReviewResponseDto addResponseDto,
        [FromServices] IProductReviewService productReviewService,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        var result = await productReviewService.AddReviewResponseAsync(
            reviewId,
            addResponseDto.Content,
            userIdGuid,
            addResponseDto.ResponseType,
            cancellationToken);

        return result != null ? Results.Created($"/api/product-reviews/response/{result.Id}", result) : Results.BadRequest("Yanıt eklenemedi");
    }

    /// <summary>
    /// Değerlendirme yanıtını güncelle
    /// </summary>
    private static async Task<IResult> UpdateReviewResponse(
        Guid responseId,
        [FromBody] UpdateReviewResponseDto updateResponseDto,
        [FromServices] IProductReviewService productReviewService,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        var result = await productReviewService.UpdateReviewResponseAsync(responseId, updateResponseDto.Content, userIdGuid, cancellationToken);
        return result ? Results.Ok(new { Message = "Yanıt güncellendi" }) : Results.NotFound("Yanıt bulunamadı");
    }

    /// <summary>
    /// Değerlendirme yanıtını sil
    /// </summary>
    private static async Task<IResult> DeleteReviewResponse(
        Guid responseId,
        [FromServices] IProductReviewService productReviewService,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        var result = await productReviewService.DeleteReviewResponseAsync(responseId, userIdGuid, cancellationToken);
        return result ? Results.NoContent() : Results.NotFound("Yanıt bulunamadı");
    }
}

/// <summary>
/// Ürün değerlendirmelerini getirme isteği
/// </summary>
public class GetProductReviewsRequest
{
    /// <summary>
    /// Onaylanmış değerlendirmeler filtresi (opsiyonel)
    /// </summary>
    public bool? IsApproved { get; set; }

    /// <summary>
    /// Puan filtresi (opsiyonel)
    /// </summary>
    public int? Rating { get; set; }

    /// <summary>
    /// Sayfa numarası
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Sıralama kriteri
    /// </summary>
    public string SortBy { get; set; } = "CreatedAt";

    /// <summary>
    /// Sıralama yönü
    /// </summary>
    public string SortDirection { get; set; } = "desc";
}

/// <summary>
/// Değerlendirme reddetme DTO'su
/// </summary>
public class RejectReviewDto
{
    /// <summary>
    /// Red sebebi
    /// </summary>
    public string RejectionReason { get; set; } = string.Empty;
}

/// <summary>
/// Değerlendirme oylama DTO'su
/// </summary>
public class VoteReviewDto
{
    /// <summary>
    /// Oy türü
    /// </summary>
    public string VoteType { get; set; } = string.Empty;
}

/// <summary>
/// Değerlendirme yanıtı ekleme DTO'su
/// </summary>
public class AddReviewResponseDto
{
    /// <summary>
    /// Yanıt içeriği
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Yanıt türü
    /// </summary>
    public string ResponseType { get; set; } = "Seller";
}

/// <summary>
/// Değerlendirme yanıtı güncelleme DTO'su
/// </summary>
public class UpdateReviewResponseDto
{
    /// <summary>
    /// Yanıt içeriği
    /// </summary>
    public string Content { get; set; } = string.Empty;
}
