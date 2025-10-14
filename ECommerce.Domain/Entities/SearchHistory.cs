using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Arama geçmişi entity'si
/// </summary>
public class SearchHistory : BaseEntity
{
    /// <summary>
    /// Kullanıcı ID'si (anonim aramalar için null)
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Kullanıcı
    /// </summary>
    public virtual User? User { get; set; }

    /// <summary>
    /// Arama terimi
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string SearchTerm { get; set; } = string.Empty;

    /// <summary>
    /// Arama sonuç sayısı
    /// </summary>
    public int ResultCount { get; set; } = 0;

    /// <summary>
    /// Arama yapılan IP adresi
    /// </summary>
    [MaxLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// User Agent
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Arama yapılan kategori ID'si
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Kategori
    /// </summary>
    public virtual Category? Category { get; set; }

    /// <summary>
    /// Arama filtresi (JSON formatında)
    /// </summary>
    public string? Filters { get; set; }

    /// <summary>
    /// Arama sonucunda tıklanan ürün ID'si
    /// </summary>
    public Guid? ClickedProductId { get; set; }

    /// <summary>
    /// Tıklanan ürün
    /// </summary>
    public virtual Product? ClickedProduct { get; set; }
}
