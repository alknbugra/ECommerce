namespace ECommerce.Application.DTOs;

/// <summary>
/// Sepet DTO'su
/// </summary>
public class CartDto
{
    /// <summary>
    /// Sepet ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Session ID
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Son güncelleme tarihi
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Toplam ürün sayısı
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Toplam tutar
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Sepet boş mu?
    /// </summary>
    public bool IsEmpty { get; set; }

    /// <summary>
    /// Sepet ürünleri
    /// </summary>
    public List<CartItemDto> CartItems { get; set; } = new();
}
