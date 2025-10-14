using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Categories.Queries.GetCategories;

/// <summary>
/// Kategorileri getirme sorgu handler'ı
/// </summary>
public class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCategoriesQueryHandler> _logger;

    public GetCategoriesQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetCategoriesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kategoriler getiriliyor - ParentCategoryId: {ParentCategoryId}, IsActive: {IsActive}", 
                request.ParentCategoryId, request.IsActive);

        var categories = await _unitOfWork.Categories.GetAllAsync();

        // Filtreleme
        var filteredCategories = categories.AsQueryable();

        if (request.ParentCategoryId.HasValue)
        {
            filteredCategories = filteredCategories.Where(c => c.ParentCategoryId == request.ParentCategoryId.Value);
        }
        else
        {
            // Ana kategoriler (ParentCategoryId null olanlar)
            filteredCategories = filteredCategories.Where(c => c.ParentCategoryId == null);
        }

        if (request.IsActive.HasValue)
        {
            filteredCategories = filteredCategories.Where(c => c.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            filteredCategories = filteredCategories.Where(c => 
                c.Name.ToLower().Contains(searchTerm) || 
                (c.Description != null && c.Description.ToLower().Contains(searchTerm)));
        }

        // Sıralama
        filteredCategories = request.SortBy?.ToLower() switch
        {
            "name" => request.SortDirection.ToLower() == "desc" 
                ? filteredCategories.OrderByDescending(c => c.Name)
                : filteredCategories.OrderBy(c => c.Name),
            "createdat" => request.SortDirection.ToLower() == "desc"
                ? filteredCategories.OrderByDescending(c => c.CreatedAt)
                : filteredCategories.OrderBy(c => c.CreatedAt),
            _ => request.SortDirection.ToLower() == "desc"
                ? filteredCategories.OrderByDescending(c => c.SortOrder).ThenByDescending(c => c.Name)
                : filteredCategories.OrderBy(c => c.SortOrder).ThenBy(c => c.Name)
        };

        // Sayfalama
        var pagedCategories = filteredCategories
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // DTO'ya dönüştür
        var categoryDtos = new List<CategoryDto>();
        foreach (var category in pagedCategories)
        {
            // Üst kategori adını al
            string? parentCategoryName = null;
            if (category.ParentCategoryId.HasValue)
            {
                var parentCategory = categories.FirstOrDefault(c => c.Id == category.ParentCategoryId.Value);
                parentCategoryName = parentCategory?.Name;
            }

            // Alt kategori sayısını al
            var subCategoryCount = categories.Count(c => c.ParentCategoryId == category.Id);

            // Ürün sayısını al (bu performans için ayrı sorgu gerekebilir)
            var productCount = await _unitOfWork.Products.CountAsync(p => p.CategoryId == category.Id);

            categoryDtos.Add(new CategoryDto
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
            });
        }

        _logger.LogInformation("{Count} kategori getirildi", categoryDtos.Count);

        return Result.Success<List<CategoryDto>>(categoryDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategoriler getirme sırasında hata oluştu");
            return Result.Failure<List<CategoryDto>>(Error.Problem("Category.GetCategoriesError", "Kategoriler getirme sırasında bir hata oluştu."));
        }
    }
}
