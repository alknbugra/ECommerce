using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Features.Products.Commands.UploadProductImage;

/// <summary>
/// UploadProductImage komutu için FluentValidation kuralları
/// </summary>
public class UploadProductImageCommandValidator : AbstractValidator<UploadProductImageCommand>
{
    public UploadProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Ürün ID'si gereklidir.");

        RuleFor(x => x.ImageFile)
            .NotNull()
            .WithMessage("Resim dosyası gereklidir.")
            .Must(BeValidImageFile)
            .WithMessage("Geçerli bir resim dosyası seçiniz (JPEG, JPG, PNG, GIF, WebP).")
            .Must(HaveValidFileSize)
            .WithMessage("Resim dosyası boyutu en fazla 5MB olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(200)
            .WithMessage("Resim açıklaması en fazla 200 karakter olabilir.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Sıralama düzeni 0 veya pozitif bir sayı olmalıdır.");
    }

    private static bool BeValidImageFile(IFormFile file)
    {
        if (file == null)
            return false;

        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
        return allowedTypes.Contains(file.ContentType.ToLower());
    }

    private static bool HaveValidFileSize(IFormFile file)
    {
        if (file == null)
            return false;

        const long maxSize = 5 * 1024 * 1024; // 5MB
        return file.Length <= maxSize;
    }
}
