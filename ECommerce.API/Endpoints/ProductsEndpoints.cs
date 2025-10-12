using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Products.Commands.CreateProduct;
using ECommerce.Application.Features.Products.Queries.GetProductById;
using ECommerce.Application.Features.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Ürün endpoint'leri
/// </summary>
public static class ProductsEndpoints
{
    /// <summary>
    /// Ürün endpoint'lerini kaydet
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapProductsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products")
            .WithOpenApi();

        // Tüm ürünleri getir
        group.MapGet("/", GetProducts)
            .WithName("GetProducts")
            .WithSummary("Tüm ürünleri getir")
            .WithDescription("Kategori, arama terimi ve diğer filtrelerle ürünleri getirir")
            .Produces<List<ProductDto>>(200)
            .Produces(400)
            .Produces(500);

        // ID'ye göre ürün getir
        group.MapGet("/{id:guid}", GetProductById)
            .WithName("GetProductById")
            .WithSummary("ID'ye göre ürün getir")
            .WithDescription("Belirtilen ID'ye sahip ürünü getirir")
            .Produces<ProductDto>(200)
            .Produces(404)
            .Produces(500);

        // Yeni ürün oluştur
        group.MapPost("/", CreateProduct)
            .WithName("CreateProduct")
            .WithSummary("Yeni ürün oluştur")
            .WithDescription("Yeni bir ürün oluşturur")
            .Produces<ProductDto>(201)
            .Produces(400)
            .Produces(500);
    }

    /// <summary>
    /// Tüm ürünleri getir
    /// </summary>
    private static async Task<IResult> GetProducts(
        [AsParameters] GetProductsRequest request,
        [FromServices] IQueryHandler<GetProductsQuery, List<ProductDto>> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProductsQuery
        {
            CategoryId = request.CategoryId,
            SearchTerm = request.SearchTerm,
            IsActive = request.IsActive,
            InStock = request.InStock,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var products = await handler.HandleAsync(query, cancellationToken);
        return Results.Ok(products);
    }

    /// <summary>
    /// ID'ye göre ürün getir
    /// </summary>
    private static async Task<IResult> GetProductById(
        Guid id,
        [FromServices] IQueryHandler<GetProductByIdQuery, ProductDto?> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProductByIdQuery { Id = id };
        var product = await handler.HandleAsync(query, cancellationToken);

        if (product == null)
        {
            return Results.NotFound($"ID'si {id} olan ürün bulunamadı.");
        }

        return Results.Ok(product);
    }

    /// <summary>
    /// Yeni ürün oluştur
    /// </summary>
    private static async Task<IResult> CreateProduct(
        [FromBody] CreateProductCommand command,
        [FromServices] ICommandHandler<CreateProductCommand, ProductDto> handler,
        CancellationToken cancellationToken = default)
    {
        var product = await handler.HandleAsync(command, cancellationToken);
        return Results.CreatedAtRoute("GetProductById", new { id = product.Id }, product);
    }

    /// <summary>
    /// GetProducts request modeli
    /// </summary>
    public record GetProductsRequest(
        Guid? CategoryId = null,
        string? SearchTerm = null,
        bool? IsActive = null,
        bool? InStock = null,
        int PageNumber = 1,
        int PageSize = 10,
        string? SortBy = null,
        string? SortDirection = "asc");
}
