using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Categories.Commands.CreateCategory;
using ECommerce.Application.Features.Categories.Commands.UpdateCategory;
using ECommerce.Application.Features.Categories.Commands.DeleteCategory;
using ECommerce.Application.Features.Categories.Queries.GetCategoryById;
using ECommerce.Application.Features.Categories.Queries.GetCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Kategori endpoint'leri
/// </summary>
public static class CategoriesEndpoints
{
    /// <summary>
    /// Kategori endpoint'lerini kaydet
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapCategoriesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/categories")
            .WithTags("Categories")
            .WithOpenApi();

        // Tüm kategorileri getir
        group.MapGet("/", GetCategories)
            .WithName("GetCategories")
            .WithSummary("Kategorileri getir")
            .WithDescription("Filtreleme, arama ve sayfalama ile kategorileri getirir")
            .Produces<List<CategoryDto>>(200)
            .Produces(400)
            .Produces(500);

        // ID'ye göre kategori getir
        group.MapGet("/{id:guid}", GetCategoryById)
            .WithName("GetCategoryById")
            .WithSummary("ID'ye göre kategori getir")
            .WithDescription("Belirtilen ID'ye sahip kategoriyi getirir")
            .Produces<CategoryDto>(200)
            .Produces(404)
            .Produces(500);

        // Alt kategorileri getir
        group.MapGet("/{id:guid}/subcategories", GetSubCategories)
            .WithName("GetSubCategories")
            .WithSummary("Alt kategorileri getir")
            .WithDescription("Belirtilen kategorinin alt kategorilerini getirir")
            .Produces<List<CategoryDto>>(200)
            .Produces(404)
            .Produces(500);

        // Yeni kategori oluştur (Admin gerekli)
        group.MapPost("/", CreateCategory)
            .WithName("CreateCategory")
            .WithSummary("Yeni kategori oluştur")
            .WithDescription("Yeni bir kategori oluşturur")
            .RequireAuthorization()
            .Produces<CategoryDto>(201)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // Kategori güncelle (Admin gerekli)
        group.MapPut("/{id:guid}", UpdateCategory)
            .WithName("UpdateCategory")
            .WithSummary("Kategori güncelle")
            .WithDescription("Mevcut bir kategoriyi günceller")
            .RequireAuthorization()
            .Produces<CategoryDto>(200)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);

        // Kategori sil (Admin gerekli)
        group.MapDelete("/{id:guid}", DeleteCategory)
            .WithName("DeleteCategory")
            .WithSummary("Kategori sil")
            .WithDescription("Bir kategoriyi siler")
            .RequireAuthorization()
            .Produces<bool>(200)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);
    }

    /// <summary>
    /// Kategorileri getir
    /// </summary>
    private static async Task<IResult> GetCategories(
        [AsParameters] GetCategoriesRequest request,
        [FromServices] IQueryHandler<GetCategoriesQuery, List<CategoryDto>> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCategoriesQuery
        {
            ParentCategoryId = request.ParentCategoryId,
            IsActive = request.IsActive,
            SearchTerm = request.SearchTerm,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var categories = await handler.Handle(query, cancellationToken);
        return Results.Ok(categories);
    }

    /// <summary>
    /// ID'ye göre kategori getir
    /// </summary>
    private static async Task<IResult> GetCategoryById(
        Guid id,
        [FromServices] IQueryHandler<GetCategoryByIdQuery, CategoryDto?> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCategoryByIdQuery { Id = id };
        var category = await handler.Handle(query, cancellationToken);

        if (category == null)
        {
            return Results.NotFound($"ID'si {id} olan kategori bulunamadı.");
        }

        return Results.Ok(category);
    }

    /// <summary>
    /// Alt kategorileri getir
    /// </summary>
    private static async Task<IResult> GetSubCategories(
        Guid id,
        [FromServices] IQueryHandler<GetCategoriesQuery, List<CategoryDto>> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCategoriesQuery
        {
            ParentCategoryId = id,
            IsActive = true // Sadece aktif alt kategoriler
        };

        var subCategories = await handler.Handle(query, cancellationToken);
        return Results.Ok(subCategories);
    }

    /// <summary>
    /// Yeni kategori oluştur
    /// </summary>
    private static async Task<IResult> CreateCategory(
        [FromBody] CreateCategoryCommand command,
        [FromServices] ICommandHandler<CreateCategoryCommand, CategoryDto> handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);
        
        return Results.CreatedAtRoute("GetCategoryById", new { id = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Kategori güncelle
    /// </summary>
    private static async Task<IResult> UpdateCategory(
        Guid id,
        [FromBody] UpdateCategoryCommand command,
        [FromServices] ICommandHandler<UpdateCategoryCommand, CategoryDto> handler,
        CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var category = await handler.Handle(command, cancellationToken);
        return Results.Ok(category);
    }

    /// <summary>
    /// Kategori sil
    /// </summary>
    private static async Task<IResult> DeleteCategory(
        Guid id,
        [FromServices] ICommandHandler<DeleteCategoryCommand, bool> handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteCategoryCommand { Id = id };
        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// GetCategories request modeli
    /// </summary>
    public record GetCategoriesRequest(
        Guid? ParentCategoryId = null,
        bool? IsActive = null,
        string? SearchTerm = null,
        int PageNumber = 1,
        int PageSize = 10,
        string? SortBy = "SortOrder",
        string SortDirection = "asc");
}
