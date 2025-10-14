using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Emails.Commands.SendEmail;

/// <summary>
/// Email gönderme komut handler'ı
/// </summary>
public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand, bool>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailCommandHandler> _logger;

    public SendEmailCommandHandler(
        IEmailService emailService,
        ILogger<SendEmailCommandHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Email gönderme işlemi başlatıldı. To: {ToEmail}, Type: {EmailType}", 
            request.ToEmail, request.EmailType);

        try
        {
            var sendEmailDto = new SendEmailDto
            {
                ToEmail = request.ToEmail,
                ToName = request.ToName,
                EmailType = request.EmailType,
                RelatedEntityId = request.RelatedEntityId,
                RelatedEntityType = request.RelatedEntityType,
                Variables = request.Variables
            };

            var result = await _emailService.SendEmailWithTemplateAsync(sendEmailDto, cancellationToken);

            _logger.LogInformation("Email gönderme işlemi tamamlandı. To: {ToEmail}, Success: {Success}", 
                request.ToEmail, result);

            return Result.Success<bool>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email gönderme işlemi sırasında hata oluştu. To: {ToEmail}", request.ToEmail);
            return Result.Failure<bool>(Error.Problem("Email.SendError", "Email gönderme işlemi sırasında bir hata oluştu."));
        }
    }
}
