using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Emails.Commands.SendEmail;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Email endpoint'leri
/// </summary>
public static class EmailEndpoints
{
    /// <summary>
    /// Email endpoint'lerini kaydet
    /// </summary>
    public static void MapEmailEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/emails")
            .WithTags("Emails")
            .WithOpenApi();

        // Email gönder
        group.MapPost("/send", SendEmail)
            .WithName("SendEmail")
            .WithSummary("Email gönder")
            .WithDescription("Şablon kullanarak email gönderir")
            .Produces<bool>(200)
            .Produces(400);

        // Kullanıcı kayıt emaili gönder
        group.MapPost("/send-registration", SendUserRegistrationEmail)
            .WithName("SendUserRegistrationEmail")
            .WithSummary("Kullanıcı kayıt emaili gönder")
            .WithDescription("Yeni kullanıcı kaydı için hoş geldin emaili gönderir")
            .Produces<bool>(200)
            .Produces(400);

        // Şifre sıfırlama emaili gönder
        group.MapPost("/send-password-reset", SendPasswordResetEmail)
            .WithName("SendPasswordResetEmail")
            .WithSummary("Şifre sıfırlama emaili gönder")
            .WithDescription("Şifre sıfırlama linki ile email gönderir")
            .Produces<bool>(200)
            .Produces(400);

        // Sipariş onay emaili gönder
        group.MapPost("/send-order-confirmation", SendOrderConfirmationEmail)
            .WithName("SendOrderConfirmationEmail")
            .WithSummary("Sipariş onay emaili gönder")
            .WithDescription("Sipariş onayı için email gönderir")
            .Produces<bool>(200)
            .Produces(400);

        // Ödeme başarılı emaili gönder
        group.MapPost("/send-payment-success", SendPaymentSuccessEmail)
            .WithName("SendPaymentSuccessEmail")
            .WithSummary("Ödeme başarılı emaili gönder")
            .WithDescription("Ödeme başarılı olduğunda email gönderir")
            .Produces<bool>(200)
            .Produces(400);

        // Sipariş durumu değişiklik emaili gönder
        group.MapPost("/send-order-status-change", SendOrderStatusChangeEmail)
            .WithName("SendOrderStatusChangeEmail")
            .WithSummary("Sipariş durumu değişiklik emaili gönder")
            .WithDescription("Sipariş durumu değiştiğinde email gönderir")
            .Produces<bool>(200)
            .Produces(400);

        // Kargo bilgisi emaili gönder
        group.MapPost("/send-shipping-info", SendShippingInfoEmail)
            .WithName("SendShippingInfoEmail")
            .WithSummary("Kargo bilgisi emaili gönder")
            .WithDescription("Kargo bilgileri ile email gönderir")
            .Produces<bool>(200)
            .Produces(400);

        // Email durumu sorgula
        group.MapGet("/status/{emailLogId:guid}", GetEmailStatus)
            .WithName("GetEmailStatus")
            .WithSummary("Email durumu sorgula")
            .WithDescription("Email gönderim durumunu sorgular")
            .Produces<string>(200)
            .Produces(404);

        // Başarısız emailleri yeniden gönder
        group.MapPost("/retry-failed", RetryFailedEmails)
            .WithName("RetryFailedEmails")
            .WithSummary("Başarısız emailleri yeniden gönder")
            .WithDescription("Başarısız olan emailleri yeniden gönderir")
            .Produces<int>(200);

