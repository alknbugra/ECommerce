namespace ECommerce.Application.DTOs;

/// <summary>
/// Değerlendirme oyu DTO'su
/// </summary>
public class ReviewVoteDto
{
    /// <summary>
    /// Oy ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Değerlendirme ID'si
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Oy veren kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Oy veren kullanıcı adı
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Oy türü
    /// </summary>
    public string VoteType { get; set; } = string.Empty;

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
