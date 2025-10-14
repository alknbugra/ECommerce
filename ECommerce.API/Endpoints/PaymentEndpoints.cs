using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Payments.Commands.CreatePayment;
using ECommerce.Application.Features.Payments.Commands.Verify3DSecure;
using ECommerce.Application.Features.Payments.Commands.CancelPayment;
using ECommerce.Application.Features.Payments.Commands.RefundPayment;
using ECommerce.Application.Features.Payments.Commands.ProcessWebhook;
using ECommerce.Application.Features.Payments.Queries.GetPaymentStatus;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Ödeme endpoint'leri
/// </summary>
public static class PaymentEndpoints
{
    /// <summary>
    /// Ödeme endpoint'lerini kaydet
    /// </summary>
    public static void MapPaymentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/payments")
            .WithTags("Payments")
            .WithOpenApi();

        // Ödeme oluştur
        group.MapPost("/", CreatePayment)
            .WithName("CreatePayment")
            .WithSummary("Ödeme oluştur")
            .WithDescription("Yeni bir ödeme işlemi başlatır")
            .Produces<PaymentResultDto>(200)
            .Produces(400)
            .Produces(404);

        // 3D Secure doğrula
        group.MapPost("/verify-3d-secure", Verify3DSecure)
            .WithName("Verify3DSecure")
            .WithSummary("3D Secure doğrula")
            .WithDescription("3D Secure doğrulamasını tamamlar")
            .Produces<PaymentResultDto>(200)
            .Produces(400)
            .Produces(404);

        // Ödeme durumu sorgula
        group.MapGet("/status/{paymentId:guid}", GetPaymentStatus)
            .WithName("GetPaymentStatus")
            .WithSummary("Ödeme durumu sorgula")
            .WithDescription("Ödeme durumunu sorgular")
            .Produces<PaymentDto>(200)
            .Produces(404);

        // Ödeme iptal et
        group.MapPost("/cancel/{paymentId:guid}", CancelPayment)
            .WithName("CancelPayment")
            .WithSummary("Ödeme iptal et")
            .WithDescription("Ödemeyi iptal eder")
            .Produces<PaymentResultDto>(200)
            .Produces(404);

        // Ödeme iade et
        group.MapPost("/refund/{paymentId:guid}", RefundPayment)
            .WithName("RefundPayment")
            .WithSummary("Ödeme iade et")
            .WithDescription("Ödemeyi iade eder")
            .Produces<PaymentResultDto>(200)
            .Produces(404);

        // Webhook endpoint
        group.MapPost("/webhook", ProcessWebhook)
            .WithName("ProcessWebhook")
            .WithSummary("Payment Gateway webhook")
            .WithDescription("Payment Gateway'den gelen webhook'ları işler")
            .Produces(200)
            .Produces(400);
    }

    /// <summary>
    /// Ödeme oluştur
    /// </summary>
    private static async Task<IResult> CreatePayment(
        CreatePaymentDto dto,
        [FromServices] ICommandHandler<CreatePaymentCommand, PaymentResultDto> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreatePaymentCommand
        {
            OrderId = dto.OrderId,
            PaymentMethod = dto.PaymentMethod,
            CardNumber = dto.CardNumber,
            CardHolderName = dto.CardHolderName,
            ExpiryMonth = dto.ExpiryMonth,
            ExpiryYear = dto.ExpiryYear,
            Cvv = dto.Cvv,
            InstallmentCount = dto.InstallmentCount,
            Use3DSecure = dto.Use3DSecure
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// 3D Secure doğrula
    /// </summary>
    private static async Task<IResult> Verify3DSecure(
        Verify3DSecureDto dto,
        [FromServices] ICommandHandler<Verify3DSecureCommand, PaymentResultDto> handler,
        CancellationToken cancellationToken)
    {
        var command = new Verify3DSecureCommand
        {
            PaymentId = dto.PaymentId,
            ThreeDSecureResponse = dto.ThreeDSecureResponse
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Ödeme durumu sorgula
    /// </summary>
    private static async Task<IResult> GetPaymentStatus(
        Guid paymentId,
        [FromServices] IQueryHandler<GetPaymentStatusQuery, PaymentDto> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentStatusQuery
        {
            PaymentId = paymentId
        };

        var result = await handler.Handle(query, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Ödeme iptal et
    /// </summary>
    private static async Task<IResult> CancelPayment(
        Guid paymentId,
        [FromServices] ICommandHandler<CancelPaymentCommand, PaymentResultDto> handler,
        CancellationToken cancellationToken)
    {
        var command = new CancelPaymentCommand
        {
            PaymentId = paymentId
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Ödeme iade et
    /// </summary>
    private static async Task<IResult> RefundPayment(
        Guid paymentId,
        RefundPaymentDto dto,
        [FromServices] ICommandHandler<RefundPaymentCommand, PaymentResultDto> handler,
        CancellationToken cancellationToken)
    {
        var command = new RefundPaymentCommand
        {
            PaymentId = paymentId,
            RefundAmount = dto.RefundAmount,
            Reason = dto.Reason
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Webhook işle
    /// </summary>
    private static async Task<IResult> ProcessWebhook(
        HttpContext context,
        [FromServices] ICommandHandler<ProcessWebhookCommand, PaymentResultDto> handler,
        CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(context.Request.Body);
        var webhookData = await reader.ReadToEndAsync();

        var signature = context.Request.Headers["X-Signature"].FirstOrDefault() ?? string.Empty;

        var command = new ProcessWebhookCommand
        {
            WebhookData = webhookData,
            Signature = signature
        };

        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }
        else
        {
            return Results.BadRequest(result.Error);
        }
    }
}

/// <summary>
/// 3D Secure doğrulama DTO'su
/// </summary>
public class Verify3DSecureDto
{
    public Guid PaymentId { get; set; }
    public string ThreeDSecureResponse { get; set; } = string.Empty;
}

/// <summary>
/// Ödeme iade DTO'su
/// </summary>
public class RefundPaymentDto
{
    public decimal RefundAmount { get; set; }
    public string? Reason { get; set; }
}
