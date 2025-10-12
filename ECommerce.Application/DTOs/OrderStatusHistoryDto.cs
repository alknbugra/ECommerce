namespace ECommerce.Application.DTOs;

/// <summary>
/// Sipariş durumu geçmişi DTO'su
/// </summary>
public class OrderStatusHistoryDto
{
    /// <summary>
    /// Geçmiş ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Önceki durum
    /// </summary>
    public string? PreviousStatus { get; set; }

    /// <summary>
    /// Yeni durum
    /// </summary>
    public string NewStatus { get; set; } = string.Empty;

    /// <summary>
    /// Durum değişiklik tarihi
    /// </summary>
    public DateTime StatusChangeDate { get; set; }

    /// <summary>
    /// Durum değişiklik notu
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Durumu değiştiren kullanıcı ID'si
    /// </summary>
    public Guid? ChangedByUserId { get; set; }
}
