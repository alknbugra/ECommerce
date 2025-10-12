using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Application.Features.Categories.Commands.DeleteCategory;

/// <summary>
/// Kategori silme komutu
/// </summary>
public class DeleteCategoryCommand : ICommand<bool>
{
    /// <summary>
    /// Kategori ID'si
    /// </summary>
    public Guid Id { get; set; }
}
