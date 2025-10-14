using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Categories.Commands.UpdateCategory;

/// <summary>
/// Kategori güncelleme komut handler'ı
/// </summary>
public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    public UpdateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kategori güncelleniyor: {Id}", request.Id);

            // Kategoriyi bul
            var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
            if (category == null)
            {
                _logger.LogWarning("Kategori güncellenemedi - Kategori bulunamadı: {Id}", request.Id);
                return Result.Failure<CategoryDto>(Error.NotFound("Category.NotFound", "Kategori bulunamadı."));
            }

            // Kategori adı benzersizlik kontrolü (kendisi hariç)
            var existingCategory = await _unitOfWork.Categories.FirstOrDefaultAsync(c => c.Name == request.Name && c.Id != request.Id);
            if (existingCategory != null)
            {
                _logger.LogWarning("Kategori güncellenemedi - Aynı isimde kategori mevcut: {Name}", request.Name);
                return Result.Failure<CategoryDto>(Error.Problem("Category.NameAlreadyExists", "Bu isimde bir kategori zaten mevcut."));
            }

            // Üst kategori kontrolü (kendisi olamaz)
            if (request.ParentCategoryId.HasValue)
            {
                if (request.ParentCategoryId.Value == request.Id)
                {
                    return Result.Failure<CategoryDto>(Error.Problem("Category.SelfParent", "Bir kategori kendisinin alt kategorisi olamaz."));
                }

                var parentCategory = await _unitOfWork.Categories.GetByIdAsync(request.ParentCategoryId.Value);
                if (parentCategory == null)
                {
                    _logger.LogWarning("Kategori güncellenemedi - Üst kategori bulunamadı: {ParentCategoryId}", request.ParentCategoryId);
                    return Result.Failure<CategoryDto>(Error.NotFound("Category.ParentNotFound", "Belirtilen üst kategori bulunamadı."));
                }
            }

            // Kategoriyi güncelle
            category.Name = request.Name;
            category.Description = request.Description;
            category.ImageUrl = request.ImageUrl;
            category.ParentCategoryId = request.ParentCategoryId;
            category.SortOrder = request.SortOrder;
            category.IsActive = request.IsActive;
            category.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Kategori başarıyla güncellendi: {Name} (ID: {Id})", request.Name, request.Id);

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = null, // Bu bilgiyi ayrıca yüklemek gerekebilir
                SortOrder = category.SortOrder,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };

            return Result.Success<CategoryDto>(categoryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategori güncelleme sırasında hata oluştu: {Id}", request.Id);
            return Result.Failure<CategoryDto>(Error.Problem("Category.UpdateError", "Kategori güncelleme sırasında bir hata oluştu."));
        }
    }
}
