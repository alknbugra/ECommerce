using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Alışveriş sepeti entity'si
/// </summary>
public class Cart : BaseEntity
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Session ID (misafir kullanıcılar için)
    /// </summary>
    [MaxLength(100)]
    public string? SessionId { get; set; }

    /// <summary>
    /// Sepet oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Son güncelleme tarihi
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Sepet aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Kullanıcı
    /// </summary>
    public virtual User? User { get; set; }

    /// <summary>
    /// Sepet ürünleri
    /// </summary>
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    /// <summary>
    /// Toplam ürün sayısı
    /// </summary>
    public int TotalItems => CartItems.Sum(item => item.Quantity);

    /// <summary>
    /// Toplam tutar
    /// </summary>
    public decimal TotalAmount => CartItems.Sum(item => item.TotalPrice);

    /// <summary>
    /// Sepet boş mu?
    /// </summary>
    public bool IsEmpty => !CartItems.Any();
}
