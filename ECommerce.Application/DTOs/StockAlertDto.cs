namespace ECommerce.Application.DTOs;

/// <summary>
/// Stok uyarısı DTO'su
/// </summary>
public class StockAlertDto
{
    /// <summary>
    /// Uyarı ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Ürün adı
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Ürün kodu
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// Uyarı türü
    /// </summary>
    public string AlertType { get; set; } = string.Empty;

    /// <summary>
    /// Mevcut stok miktarı
    /// </summary>
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// Uyarı seviyesi
    /// </summary>
    public decimal AlertLevel { get; set; }

    /// <summary>
    /// Uyarı mesajı
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Uyarı durumu
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Uyarı tarihi
    /// </summary>
    public DateTime AlertDate { get; set; }

    /// <summary>
    /// Uyarı okundu mu?
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Okunma tarihi
    /// </summary>
    public DateTime? ReadDate { get; set; }

    /// <summary>
    /// Uyarıyı okuyan kullanıcı ID'si
    /// </summary>
    public Guid? ReadByUserId { get; set; }

    /// <summary>
    /// Uyarıyı okuyan kullanıcı adı
    /// </summary>
    public string? ReadByUserName { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
