using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Categories.Queries.GetCategoryById;

/// <summary>
/// ID'ye göre kategori getirme sorgusu
/// </summary>
public class GetCategoryByIdQuery : IQuery<CategoryDto>
{
    /// <summary>
    /// Kategori ID'si
    /// </summary>
    public Guid Id { get; set; }
}
