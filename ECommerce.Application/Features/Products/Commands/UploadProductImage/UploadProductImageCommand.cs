using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Features.Products.Commands.UploadProductImage;

/// <summary>
/// Ürün resmi yükleme komutu
/// </summary>
public class UploadProductImageCommand : ICommand<ProductImageDto>
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Yüklenecek resim dosyası
    /// </summary>
    public IFormFile ImageFile { get; set; } = null!;

    /// <summary>
    /// Resim açıklaması
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Ana resim mi?
    /// </summary>
    public bool IsMainImage { get; set; } = false;

    /// <summary>
    /// Sıralama düzeni
    /// </summary>
    public int SortOrder { get; set; } = 0;
}
