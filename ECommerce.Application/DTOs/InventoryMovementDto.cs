namespace ECommerce.Application.DTOs;

/// <summary>
/// Stok hareketi DTO'su
/// </summary>
public class InventoryMovementDto
{
    /// <summary>
    /// Hareket ID'si
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
    /// Stok hareket türü
    /// </summary>
    public string MovementType { get; set; } = string.Empty;

    /// <summary>
    /// Hareket miktarı
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Hareket öncesi stok
    /// </summary>
    public decimal PreviousStock { get; set; }

    /// <summary>
    /// Hareket sonrası stok
    /// </summary>
    public decimal NewStock { get; set; }

    /// <summary>
    /// Hareket nedeni
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// İlişkili entity ID'si
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// İlişkili entity türü
    /// </summary>
    public string? RelatedEntityType { get; set; }

    /// <summary>
    /// Hareket tarihi
    /// </summary>
    public DateTime MovementDate { get; set; }

    /// <summary>
    /// Hareketi yapan kullanıcı ID'si
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Hareketi yapan kullanıcı adı
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Hareket notları
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
