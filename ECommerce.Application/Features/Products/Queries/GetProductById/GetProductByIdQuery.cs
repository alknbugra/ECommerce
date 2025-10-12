using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// ID'ye göre ürün getirme sorgusu
/// </summary>
public class GetProductByIdQuery : IQuery<ProductDto?>
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid Id { get; set; }
}
