using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Categories.Queries.GetCategoryById;

/// <summary>
/// ID'ye göre kategori getirme sorgu handler'ı
/// </summary>
public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCategoryByIdQueryHandler> _logger;

    public GetCategoryByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetCategoryByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CategoryDto?> HandleAsync(GetCategoryByIdQuery request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Kategori getiriliyor: {Id}", request.Id);

        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category == null)
        {
            _logger.LogWarning("Kategori bulunamadı: {Id}", request.Id);
            return null;
        }

        // Üst kategori adını al
        string? parentCategoryName = null;
        if (category.ParentCategoryId.HasValue)
        {
            var parentCategory = await _unitOfWork.Categories.GetByIdAsync(category.ParentCategoryId.Value);
            parentCategoryName = parentCategory?.Name;
        }

        // Alt kategori sayısını al
        var subCategoryCount = await _unitOfWork.Categories.CountAsync(c => c.ParentCategoryId == request.Id);

        // Ürün sayısını al
        var productCount = await _unitOfWork.Products.CountAsync(p => p.CategoryId == request.Id);

        _logger.LogInformation("Kategori başarıyla getirildi: {Name} (ID: {Id})", category.Name, request.Id);

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            ParentCategoryId = category.ParentCategoryId,
            ParentCategoryName = parentCategoryName,
            SortOrder = category.SortOrder,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            SubCategoryCount = subCategoryCount,
            ProductCount = productCount
        };
    }
}
