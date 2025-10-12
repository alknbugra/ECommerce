using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Kategori oluşturma komutu
/// </summary>
public class CreateCategoryCommand : ICommand<CategoryDto>
{
    /// <summary>
    /// Kategori adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategori açıklaması
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Kategori resmi URL'si
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Üst kategori ID'si
    /// </summary>
    public Guid? ParentCategoryId { get; set; }

    /// <summary>
    /// Sıralama düzeni
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// Kategori aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;
}
