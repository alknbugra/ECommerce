using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace ECommerce.Application.Features.ProductReviews.Commands.CreateProductReview;

/// <summary>
/// Ürün değerlendirmesi oluşturma komutu
/// </summary>
public class CreateProductReviewCommand : ICommand<ProductReviewDto>
{
    /// <summary>
    /// Değerlendirme oluşturma DTO'su
    /// </summary>
    public CreateProductReviewDto CreateReviewDto { get; set; } = null!;

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Kullanıcı IP adresi
    /// </summary>
    public string? UserIpAddress { get; set; }

    /// <summary>
    /// Kullanıcı agent bilgisi
    /// </summary>
    public string? UserAgent { get; set; }
}

/// <summary>
/// Ürün değerlendirmesi oluşturma komut işleyicisi
/// </summary>
public class CreateProductReviewCommandHandler : ICommandHandler<CreateProductReviewCommand, ProductReviewDto>
{
    private readonly IProductReviewService _productReviewService;
    private readonly ILogger<CreateProductReviewCommandHandler> _logger;

    public CreateProductReviewCommandHandler(
        IProductReviewService productReviewService,
        ILogger<CreateProductReviewCommandHandler> logger)
    {
        _productReviewService = productReviewService;
        _logger = logger;
    }

    public async Task<Result<ProductReviewDto>> Handle(CreateProductReviewCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ürün değerlendirmesi oluşturma komutu işleniyor. ProductId: {ProductId}, UserId: {UserId}", 
                request.CreateReviewDto.ProductId, request.UserId);

            var result = await _productReviewService.CreateReviewAsync(
                request.CreateReviewDto,
                request.UserId,
                request.UserIpAddress,
                request.UserAgent,
                cancellationToken);

            if (result == null)
            {
                _logger.LogWarning("Ürün değerlendirmesi oluşturulamadı. ProductId: {ProductId}, UserId: {UserId}", 
                    request.CreateReviewDto.ProductId, request.UserId);
                return Result.Failure<ProductReviewDto>(Error.Validation("ProductReview", "Ürün değerlendirmesi oluşturulamadı. Kullanıcı daha önce bu ürünü değerlendirmiş olabilir veya ürünü satın almamış olabilir."));
            }

            _logger.LogInformation("Ürün değerlendirmesi başarıyla oluşturuldu. Id: {Id}, ProductId: {ProductId}", 
                result.Id, request.CreateReviewDto.ProductId);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmesi oluşturma sırasında hata oluştu. ProductId: {ProductId}", 
                request.CreateReviewDto.ProductId);
            return Result.Failure<ProductReviewDto>(Error.Failure("CreateProductReview.Failed", $"Ürün değerlendirmesi oluşturulurken hata oluştu: {ex.Message}"));
        }
    }
}

/// <summary>
/// Ürün değerlendirmesi oluşturma komut validator'ı
/// </summary>
public class CreateProductReviewCommandValidator : AbstractValidator<CreateProductReviewCommand>
{
    public CreateProductReviewCommandValidator()
    {
        RuleFor(x => x.CreateReviewDto)
            .NotNull()
            .WithMessage("Değerlendirme bilgileri boş olamaz");

        RuleFor(x => x.CreateReviewDto.ProductId)
            .NotEmpty()
            .WithMessage("Ürün ID'si boş olamaz");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID'si boş olamaz");

        RuleFor(x => x.CreateReviewDto.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Puan 1-5 arasında olmalıdır");

        RuleFor(x => x.CreateReviewDto.Title)
            .MaximumLength(200)
            .WithMessage("Başlık en fazla 200 karakter olabilir");

        RuleFor(x => x.CreateReviewDto.Content)
            .MaximumLength(2000)
            .WithMessage("İçerik en fazla 2000 karakter olabilir");

        RuleFor(x => x.CreateReviewDto.ReviewType)
            .Must(BeValidReviewType)
            .WithMessage("Geçersiz değerlendirme türü");
    }

    private bool BeValidReviewType(string reviewType)
    {
        var validTypes = new[] { "Verified", "Unverified", "Guest" };
        return validTypes.Contains(reviewType);
    }
}
