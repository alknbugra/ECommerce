using AutoMapper;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// ID'ye göre ürün getirme sorgu handler'ı
/// </summary>
public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto?> HandleAsync(GetProductByIdQuery request, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        
        if (product == null)
        {
            return null;
        }

        var productDto = _mapper.Map<ProductDto>(product);
        
        // Kategori adını al
        if (product.Category != null)
        {
            productDto.CategoryName = product.Category.Name;
        }

        // Ürün resimlerini al
        var images = await _unitOfWork.ProductImages.FindAsync(img => img.ProductId == product.Id);
        productDto.Images = _mapper.Map<List<ProductImageDto>>(images);

        return productDto;
    }
}
