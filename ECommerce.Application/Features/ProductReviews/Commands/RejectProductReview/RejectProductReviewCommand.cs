using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace ECommerce.Application.Features.ProductReviews.Commands.RejectProductReview;

/// <summary>
/// Ürün değerlendirmesi reddetme komutu
/// </summary>
public class RejectProductReviewCommand : ICommand<bool>
{
    /// <summary>
    /// Değerlendirme ID'si
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Red sebebi
    /// </summary>
    public string RejectionReason { get; set; } = string.Empty;

    /// <summary>
    /// Reddeden kullanıcı ID'si
    /// </summary>
    public Guid RejectedByUserId { get; set; }
}

/// <summary>
/// Ürün değerlendirmesi reddetme komut işleyicisi
/// </summary>
public class RejectProductReviewCommandHandler : ICommandHandler<RejectProductReviewCommand, bool>
{
    private readonly IProductReviewService _productReviewService;
    private readonly ILogger<RejectProductReviewCommandHandler> _logger;

    public RejectProductReviewCommandHandler(
        IProductReviewService productReviewService,
        ILogger<RejectProductReviewCommandHandler> logger)
    {
        _productReviewService = productReviewService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(RejectProductReviewCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ürün değerlendirmesi reddetme komutu işleniyor. ReviewId: {ReviewId}, RejectedByUserId: {RejectedByUserId}", 
                request.ReviewId, request.RejectedByUserId);

            var result = await _productReviewService.RejectReviewAsync(
                request.ReviewId,
                request.RejectionReason,
                request.RejectedByUserId,
                cancellationToken);

            if (!result)
            {
                _logger.LogWarning("Ürün değerlendirmesi reddedilemedi. ReviewId: {ReviewId}", request.ReviewId);
                return Result.Failure<bool>(Error.NotFound("ProductReview", request.ReviewId.ToString()));
            }

            _logger.LogInformation("Ürün değerlendirmesi başarıyla reddedildi. ReviewId: {ReviewId}", request.ReviewId);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmesi reddetme sırasında hata oluştu. ReviewId: {ReviewId}", request.ReviewId);
            return Result.Failure<bool>(Error.Failure("RejectProductReview.Failed", $"Ürün değerlendirmesi reddedilirken hata oluştu: {ex.Message}"));
        }
    }
}

/// <summary>
/// Ürün değerlendirmesi reddetme komut validator'ı
/// </summary>
public class RejectProductReviewCommandValidator : AbstractValidator<RejectProductReviewCommand>
{
    public RejectProductReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId)
            .NotEmpty()
            .WithMessage("Değerlendirme ID'si boş olamaz");

        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .WithMessage("Red sebebi boş olamaz")
            .MaximumLength(500)
            .WithMessage("Red sebebi en fazla 500 karakter olabilir");

        RuleFor(x => x.RejectedByUserId)
            .NotEmpty()
            .WithMessage("Reddeden kullanıcı ID'si boş olamaz");
    }
}
