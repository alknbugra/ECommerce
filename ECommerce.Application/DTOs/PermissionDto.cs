namespace ECommerce.Application.DTOs;

/// <summary>
/// Yetki (Permission) DTO'su
/// </summary>
public class PermissionDto
{
    /// <summary>
    /// Yetki ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Yetki adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Yetki açıklaması
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Yetki kategorisi
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Yetki eylemi
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Yetki kaynağı
    /// </summary>
    public string Resource { get; set; } = string.Empty;

    /// <summary>
    /// Yetki aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
