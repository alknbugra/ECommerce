using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Stok uyarısı entity'si
/// </summary>
public class StockAlert : BaseEntity
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Uyarı türü
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string AlertType { get; set; } = string.Empty;

    /// <summary>
    /// Mevcut stok miktarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// Uyarı seviyesi
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal AlertLevel { get; set; }

    /// <summary>
    /// Uyarı mesajı
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Uyarı durumu
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Active";

    /// <summary>
    /// Uyarı tarihi
    /// </summary>
    public DateTime AlertDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Uyarı okundu mu?
    /// </summary>
    public bool IsRead { get; set; } = false;

    /// <summary>
    /// Okunma tarihi
    /// </summary>
    public DateTime? ReadDate { get; set; }

    /// <summary>
    /// Uyarıyı okuyan kullanıcı ID'si
    /// </summary>
    public Guid? ReadByUserId { get; set; }

    /// <summary>
    /// Ürün
    /// </summary>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Okuyan kullanıcı
    /// </summary>
    public virtual User? ReadByUser { get; set; }
}
