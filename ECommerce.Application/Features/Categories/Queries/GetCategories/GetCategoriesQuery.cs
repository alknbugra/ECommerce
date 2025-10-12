using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Categories.Queries.GetCategories;

/// <summary>
/// Kategorileri getirme sorgusu
/// </summary>
public class GetCategoriesQuery : IQuery<List<CategoryDto>>
{
    /// <summary>
    /// Üst kategori ID'si (null ise ana kategoriler)
    /// </summary>
    public Guid? ParentCategoryId { get; set; }

    /// <summary>
    /// Sadece aktif kategoriler
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Arama terimi
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Sayfa numarası
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Sıralama alanı
    /// </summary>
    public string? SortBy { get; set; } = "SortOrder";

    /// <summary>
    /// Sıralama yönü
    /// </summary>
    public string SortDirection { get; set; } = "asc";
}
