using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Products.Commands.UploadProductImage;

/// <summary>
/// Ürün resmi yükleme komut handler'ı
/// </summary>
public class UploadProductImageCommandHandler : ICommandHandler<UploadProductImageCommand, ProductImageDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileUploadService _fileUploadService;
    private readonly ILogger<UploadProductImageCommandHandler> _logger;

    public UploadProductImageCommandHandler(
        IUnitOfWork unitOfWork,
        IFileUploadService fileUploadService,
        ILogger<UploadProductImageCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileUploadService = fileUploadService;
        _logger = logger;
    }

    public async Task<Result<ProductImageDto>> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ürün resmi yükleniyor: ProductId={ProductId}, FileName={FileName}", 
                request.ProductId, request.ImageFile.FileName);

            // Ürünü bul
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result.Failure<ProductImageDto>(Error.NotFound("Product", request.ProductId.ToString()));
            }

            if (!product.IsActive)
            {
                return Result.Failure<ProductImageDto>(Error.Validation("Product", "Ürün aktif değil."));
            }

            // Dosyayı yükle
            var uploadResult = await _fileUploadService.UploadFileAsync(request.ImageFile, "products", cancellationToken);
            if (!uploadResult.IsSuccess)
            {
                return Result.Failure<ProductImageDto>(Error.Failure("FileUpload.Failed", $"Resim yükleme hatası: {uploadResult.ErrorMessage}"));
            }

            // Ana resim ise diğer resimlerin ana resim durumunu kaldır
            if (request.IsMainImage)
            {
                var existingImages = await _unitOfWork.ProductImages.FindAsync(pi => pi.ProductId == request.ProductId);
                foreach (var existingImage in existingImages)
                {
                    existingImage.IsMainImage = false;
                    await _unitOfWork.ProductImages.UpdateAsync(existingImage);
                }

                // Ürünün ana resim URL'sini güncelle
                product.MainImageUrl = uploadResult.FilePath;
                await _unitOfWork.Products.UpdateAsync(product);
            }

            // Ürün resmi entity'sini oluştur
            var productImage = new ProductImage
            {
                ProductId = request.ProductId,
                ImageUrl = uploadResult.FilePath,
                Description = request.Description,
                IsMainImage = request.IsMainImage,
                SortOrder = request.SortOrder,
                FileName = uploadResult.FileName,
                FileSize = uploadResult.FileSize,
                ContentType = uploadResult.ContentType
            };

            await _unitOfWork.ProductImages.AddAsync(productImage);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Ürün resmi başarıyla yüklendi: ProductId={ProductId}, ImageId={ImageId}", 
                request.ProductId, productImage.Id);

            var productImageDto = new ProductImageDto
            {
                Id = productImage.Id,
                ProductId = productImage.ProductId,
                ImageUrl = productImage.ImageUrl,
                Description = productImage.Description,
                IsMainImage = productImage.IsMainImage,
                SortOrder = productImage.SortOrder,
                FileName = productImage.FileName,
                FileSize = productImage.FileSize,
                ContentType = productImage.ContentType,
                CreatedAt = productImage.CreatedAt
            };

            return Result.Success(productImageDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün resmi yüklenirken hata oluştu: ProductId={ProductId}", request.ProductId);
            return Result.Failure<ProductImageDto>(Error.Failure("UploadProductImage.Failed", $"Ürün resmi yüklenirken hata oluştu: {ex.Message}"));
        }
    }
}
