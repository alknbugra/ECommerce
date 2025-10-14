using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ECommerce.Infrastructure.Services.Email;

/// <summary>
/// SMTP Email servis implementasyonu
/// </summary>
public class SmtpEmailService : IEmailService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpEmailService> _logger;

    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly bool _smtpEnableSsl;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public SmtpEmailService(
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<SmtpEmailService> logger)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;

        _smtpHost = _configuration["Email:Smtp:Host"] ?? throw new ArgumentNullException("SMTP Host not configured");
        _smtpPort = _configuration.GetValue<int>("Email:Smtp:Port", 587);
        _smtpUsername = _configuration["Email:Smtp:Username"] ?? throw new ArgumentNullException("SMTP Username not configured");
        _smtpPassword = _configuration["Email:Smtp:Password"] ?? throw new ArgumentNullException("SMTP Password not configured");
        _smtpEnableSsl = _configuration.GetValue<bool>("Email:Smtp:EnableSsl", true);
        _fromEmail = _configuration["Email:From:Email"] ?? throw new ArgumentNullException("From Email not configured");
        _fromName = _configuration["Email:From:Name"] ?? "E-Commerce";
    }

    public async Task<bool> SendEmailAsync(EmailDto emailDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Email gönderiliyor. To: {ToEmail}, Subject: {Subject}", 
                emailDto.ToEmail, emailDto.Subject);

            // Email log oluştur
            var emailLog = new EmailLog
            {
                ToEmail = emailDto.ToEmail,
                ToName = emailDto.ToName,
                FromEmail = emailDto.FromEmail ?? _fromEmail,
                FromName = emailDto.FromName ?? _fromName,
                Subject = emailDto.Subject,
                Content = emailDto.Content,
                Status = "Pending",
                EmailType = emailDto.EmailType,
                RelatedEntityId = emailDto.RelatedEntityId,
                RelatedEntityType = emailDto.RelatedEntityType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.EmailLogs.AddAsync(emailLog);
            await _unitOfWork.SaveChangesAsync();

            // SMTP ile email gönder
            var success = await SendSmtpEmailAsync(emailDto, cancellationToken);

            // Email log'u güncelle
            emailLog.Status = success ? "Sent" : "Failed";
            emailLog.SentAt = success ? DateTime.UtcNow : (DateTime?)null;
            emailLog.UpdatedAt = DateTime.UtcNow;

            if (!success)
            {
                emailLog.ErrorMessage = "SMTP gönderim hatası";
                emailLog.RetryCount++;
                emailLog.NextRetryAt = DateTime.UtcNow.AddMinutes(5);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Email gönderim sonucu. To: {ToEmail}, Success: {Success}", 
                emailDto.ToEmail, success);

            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email gönderim sırasında hata oluştu. To: {ToEmail}", emailDto.ToEmail);
            return false;
        }
    }

    public async Task<bool> SendEmailWithTemplateAsync(SendEmailDto sendEmailDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Şablonlu email gönderiliyor. To: {ToEmail}, Type: {EmailType}", 
                sendEmailDto.ToEmail, sendEmailDto.EmailType);

            // Email şablonunu al
            var template = await GetEmailTemplateAsync(sendEmailDto.EmailType, cancellationToken);
            if (template == null)
            {
                _logger.LogWarning("Email şablonu bulunamadı. Type: {EmailType}", sendEmailDto.EmailType);
                return false;
            }

            // Şablonu değişkenlerle doldur
            var subject = ProcessTemplate(template.Subject, sendEmailDto.Variables);
            var content = ProcessTemplate(template.Content, sendEmailDto.Variables);
            var plainTextContent = !string.IsNullOrEmpty(template.PlainTextContent) 
                ? ProcessTemplate(template.PlainTextContent, sendEmailDto.Variables) 
                : null;

            // Email DTO'su oluştur
            var emailDto = new EmailDto
            {
                ToEmail = sendEmailDto.ToEmail,
                ToName = sendEmailDto.ToName,
                FromEmail = _fromEmail,
                FromName = _fromName,
                Subject = subject,
                Content = content,
                PlainTextContent = plainTextContent,
                EmailType = sendEmailDto.EmailType,
                RelatedEntityId = sendEmailDto.RelatedEntityId,
                RelatedEntityType = sendEmailDto.RelatedEntityType,
                Variables = sendEmailDto.Variables
            };

            return await SendEmailAsync(emailDto, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Şablonlu email gönderim sırasında hata oluştu. To: {ToEmail}", sendEmailDto.ToEmail);
            return false;
        }
    }

    public async Task<List<bool>> SendBulkEmailAsync(List<EmailDto> emailDtos, CancellationToken cancellationToken = default)
    {
        var results = new List<bool>();

        foreach (var emailDto in emailDtos)
        {
            var result = await SendEmailAsync(emailDto, cancellationToken);
            results.Add(result);
        }

        return results;
    }

    public async Task<EmailTemplateDto?> GetEmailTemplateAsync(string templateCode, CancellationToken cancellationToken = default)
    {
        var template = await _unitOfWork.EmailTemplates.FirstOrDefaultAsync(
            t => t.Code == templateCode && t.IsActive);

        if (template == null)
            return null;

        return new EmailTemplateDto
        {
            Id = template.Id,
            Name = template.Name,
            Code = template.Code,
            Subject = template.Subject,
            Content = template.Content,
            PlainTextContent = template.PlainTextContent,
            Category = template.Category,
            IsActive = template.IsActive,
            Variables = template.Variables,
            Description = template.Description,
            CreatedAt = template.CreatedAt,
            UpdatedAt = template.UpdatedAt
        };
    }

    public string ProcessTemplate(string template, Dictionary<string, object>? variables)
    {
        if (string.IsNullOrEmpty(template) || variables == null || !variables.Any())
            return template;

        var result = template;

        foreach (var variable in variables)
        {
            var placeholder = $"{{{variable.Key}}}";
            var value = variable.Value?.ToString() ?? string.Empty;
            result = result.Replace(placeholder, value);
        }

        return result;
    }

    public async Task<string> GetEmailStatusAsync(Guid emailLogId, CancellationToken cancellationToken = default)
    {
        var emailLog = await _unitOfWork.EmailLogs.GetByIdAsync(emailLogId);
        return emailLog?.Status ?? "NotFound";
    }

    public async Task<int> RetryFailedEmailsAsync(CancellationToken cancellationToken = default)
    {
        var failedEmails = await _unitOfWork.EmailLogs.GetAll()
            .Where(e => e.Status == "Failed" && e.RetryCount < e.MaxRetryCount)
            .Where(e => e.NextRetryAt == null || e.NextRetryAt <= DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        var retryCount = 0;

        foreach (var emailLog in failedEmails)
        {
            try
            {
                var emailDto = new EmailDto
                {
                    ToEmail = emailLog.ToEmail,
                    ToName = emailLog.ToName,
                    FromEmail = emailLog.FromEmail,
                    FromName = emailLog.FromName,
                    Subject = emailLog.Subject,
                    Content = emailLog.Content,
                    EmailType = emailLog.EmailType,
                    RelatedEntityId = emailLog.RelatedEntityId,
                    RelatedEntityType = emailLog.RelatedEntityType
                };

                var success = await SendSmtpEmailAsync(emailDto, cancellationToken);

                emailLog.Status = success ? "Sent" : "Failed";
                emailLog.SentAt = success ? DateTime.UtcNow : (DateTime?)null;
                emailLog.RetryCount++;
                emailLog.UpdatedAt = DateTime.UtcNow;

                if (!success)
                {
                    emailLog.ErrorMessage = $"Retry {emailLog.RetryCount} failed";
                    emailLog.NextRetryAt = DateTime.UtcNow.AddMinutes(Math.Pow(2, emailLog.RetryCount));
                }

                retryCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email retry sırasında hata oluştu. EmailLogId: {EmailLogId}", emailLog.Id);
            }
        }

        await _unitOfWork.SaveChangesAsync();
        return retryCount;
    }

    private async Task<bool> SendSmtpEmailAsync(EmailDto emailDto, CancellationToken cancellationToken = default)
    {
        try
        {
            using var smtpClient = new SmtpClient(_smtpHost, _smtpPort);
            smtpClient.EnableSsl = _smtpEnableSsl;
            smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);

            using var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailDto.FromEmail, emailDto.FromName);
            mailMessage.To.Add(new MailAddress(emailDto.ToEmail, emailDto.ToName));
            mailMessage.Subject = emailDto.Subject;
            mailMessage.Body = emailDto.Content;
            mailMessage.IsBodyHtml = true;

            if (!string.IsNullOrEmpty(emailDto.PlainTextContent))
            {
                var plainTextView = AlternateView.CreateAlternateViewFromString(
                    emailDto.PlainTextContent, Encoding.UTF8, "text/plain");
                mailMessage.AlternateViews.Add(plainTextView);
            }

            await smtpClient.SendMailAsync(mailMessage, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SMTP email gönderim hatası. To: {ToEmail}", emailDto.ToEmail);
            return false;
        }
    }
}
