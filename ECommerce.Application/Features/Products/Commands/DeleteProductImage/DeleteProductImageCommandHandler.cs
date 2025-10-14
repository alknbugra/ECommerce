using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Products.Commands.DeleteProductImage;

/// <summary>
/// Ürün resmi silme komut handler'ı
/// </summary>
public class DeleteProductImageCommandHandler : ICommandHandler<DeleteProductImageCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileUploadService _fileUploadService;
    private readonly ILogger<DeleteProductImageCommandHandler> _logger;

    public DeleteProductImageCommandHandler(
        IUnitOfWork unitOfWork,
        IFileUploadService fileUploadService,
        ILogger<DeleteProductImageCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileUploadService = fileUploadService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ürün resmi siliniyor: ImageId={ImageId}", request.ImageId);

            // Ürün resmini bul
            var productImage = await _unitOfWork.ProductImages.GetByIdAsync(request.ImageId);
            if (productImage == null)
            {
                return Result.Failure<bool>(Error.NotFound("ProductImage", request.ImageId.ToString()));
            }

            // Ürünü bul
            var product = await _unitOfWork.Products.GetByIdAsync(productImage.ProductId);
            if (product == null)
            {
                return Result.Failure<bool>(Error.NotFound("Product", productImage.ProductId.ToString()));
            }

            // TODO: Yetki kontrolü eklenebilir (sadece ürün sahibi veya admin silebilir)

            // Ana resim ise ürünün ana resim URL'sini temizle
            if (productImage.IsMainImage)
            {
                product.MainImageUrl = null;
                await _unitOfWork.Products.UpdateAsync(product);
            }

            // Fiziksel dosyayı sil
            var fileDeleted = await _fileUploadService.DeleteFileAsync(productImage.ImageUrl, cancellationToken);
            if (!fileDeleted)
            {
                _logger.LogWarning("Fiziksel dosya silinemedi: {ImageUrl}", productImage.ImageUrl);
            }

            // Veritabanından sil
            await _unitOfWork.ProductImages.DeleteAsync(productImage);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Ürün resmi başarıyla silindi: ImageId={ImageId}, ProductId={ProductId}", 
                request.ImageId, productImage.ProductId);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün resmi silinirken hata oluştu: ImageId={ImageId}", request.ImageId);
            return Result.Failure<bool>(Error.Failure("DeleteProductImage.Failed", $"Ürün resmi silinirken hata oluştu: {ex.Message}"));
        }
    }
}
