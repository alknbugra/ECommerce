using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Coupons.Commands.CreateCoupon;
using ECommerce.Application.Features.Coupons.Commands.ValidateCoupon;
using ECommerce.Application.Features.Coupons.Queries.GetCoupon;
using ECommerce.Application.Features.Coupons.Queries.GetCouponByCode;
using ECommerce.Application.Features.Coupons.Queries.GetAllCoupons;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Kupon endpoint'leri
/// </summary>
public static class CouponsEndpoints
{
    /// <summary>
    /// Kupon endpoint'lerini map et
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapCouponsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/coupons")
            .WithTags("Kuponlar")
            .WithOpenApi();

        // Kupon oluştur
        group.MapPost("/", CreateCoupon)
            .WithName("CreateCoupon")
            .WithSummary("Kupon oluştur")
            .WithDescription("Yeni bir kupon oluşturur")
            .RequireAuthorization("AdminPolicy")
            .Produces<CouponDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        // Kupon güncelle
        group.MapPut("/{couponId:guid}", UpdateCoupon)
            .WithName("UpdateCoupon")
            .WithSummary("Kupon güncelle")
            .WithDescription("Mevcut bir kuponu günceller")
            .RequireAuthorization("AdminPolicy")
            .Produces<CouponDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        // Kupon sil
        group.MapDelete("/{couponId:guid}", DeleteCoupon)
            .WithName("DeleteCoupon")
            .WithSummary("Kupon sil")
            .WithDescription("Mevcut bir kuponu siler")
            .RequireAuthorization("AdminPolicy")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        // Kupon getir
        group.MapGet("/{couponId:guid}", GetCoupon)
            .WithName("GetCoupon")
            .WithSummary("Kupon getir")
            .WithDescription("ID ile kupon bilgilerini getirir")
            .RequireAuthorization("AdminPolicy")
            .Produces<CouponDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        // Kupon kodu ile kupon getir
        group.MapGet("/code/{code}", GetCouponByCode)
            .WithName("GetCouponByCode")
            .WithSummary("Kupon kodu ile kupon getir")
            .WithDescription("Kupon kodu ile kupon bilgilerini getirir")
            .RequireAuthorization()
            .Produces<CouponDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

