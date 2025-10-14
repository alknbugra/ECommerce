using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Stok hareketi entity'si
/// </summary>
public class InventoryMovement : BaseEntity
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Stok hareket türü
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string MovementType { get; set; } = string.Empty;

    /// <summary>
    /// Hareket miktarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Hareket öncesi stok
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal PreviousStock { get; set; }

    /// <summary>
    /// Hareket sonrası stok
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal NewStock { get; set; }

    /// <summary>
    /// Hareket nedeni
    /// </summary>
    [MaxLength(200)]
    public string? Reason { get; set; }

    /// <summary>
    /// İlişkili entity ID'si (Order, Purchase, etc.)
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// İlişkili entity türü
    /// </summary>
    [MaxLength(50)]
    public string? RelatedEntityType { get; set; }

    /// <summary>
    /// Hareket tarihi
    /// </summary>
    public DateTime MovementDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Hareketi yapan kullanıcı ID'si
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Hareket notları
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Ürün
    /// </summary>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Kullanıcı
    /// </summary>
    public virtual User? User { get; set; }
}
