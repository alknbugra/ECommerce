using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Kategori oluşturma komut handler'ı
/// </summary>
public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Yeni kategori oluşturuluyor: {Name}", request.Name);

            // Kategori adı benzersizlik kontrolü
            var existingCategory = await _unitOfWork.Categories.FirstOrDefaultAsync(c => c.Name == request.Name);
            if (existingCategory != null)
            {
                _logger.LogWarning("Kategori oluşturulamadı - Aynı isimde kategori mevcut: {Name}", request.Name);
                return Result.Failure<CategoryDto>(Error.Problem("Category.NameAlreadyExists", "Bu isimde bir kategori zaten mevcut."));
            }

            // Üst kategori kontrolü
            if (request.ParentCategoryId.HasValue)
            {
                var parentCategory = await _unitOfWork.Categories.GetByIdAsync(request.ParentCategoryId.Value);
                if (parentCategory == null)
                {
                    _logger.LogWarning("Kategori oluşturulamadı - Üst kategori bulunamadı: {ParentCategoryId}", request.ParentCategoryId);
                    return Result.Failure<CategoryDto>(Error.NotFound("Category.ParentNotFound", "Belirtilen üst kategori bulunamadı."));
                }
            }

            // Yeni kategori oluştur
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                ParentCategoryId = request.ParentCategoryId,
                SortOrder = request.SortOrder,
                IsActive = request.IsActive
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Kategori başarıyla oluşturuldu: {Name} (ID: {Id})", request.Name, category.Id);

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
            _logger.LogError(ex, "Kategori oluşturma sırasında hata oluştu: {Name}", request.Name);
            return Result.Failure<CategoryDto>(Error.Problem("Category.CreateError", "Kategori oluşturma sırasında bir hata oluştu."));
        }
    }
}
