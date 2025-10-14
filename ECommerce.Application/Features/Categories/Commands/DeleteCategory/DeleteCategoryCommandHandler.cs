using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Categories.Commands.DeleteCategory;

/// <summary>
/// Kategori silme komut handler'ı
/// </summary>
public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteCategoryCommandHandler> _logger;

    public DeleteCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kategori siliniyor: {Id}", request.Id);

            // Kategoriyi bul
            var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
            if (category == null)
            {
                _logger.LogWarning("Kategori silinemedi - Kategori bulunamadı: {Id}", request.Id);
                return Result.Failure<bool>(Error.NotFound("Category.NotFound", "Kategori bulunamadı."));
            }

            // Alt kategoriler var mı kontrol et
            var hasSubCategories = await _unitOfWork.Categories.AnyAsync(c => c.ParentCategoryId == request.Id);
            if (hasSubCategories)
            {
                _logger.LogWarning("Kategori silinemedi - Alt kategoriler mevcut: {Id}", request.Id);
                return Result.Failure<bool>(Error.Problem("Category.HasSubCategories", "Bu kategorinin alt kategorileri bulunmaktadır. Önce alt kategorileri siliniz."));
            }

            // Kategoriye ait ürünler var mı kontrol et
            var hasProducts = await _unitOfWork.Products.AnyAsync(p => p.CategoryId == request.Id);
            if (hasProducts)
            {
                _logger.LogWarning("Kategori silinemedi - Kategoriye ait ürünler mevcut: {Id}", request.Id);
                return Result.Failure<bool>(Error.Problem("Category.HasProducts", "Bu kategoride ürünler bulunmaktadır. Önce ürünleri siliniz veya başka kategoriye taşıyınız."));
            }

            // Kategoriyi sil (soft delete)
            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Kategori başarıyla silindi: {Id}", request.Id);

            return Result.Success<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategori silme sırasında hata oluştu: {Id}", request.Id);
            return Result.Failure<bool>(Error.Problem("Category.DeleteError", "Kategori silme sırasında bir hata oluştu."));
        }
    }
}
