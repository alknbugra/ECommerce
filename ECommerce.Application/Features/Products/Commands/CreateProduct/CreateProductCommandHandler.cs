using AutoMapper;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Ürün oluşturma komut handler'ı
/// </summary>
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto> HandleAsync(CreateProductCommand request, CancellationToken cancellationToken = default)
    {
        // Kategori var mı kontrol et
        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            throw new Common.Exceptions.NotFoundException("Category", request.CategoryId);
        }

        // SKU benzersiz mi kontrol et
        var existingProduct = await _unitOfWork.Products.FirstOrDefaultAsync(p => p.Sku == request.Sku);
        if (existingProduct != null)
        {
            throw new Common.Exceptions.BadRequestException($"SKU '{request.Sku}' zaten kullanılıyor.");
        }

        // Ürün oluştur
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            ShortDescription = request.ShortDescription,
            Sku = request.Sku,
            Price = request.Price,
            DiscountedPrice = request.DiscountedPrice,
            StockQuantity = request.StockQuantity,
            MinStockLevel = request.MinStockLevel,
            Weight = request.Weight,
            Length = request.Length,
            Width = request.Width,
            Height = request.Height,
            MainImageUrl = request.MainImageUrl,
            CategoryId = request.CategoryId,
            IsActive = request.IsActive
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        // DTO'ya dönüştür
        var productDto = _mapper.Map<ProductDto>(product);
        productDto.CategoryName = category.Name;

        return productDto;
    }
}
