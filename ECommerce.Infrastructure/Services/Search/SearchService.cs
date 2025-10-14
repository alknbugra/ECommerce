using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Text.Json;

namespace ECommerce.Infrastructure.Services.Search;

/// <summary>
/// Arama servisi implementasyonu
/// </summary>
public class SearchService : ISearchService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SearchService> _logger;

    public SearchService(IUnitOfWork unitOfWork, ILogger<SearchService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<SearchResultDto> SearchProductsAsync(AdvancedSearchDto searchDto, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            // Filtreleme koşulları oluştur
            var filters = BuildSearchFilters(searchDto);

            // Ürünleri getir
            var products = await _unitOfWork.Products.GetAllAsync();
            var query = products.AsQueryable();

            // Filtreleri uygula
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }

            // Sıralama uygula
            query = ApplySorting(query, searchDto.SortBy, searchDto.SortDirection);

            // Toplam sayıyı al
            var totalCount = query.Count();

            // Sayfalama uygula
            var pagedProducts = query
                .Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToList();

            // DTO'ya dönüştür
            var productDtos = new List<ProductDto>();
            foreach (var product in pagedProducts)
            {
                var productDto = MapToProductDto(product);
                productDtos.Add(productDto);
            }

            // Filtreleme seçeneklerini oluştur
            var filtersDto = await BuildSearchFiltersDto(searchDto, products);

            stopwatch.Stop();

            return new SearchResultDto
            {
                Products = productDtos,
                TotalCount = totalCount,
                PageNumber = searchDto.PageNumber,
                PageSize = searchDto.PageSize,
                SearchTimeMs = stopwatch.ElapsedMilliseconds,
                Filters = filtersDto
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Arama sırasında hata oluştu");
            throw;
        }
    }

    public async Task<List<SearchSuggestionDto>> GetSearchSuggestionsAsync(string query, int limit = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var suggestions = new List<SearchSuggestionDto>();

            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            {
                return suggestions;
            }

            var searchTerm = query.ToLower();

            // Ürün adlarından öneriler
            var products = await _unitOfWork.Products.GetAllAsync();
            var productSuggestions = products
                .Where(p => p.Name.ToLower().Contains(searchTerm) && p.IsActive)
                .Take(limit / 2)
                .Select(p => new SearchSuggestionDto
                {
                    Text = p.Name,
                    Type = SuggestionType.Product,
                    RelatedId = p.Id,
                    Score = CalculateRelevanceScore(p.Name, searchTerm)
                })
                .OrderByDescending(s => s.Score)
                .ToList();

            suggestions.AddRange(productSuggestions);

            // Kategori adlarından öneriler
            var categories = await _unitOfWork.Categories.GetAllAsync();
            var categorySuggestions = categories
                .Where(c => c.Name.ToLower().Contains(searchTerm) && c.IsActive)
                .Take(limit / 4)
                .Select(c => new SearchSuggestionDto
                {
                    Text = c.Name,
                    Type = SuggestionType.Category,
                    RelatedId = c.Id,
                    Score = CalculateRelevanceScore(c.Name, searchTerm)
                })
                .OrderByDescending(s => s.Score)
                .ToList();

            suggestions.AddRange(categorySuggestions);

            // Marka adlarından öneriler
            var brands = await _unitOfWork.ProductBrands.GetAllAsync();
            var brandSuggestions = brands
                .Where(b => b.Name.ToLower().Contains(searchTerm) && b.IsActive)
                .Take(limit / 4)
                .Select(b => new SearchSuggestionDto
                {
                    Text = b.Name,
                    Type = SuggestionType.Brand,
                    RelatedId = b.Id,
                    Score = CalculateRelevanceScore(b.Name, searchTerm)
                })
                .OrderByDescending(s => s.Score)
                .ToList();

            suggestions.AddRange(brandSuggestions);

            // Popüler aramalardan öneriler
            var popularSearches = await GetPopularSearchesAsync(limit / 4, cancellationToken);
            var popularSuggestions = popularSearches
                .Where(ps => ps.ToLower().Contains(searchTerm))
                .Select(ps => new SearchSuggestionDto
                {
                    Text = ps,
                    Type = SuggestionType.PopularSearch,
                    Score = 50 // Orta skor
                })
                .ToList();

            suggestions.AddRange(popularSuggestions);

            return suggestions
                .OrderByDescending(s => s.Score)
                .Take(limit)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Arama önerileri getirilirken hata oluştu");
            return new List<SearchSuggestionDto>();
        }
    }

    public async Task<List<string>> GetPopularSearchesAsync(int limit = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var searchHistories = await _unitOfWork.SearchHistories.GetAllAsync();
            
            return searchHistories
                .Where(sh => !string.IsNullOrEmpty(sh.SearchTerm))
                .GroupBy(sh => sh.SearchTerm.ToLower())
                .OrderByDescending(g => g.Count())
                .Take(limit)
                .Select(g => g.Key)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Popüler aramalar getirilirken hata oluştu");
            return new List<string>();
        }
    }

    public async Task SaveSearchHistoryAsync(string searchTerm, Guid? userId, string? ipAddress, string? userAgent, int resultCount, CancellationToken cancellationToken = default)
    {
        try
        {
            var searchHistory = new SearchHistory
            {
                SearchTerm = searchTerm,
                UserId = userId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                ResultCount = resultCount
            };

            await _unitOfWork.SearchHistories.AddAsync(searchHistory);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Arama geçmişi kaydedilirken hata oluştu");
        }
    }

    public async Task SaveProductClickAsync(string searchTerm, Guid productId, Guid? userId, string? ipAddress, CancellationToken cancellationToken = default)
    {
        try
        {
            var searchHistory = new SearchHistory
            {
                SearchTerm = searchTerm,
                UserId = userId,
                IpAddress = ipAddress,
                ClickedProductId = productId
            };

            await _unitOfWork.SearchHistories.AddAsync(searchHistory);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün tıklama geçmişi kaydedilirken hata oluştu");
        }
    }

    public async Task RefreshSearchIndexAsync(CancellationToken cancellationToken = default)
    {
        // Bu implementasyon gelecekte Elasticsearch veya benzeri bir arama motoru ile geliştirilebilir
        _logger.LogInformation("Arama indeksi yenileniyor...");
        await Task.CompletedTask;
    }

    #region Private Methods

    private List<Expression<Func<Product, bool>>> BuildSearchFilters(AdvancedSearchDto searchDto)
    {
        var filters = new List<Expression<Func<Product, bool>>>();

        // Arama terimi filtresi
        if (!string.IsNullOrEmpty(searchDto.SearchTerm))
        {
            var searchTerm = searchDto.SearchTerm.ToLower();
            filters.Add(p => p.Name.ToLower().Contains(searchTerm) ||
                           (p.Description != null && p.Description.ToLower().Contains(searchTerm)) ||
                           p.Sku.ToLower().Contains(searchTerm));
        }

        // Kategori filtresi
        if (searchDto.CategoryIds != null && searchDto.CategoryIds.Any())
        {
            filters.Add(p => searchDto.CategoryIds.Contains(p.CategoryId));
        }

        // Marka filtresi
        if (searchDto.BrandIds != null && searchDto.BrandIds.Any())
        {
            filters.Add(p => p.BrandId.HasValue && searchDto.BrandIds.Contains(p.BrandId.Value));
        }

        // Fiyat filtresi
        if (searchDto.MinPrice.HasValue)
        {
            filters.Add(p => p.Price >= searchDto.MinPrice.Value);
        }

        if (searchDto.MaxPrice.HasValue)
        {
            filters.Add(p => p.Price <= searchDto.MaxPrice.Value);
        }

        // Stok filtresi
        if (searchDto.InStock.HasValue && searchDto.InStock.Value)
        {
            filters.Add(p => p.StockQuantity > 0);
        }

        // İndirim filtresi
        if (searchDto.OnSale.HasValue && searchDto.OnSale.Value)
        {
            filters.Add(p => p.DiscountedPrice.HasValue && p.DiscountedPrice < p.Price);
        }

        // Aktif ürün filtresi
        filters.Add(p => p.IsActive);

        return filters;
    }

    private IQueryable<Product> ApplySorting(IQueryable<Product> query, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return sortBy?.ToLower() switch
        {
            "name" => isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "newest" => isDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
            "popularity" => query.OrderByDescending(p => p.OrderItems.Count), // Sipariş sayısına göre
            "rating" => query.OrderByDescending(p => p.Reviews.Average(r => r.Rating)), // Ortalama değerlendirmeye göre
            "discount" => query.OrderByDescending(p => p.DiscountPercentage ?? 0),
            _ => query.OrderBy(p => p.Name)
        };
    }

    private async Task<SearchFiltersDto> BuildSearchFiltersDto(AdvancedSearchDto searchDto, IEnumerable<Product> allProducts)
    {
        var filtersDto = new SearchFiltersDto();

        // Kategori filtreleri
        var categories = await _unitOfWork.Categories.GetAllAsync();
        filtersDto.Categories = categories
            .Where(c => c.IsActive)
            .Select(c => new FilterOptionDto
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Count = allProducts.Count(p => p.CategoryId == c.Id),
                IsSelected = searchDto.CategoryIds?.Contains(c.Id) ?? false
            })
            .Where(f => f.Count > 0)
            .OrderBy(f => f.Name)
            .ToList();

        // Marka filtreleri
        var brands = await _unitOfWork.ProductBrands.GetAllAsync();
        filtersDto.Brands = brands
            .Where(b => b.IsActive)
            .Select(b => new FilterOptionDto
            {
                Id = b.Id.ToString(),
                Name = b.Name,
                Count = allProducts.Count(p => p.BrandId == b.Id),
                IsSelected = searchDto.BrandIds?.Contains(b.Id) ?? false
            })
            .Where(f => f.Count > 0)
            .OrderBy(f => f.Name)
            .ToList();

        // Fiyat aralığı
        if (allProducts.Any())
        {
            var minPrice = allProducts.Min(p => p.Price);
            var maxPrice = allProducts.Max(p => p.Price);
            
            filtersDto.PriceRange = new PriceRangeDto
            {
                Min = minPrice,
                Max = maxPrice,
                SelectedMin = searchDto.MinPrice,
                SelectedMax = searchDto.MaxPrice
            };
        }

        return filtersDto;
    }

    private ProductDto MapToProductDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ShortDescription = product.ShortDescription,
            Sku = product.Sku,
            Price = product.Price,
            DiscountedPrice = product.DiscountedPrice,
            StockQuantity = product.StockQuantity,
            MinStockLevel = product.MinStockLevel,
            Weight = product.Weight,
            Length = product.Length,
            Width = product.Width,
            Height = product.Height,
            MainImageUrl = product.MainImageUrl,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            BrandId = product.BrandId,
            BrandName = product.Brand?.Name,
            IsActive = product.IsActive,
            InStock = product.InStock,
            DiscountPercentage = product.DiscountPercentage,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt ?? product.CreatedAt
        };
    }

    private int CalculateRelevanceScore(string text, string searchTerm)
    {
        var textLower = text.ToLower();
        var searchLower = searchTerm.ToLower();

        // Tam eşleşme
        if (textLower == searchLower)
            return 100;

        // Başlangıçta eşleşme
        if (textLower.StartsWith(searchLower))
            return 90;

        // Kelime başlangıcında eşleşme
        if (textLower.Contains($" {searchLower}"))
            return 80;

        // İçerisinde eşleşme
        if (textLower.Contains(searchLower))
            return 70;

        return 0;
    }

    #endregion
}
