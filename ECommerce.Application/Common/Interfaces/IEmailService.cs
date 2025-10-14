using ECommerce.Application.DTOs;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Email servis interface'i
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Email gönder
    /// </summary>
    /// <param name="emailDto">Email DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Gönderim sonucu</returns>
    Task<bool> SendEmailAsync(EmailDto emailDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Şablon kullanarak email gönder
    /// </summary>
    /// <param name="sendEmailDto">Email gönderme DTO'su</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Gönderim sonucu</returns>
    Task<bool> SendEmailWithTemplateAsync(SendEmailDto sendEmailDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Toplu email gönder
    /// </summary>
    /// <param name="emailDtos">Email DTO'ları</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Gönderim sonuçları</returns>
    Task<List<bool>> SendBulkEmailAsync(List<EmailDto> emailDtos, CancellationToken cancellationToken = default);

    /// <summary>
    /// Email şablonu al
    /// </summary>
    /// <param name="templateCode">Şablon kodu</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Email şablonu</returns>
    Task<EmailTemplateDto?> GetEmailTemplateAsync(string templateCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Şablon içeriğini değişkenlerle doldur
    /// </summary>
    /// <param name="template">Şablon içeriği</param>
    /// <param name="variables">Değişkenler</param>
    /// <returns>Doldurulmuş içerik</returns>
    string ProcessTemplate(string template, Dictionary<string, object>? variables);

    /// <summary>
    /// Email durumunu kontrol et
    /// </summary>
    /// <param name="emailLogId">Email log ID'si</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Email durumu</returns>
    Task<string> GetEmailStatusAsync(Guid emailLogId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Başarısız emailleri yeniden gönder
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Yeniden gönderim sonucu</returns>
    Task<int> RetryFailedEmailsAsync(CancellationToken cancellationToken = default);
}
