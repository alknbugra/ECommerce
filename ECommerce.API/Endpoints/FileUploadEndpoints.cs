using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Products.Commands.UploadProductImage;
using ECommerce.Application.Features.Products.Commands.DeleteProductImage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Dosya yükleme endpoint'leri
/// </summary>
public static class FileUploadEndpoints
{
    /// <summary>
    /// Dosya yükleme endpoint'lerini kaydet
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapFileUploadEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/upload")
            .WithTags("File Upload")
            .WithOpenApi();

        // Ürün resmi yükle
        group.MapPost("/products/{productId:guid}/images", UploadProductImage)
            .WithName("UploadProductImage")
            .WithSummary("Ürün resmi yükle")
            .WithDescription("Belirtilen ürün için resim yükler")
            .RequireAuthorization()
            .Produces<ProductImageDto>(201)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);

        // Ürün resmi sil
        group.MapDelete("/products/images/{imageId:guid}", DeleteProductImage)
            .WithName("DeleteProductImage")
            .WithSummary("Ürün resmi sil")
            .WithDescription("Belirtilen ürün resmini siler")
            .RequireAuthorization()
            .Produces<bool>(200)
            .Produces(401)
            .Produces(404)
            .Produces(500);

        // Genel dosya yükleme
        group.MapPost("/{folder}", UploadFile)
            .WithName("UploadFile")
            .WithSummary("Dosya yükle")
            .WithDescription("Belirtilen klasöre dosya yükler")
            .RequireAuthorization()
            .Produces<FileUploadResult>(201)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // Birden fazla dosya yükleme
        group.MapPost("/{folder}/multiple", UploadFiles)
            .WithName("UploadFiles")
            .WithSummary("Birden fazla dosya yükle")
            .WithDescription("Belirtilen klasöre birden fazla dosya yükler")
            .RequireAuthorization()
            .Produces<List<FileUploadResult>>(201)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // Dosya silme
        group.MapDelete("/{filePath:required}", DeleteFile)
            .WithName("DeleteFile")
            .WithSummary("Dosya sil")
            .WithDescription("Belirtilen dosyayı siler")
            .RequireAuthorization()
            .Produces<bool>(200)
            .Produces(401)
            .Produces(404)
            .Produces(500);

        // Desteklenen dosya tiplerini getir
        group.MapGet("/supported-types", GetSupportedFileTypes)
            .WithName("GetSupportedFileTypes")
            .WithSummary("Desteklenen dosya tiplerini getir")
            .WithDescription("Yüklenebilecek dosya tiplerini ve maksimum boyutu döndürür")
            .Produces<FileUploadInfo>(200)
            .Produces(500);
    }

    /// <summary>
    /// Ürün resmi yükle
    /// </summary>
    private static async Task<IResult> UploadProductImage(
        Guid productId,
        [FromForm] UploadProductImageRequest request,
        [FromServices] ICommandHandler<UploadProductImageCommand, ProductImageDto> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        if (request.ImageFile == null || request.ImageFile.Length == 0)
        {
            return Results.BadRequest("Resim dosyası gerekli.");
        }

        var command = new UploadProductImageCommand
        {
            ProductId = productId,
            ImageFile = request.ImageFile,
            Description = request.Description,
            IsMainImage = request.IsMainImage,
            SortOrder = request.SortOrder
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.CreatedAtRoute("GetProductById", new { id = productId }, result);
    }

    /// <summary>
    /// Ürün resmi sil
    /// </summary>
    private static async Task<IResult> DeleteProductImage(
        Guid imageId,
        [FromServices] ICommandHandler<DeleteProductImageCommand, bool> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        var command = new DeleteProductImageCommand
        {
            ImageId = imageId,
            UserId = userId.Value
        };

        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Genel dosya yükleme
    /// </summary>
    private static async Task<IResult> UploadFile(
        string folder,
        [FromForm] IFormFile file,
        [FromServices] IFileUploadService fileUploadService,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        if (file == null || file.Length == 0)
        {
            return Results.BadRequest("Dosya gerekli.");
        }

        var result = await fileUploadService.UploadFileAsync(file, folder, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return Results.BadRequest(result.ErrorMessage);
        }

        return Results.Created($"/api/upload/{folder}/{result.FileName}", result);
    }

    /// <summary>
    /// Birden fazla dosya yükleme
    /// </summary>
    private static async Task<IResult> UploadFiles(
        string folder,
        [FromForm] IFormFileCollection files,
        [FromServices] IFileUploadService fileUploadService,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        if (files == null || files.Count == 0)
        {
            return Results.BadRequest("En az bir dosya gerekli.");
        }

        var results = await fileUploadService.UploadFilesAsync(files, folder, cancellationToken);
        return Results.Created($"/api/upload/{folder}", results);
    }

    /// <summary>
    /// Dosya silme
    /// </summary>
    private static async Task<IResult> DeleteFile(
        string filePath,
        [FromServices] IFileUploadService fileUploadService,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        var result = await fileUploadService.DeleteFileAsync(filePath, cancellationToken);
        
        if (!result)
        {
            return Results.NotFound("Dosya bulunamadı veya silinemedi.");
        }

        return Results.Ok(true);
    }

    /// <summary>
    /// Desteklenen dosya tiplerini getir
    /// </summary>
    private static IResult GetSupportedFileTypes(
        [FromServices] IFileUploadService fileUploadService)
    {
        var supportedTypes = fileUploadService.GetSupportedFileTypes();
        var maxFileSize = fileUploadService.GetMaxFileSize();

        var info = new FileUploadInfo(
            supportedTypes.ToList(),
            maxFileSize,
            maxFileSize / (1024 * 1024)
        );

        return Results.Ok(info);
    }

    /// <summary>
    /// HttpContext'ten kullanıcı ID'sini al
    /// </summary>
    private static Guid? GetUserIdFromContext(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        return null;
    }

    /// <summary>
    /// UploadProductImage request modeli
    /// </summary>
    public record UploadProductImageRequest(
        IFormFile ImageFile,
        string? Description = null,
        bool IsMainImage = false,
        int SortOrder = 0);

    /// <summary>
    /// File upload bilgi modeli
    /// </summary>
    public record FileUploadInfo(
        List<string> SupportedFileTypes,
        long MaxFileSize,
        long MaxFileSizeMB);
}