        // Log test endpoint'i
        group.MapGet("/test-logs", TestLogs)
            .WithName("TestLogs")
            .WithSummary("Log test endpoint'i")
            .WithDescription("Farklı log seviyelerini test eder")
            .Produces<string>(200);
    }

    /// <summary>
    /// Email gönder
    /// </summary>
    private static async Task<IResult> SendEmail(
        SendEmailDto dto,
        [FromServices] ICommandHandler<SendEmailCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        var command = new SendEmailCommand
        {
            ToEmail = dto.ToEmail,
            ToName = dto.ToName,
            EmailType = dto.EmailType,
            RelatedEntityId = dto.RelatedEntityId,
            RelatedEntityType = dto.RelatedEntityType,
            Variables = dto.Variables
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Kullanıcı kayıt emaili gönder
    /// </summary>
    private static async Task<IResult> SendUserRegistrationEmail(
        SendUserRegistrationEmailDto dto,
        IEmailService emailService,
        CancellationToken cancellationToken)
    {
        var sendEmailDto = new SendEmailDto
        {
            EmailType = "USER_REGISTRATION",
            ToEmail = dto.UserEmail,
            Variables = new Dictionary<string, object>
            {
                { "UserName", dto.UserName }
            }
        };
        var result = await emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Şifre sıfırlama emaili gönder
    /// </summary>
    private static async Task<IResult> SendPasswordResetEmail(
        SendPasswordResetEmailDto dto,
        IEmailService emailService,
        CancellationToken cancellationToken)
    {
        var sendEmailDto = new SendEmailDto
        {
            EmailType = "PASSWORD_RESET",
            ToEmail = dto.UserEmail,
            Variables = new Dictionary<string, object>
            {
                { "UserName", dto.UserName },
                { "ResetToken", dto.ResetToken }
            }
        };
        var result = await emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Sipariş onay emaili gönder
    /// </summary>
    private static async Task<IResult> SendOrderConfirmationEmail(
        OrderDto orderDto,
        IEmailService emailService,
        CancellationToken cancellationToken)
    {
        var sendEmailDto = new SendEmailDto
        {
            EmailType = "ORDER_CONFIRMATION",
            ToEmail = orderDto.UserEmail ?? "",
            Variables = new Dictionary<string, object>
            {
                { "OrderNumber", orderDto.OrderNumber },
                { "TotalAmount", orderDto.TotalAmount }
            }
        };
        var result = await emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Ödeme başarılı emaili gönder
    /// </summary>
    private static async Task<IResult> SendPaymentSuccessEmail(
        SendPaymentSuccessEmailDto dto,
        IEmailService emailService,
        CancellationToken cancellationToken)
    {
        var sendEmailDto = new SendEmailDto
        {
            EmailType = "PAYMENT_SUCCESS",
            ToEmail = dto.OrderDto.UserEmail ?? "",
            Variables = new Dictionary<string, object>
            {
                { "OrderNumber", dto.OrderDto.OrderNumber },
                { "PaymentAmount", dto.PaymentDto.Amount }
            }
        };
        var result = await emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Sipariş durumu değişiklik emaili gönder
    /// </summary>
    private static async Task<IResult> SendOrderStatusChangeEmail(
        SendOrderStatusChangeEmailDto dto,
        IEmailService emailService,
        CancellationToken cancellationToken)
    {
        var sendEmailDto = new SendEmailDto
        {
            EmailType = "ORDER_STATUS_CHANGE",
            ToEmail = dto.OrderDto.UserEmail ?? "",
            Variables = new Dictionary<string, object>
            {
                { "OrderNumber", dto.OrderDto.OrderNumber },
                { "OldStatus", dto.OldStatus },
                { "NewStatus", dto.NewStatus }
            }
        };
        var result = await emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Kargo bilgisi emaili gönder
    /// </summary>
    private static async Task<IResult> SendShippingInfoEmail(
        SendShippingInfoEmailDto dto,
        IEmailService emailService,
        CancellationToken cancellationToken)
    {
        var sendEmailDto = new SendEmailDto
        {
            EmailType = "SHIPPING_INFO",
            ToEmail = dto.OrderDto.UserEmail ?? "",
            Variables = new Dictionary<string, object>
            {
                { "OrderNumber", dto.OrderDto.OrderNumber },
                { "TrackingNumber", dto.TrackingNumber },
                { "ShippingCompany", dto.ShippingCompany }
            }
        };
        var result = await emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Email durumu sorgula
    /// </summary>
    private static async Task<IResult> GetEmailStatus(
        Guid emailLogId,
        IEmailService emailService,
        CancellationToken cancellationToken)
    {
        var status = await emailService.GetEmailStatusAsync(emailLogId, cancellationToken);
        return Results.Ok(new { status });
    }

    /// <summary>
    /// Başarısız emailleri yeniden gönder
    /// </summary>
    private static async Task<IResult> RetryFailedEmails(
        IEmailService emailService,
        CancellationToken cancellationToken)
    {
        var retryCount = await emailService.RetryFailedEmailsAsync(cancellationToken);
        return Results.Ok(new { retryCount });
    }

    /// <summary>
    /// Log test endpoint'i
    /// </summary>
    private static IResult TestLogs(ILogger<Program> logger)
    {
        var testId = Guid.NewGuid();
        
        logger.LogDebug("Debug log test - TestId: {TestId}", testId);
        logger.LogInformation("Information log test - TestId: {TestId}", testId);
        logger.LogWarning("Warning log test - TestId: {TestId}", testId);
        logger.LogError("Error log test - TestId: {TestId}", testId);
        
        // Structured logging örneği
        logger.LogInformation("Structured log test - TestId: {TestId}, Timestamp: {Timestamp}, Level: {Level}", 
            testId, DateTime.UtcNow, "Information");
        
        return Results.Ok(new { 
            message = "Log test completed", 
            testId = testId,
            timestamp = DateTime.UtcNow,
            logs = new[] { "Debug", "Information", "Warning", "Error" }
        });
    }
}

/// <summary>
/// Kullanıcı kayıt emaili DTO'su
/// </summary>
public class SendUserRegistrationEmailDto
{
    public string UserEmail { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}

/// <summary>
/// Şifre sıfırlama emaili DTO'su
/// </summary>
public class SendPasswordResetEmailDto
{
    public string UserEmail { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string ResetToken { get; set; } = string.Empty;
}

/// <summary>
/// Ödeme başarılı emaili DTO'su
/// </summary>
public class SendPaymentSuccessEmailDto
{
    public PaymentDto PaymentDto { get; set; } = null!;
    public OrderDto OrderDto { get; set; } = null!;
}

/// <summary>
/// Sipariş durumu değişiklik emaili DTO'su
/// </summary>
public class SendOrderStatusChangeEmailDto
{
    public OrderDto OrderDto { get; set; } = null!;
    public string OldStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
}

/// <summary>
/// Kargo bilgisi emaili DTO'su
/// </summary>
public class SendShippingInfoEmailDto
{
    public OrderDto OrderDto { get; set; } = null!;
    public string TrackingNumber { get; set; } = string.Empty;
    public string ShippingCompany { get; set; } = string.Empty;
}
