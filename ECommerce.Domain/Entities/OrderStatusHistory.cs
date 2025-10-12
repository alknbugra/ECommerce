using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Sipariş durumu geçmişi entity'si
/// </summary>
public class OrderStatusHistory : BaseEntity
{
    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Önceki durum
    /// </summary>
    [MaxLength(20)]
    public string? PreviousStatus { get; set; }

    /// <summary>
    /// Yeni durum
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string NewStatus { get; set; } = string.Empty;

    /// <summary>
    /// Durum değişiklik tarihi
    /// </summary>
    public DateTime StatusChangeDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Durum değişiklik notu
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Durumu değiştiren kullanıcı ID'si
    /// </summary>
    public Guid? ChangedByUserId { get; set; }

    /// <summary>
    /// Sipariş
    /// </summary>
    public virtual Order Order { get; set; } = null!;
}