        // Tüm kuponları getir
        group.MapGet("/", GetAllCoupons)
            .WithName("GetAllCoupons")
            .WithSummary("Tüm kuponları getir")
            .WithDescription("Tüm kuponları listeler")
            .RequireAuthorization("AdminPolicy")
            .Produces<List<CouponDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        // Aktif kuponları getir
        group.MapGet("/active", GetActiveCoupons)
            .WithName("GetActiveCoupons")
            .WithSummary("Aktif kuponları getir")
            .WithDescription("Aktif kuponları listeler")
            .RequireAuthorization()
            .Produces<List<CouponDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        // Kupon doğrula
        group.MapPost("/validate", ValidateCoupon)
            .WithName("ValidateCoupon")
            .WithSummary("Kupon doğrula")
            .WithDescription("Kupon kodunu doğrular ve indirim tutarını hesaplar")
            .RequireAuthorization()
            .Produces<CouponValidationResultDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        // Kupon kullan
        group.MapPost("/use", UseCoupon)
            .WithName("UseCoupon")
            .WithSummary("Kupon kullan")
            .WithDescription("Kuponu kullanır ve kullanım kaydını oluşturur")
            .RequireAuthorization()
            .Produces<CouponValidationResultDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        // Kupon kullanımlarını getir
        group.MapGet("/{couponId:guid}/usages", GetCouponUsages)
            .WithName("GetCouponUsages")
            .WithSummary("Kupon kullanımlarını getir")
            .WithDescription("Belirli bir kuponun kullanımlarını listeler")
            .RequireAuthorization("AdminPolicy")
            .Produces<List<CouponUsageDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        // Kupon istatistiklerini getir
        group.MapGet("/{couponId:guid}/stats", GetCouponStats)
            .WithName("GetCouponStats")
            .WithSummary("Kupon istatistiklerini getir")
            .WithDescription("Belirli bir kuponun istatistiklerini getirir")
            .RequireAuthorization("AdminPolicy")
            .Produces<CouponStatsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        // Kupon kodu oluştur
        group.MapPost("/generate-code", GenerateCouponCode)
            .WithName("GenerateCouponCode")
            .WithSummary("Kupon kodu oluştur")
            .WithDescription("Benzersiz bir kupon kodu oluşturur")
            .RequireAuthorization("AdminPolicy")
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
    }

    /// <summary>
    /// Kupon oluştur
    /// </summary>
    private static async Task<IResult> CreateCoupon(
        [FromBody] CreateCouponDto createCouponDto,
        [FromServices] ICommandHandler<CreateCouponCommand, CouponDto?> handler,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userIdGuid = Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : (Guid?)null;

        var command = new CreateCouponCommand
        {
            CreateCouponDto = createCouponDto,
            CreatedByUserId = userIdGuid
        };

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);
        
        return Results.Created($"/api/coupons/{result.Value.Id}", result.Value);
    }

    /// <summary>
    /// Kupon güncelle
    /// </summary>
    private static async Task<IResult> UpdateCoupon(
        Guid couponId,
        [FromBody] CreateCouponDto updateCouponDto,
        [FromServices] ICouponService couponService)
    {
        var result = await couponService.UpdateCouponAsync(couponId, updateCouponDto);
        return result != null ? Results.Ok(result) : Results.NotFound("Kupon bulunamadı");
    }

    /// <summary>
    /// Kupon sil
    /// </summary>
    private static async Task<IResult> DeleteCoupon(
        Guid couponId,
        [FromServices] ICouponService couponService)
    {
        var result = await couponService.DeleteCouponAsync(couponId);
        return result ? Results.NoContent() : Results.NotFound("Kupon bulunamadı");
    }

    /// <summary>
    /// Kupon getir
    /// </summary>
    private static async Task<IResult> GetCoupon(
        Guid couponId,
        [FromServices] IQueryHandler<GetCouponQuery, CouponDto?> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCouponQuery { CouponId = couponId };
        var result = await handler.Handle(query, cancellationToken);
        return result != null ? Results.Ok(result) : Results.NotFound("Kupon bulunamadı");
    }

    /// <summary>
    /// Kupon kodu ile kupon getir
    /// </summary>
    private static async Task<IResult> GetCouponByCode(
        string code,
        [FromServices] IQueryHandler<GetCouponByCodeQuery, CouponDto?> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCouponByCodeQuery { Code = code };
        var result = await handler.Handle(query, cancellationToken);
        return result != null ? Results.Ok(result) : Results.NotFound("Kupon bulunamadı");
    }

    /// <summary>
    /// Tüm kuponları getir
    /// </summary>
    private static async Task<IResult> GetAllCoupons(
        [FromQuery] bool? isActive,
        [FromServices] IQueryHandler<GetAllCouponsQuery, List<CouponDto>> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllCouponsQuery { IsActive = isActive };
        var result = await handler.Handle(query, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Aktif kuponları getir
    /// </summary>
    private static async Task<IResult> GetActiveCoupons(
        [FromServices] ICouponService couponService)
    {
        var result = await couponService.GetActiveCouponsAsync();
        return Results.Ok(result);
    }

    /// <summary>
    /// Kupon doğrula
    /// </summary>
    private static async Task<IResult> ValidateCoupon(
        [FromBody] ValidateCouponDto validateCouponDto,
        [FromServices] ICommandHandler<ValidateCouponCommand, CouponValidationResultDto> handler,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        validateCouponDto.UserId = userIdGuid;

        var command = new ValidateCouponCommand { ValidateCouponDto = validateCouponDto };
        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Kupon kullan
    /// </summary>
    private static async Task<IResult> UseCoupon(
        [FromBody] UseCouponDto useCouponDto,
        [FromServices] ICouponService couponService,
        ClaimsPrincipal user,
        HttpContext context)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Results.Unauthorized();
        }

        var userIpAddress = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers.UserAgent.ToString();

        var result = await couponService.UseCouponAsync(
            useCouponDto.Code,
            userIdGuid,
            useCouponDto.OrderId,
            useCouponDto.OrderAmount,
            userIpAddress,
            userAgent);

        return Results.Ok(result);
    }

    /// <summary>
    /// Kupon kullanımlarını getir
    /// </summary>
    private static async Task<IResult> GetCouponUsages(
        Guid couponId,
        [FromQuery] Guid? userId,
        [FromServices] ICouponService couponService)
    {
        var result = await couponService.GetCouponUsagesAsync(couponId, userId);
        return Results.Ok(result);
    }

    /// <summary>
    /// Kupon istatistiklerini getir
    /// </summary>
    private static async Task<IResult> GetCouponStats(
        Guid couponId,
        [FromServices] ICouponService couponService)
    {
        var result = await couponService.GetCouponStatsAsync(couponId);
        return Results.Ok(result);
    }

    /// <summary>
    /// Kupon kodu oluştur
    /// </summary>
    private static async Task<IResult> GenerateCouponCode(
        [FromBody] GenerateCouponCodeDto generateDto,
        [FromServices] ICouponService couponService)
    {
        var result = await couponService.GenerateCouponCodeAsync(
            generateDto.Length,
            generateDto.Prefix);
        return Results.Ok(new { Code = result });
    }
}

/// <summary>
/// Kupon kullanma DTO'su
/// </summary>
public class UseCouponDto
{
    /// <summary>
    /// Kupon kodu
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Sipariş tutarı
    /// </summary>
    public decimal OrderAmount { get; set; }
}

/// <summary>
/// Kupon kodu oluşturma DTO'su
/// </summary>
public class GenerateCouponCodeDto
{
    /// <summary>
    /// Kod uzunluğu
    /// </summary>
    public int Length { get; set; } = 8;

    /// <summary>
    /// Ön ek
    /// </summary>
    public string? Prefix { get; set; }
}
