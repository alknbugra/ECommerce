using AutoMapper;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Interfaces;
using System.Linq.Expressions;

namespace ECommerce.Application.Features.Products.Queries.GetProducts;

/// <summary>
/// Ürünleri getirme sorgu handler'ı
/// </summary>
public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> HandleAsync(GetProductsQuery request, CancellationToken cancellationToken = default)
    {
        // Filtreleme koşulları
        var filters = new List<Expression<Func<Domain.Entities.Product, bool>>>();

        if (request.CategoryId.HasValue)
        {
            filters.Add(p => p.CategoryId == request.CategoryId.Value);
        }

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            filters.Add(p => p.Name.ToLower().Contains(searchTerm) || 
                           (p.Description != null && p.Description.ToLower().Contains(searchTerm)) ||
                           p.Sku.ToLower().Contains(searchTerm));
        }

        if (request.IsActive.HasValue)
        {
            filters.Add(p => p.IsActive == request.IsActive.Value);
        }

        if (request.InStock.HasValue && request.InStock.Value)
        {
            filters.Add(p => p.StockQuantity > 0);
        }

        // Tüm ürünleri getir (şimdilik basit implementasyon)
        var products = await _unitOfWork.Products.GetAllAsync();

        // Filtreleri uygula
        var filteredProducts = products.AsQueryable();
        
        foreach (var filter in filters)
        {
            filteredProducts = filteredProducts.Where(filter);
        }

        // Sıralama
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            switch (request.SortBy.ToLower())
            {
                case "name":
                    filteredProducts = request.SortDirection?.ToLower() == "desc" 
                        ? filteredProducts.OrderByDescending(p => p.Name)
                        : filteredProducts.OrderBy(p => p.Name);
                    break;
                case "price":
                    filteredProducts = request.SortDirection?.ToLower() == "desc" 
                        ? filteredProducts.OrderByDescending(p => p.Price)
                        : filteredProducts.OrderBy(p => p.Price);
                    break;
                case "createdat":
                    filteredProducts = request.SortDirection?.ToLower() == "desc" 
                        ? filteredProducts.OrderByDescending(p => p.CreatedAt)
                        : filteredProducts.OrderBy(p => p.CreatedAt);
                    break;
                default:
                    filteredProducts = filteredProducts.OrderBy(p => p.Name);
                    break;
            }
        }
        else
        {
            filteredProducts = filteredProducts.OrderBy(p => p.Name);
        }

        // Sayfalama
        var pagedProducts = filteredProducts
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // DTO'ya dönüştür
        var productDtos = _mapper.Map<List<ProductDto>>(pagedProducts);

        // Kategori adlarını ve resimleri doldur
        foreach (var productDto in productDtos)
        {
            var product = pagedProducts.First(p => p.Id == productDto.Id);
            
            if (product.Category != null)
            {
                productDto.CategoryName = product.Category.Name;
            }

            var images = await _unitOfWork.ProductImages.FindAsync(img => img.ProductId == product.Id);
            productDto.Images = _mapper.Map<List<ProductImageDto>>(images);
        }

        return productDtos;
    }
}
