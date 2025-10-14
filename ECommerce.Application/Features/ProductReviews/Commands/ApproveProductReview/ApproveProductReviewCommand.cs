using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace ECommerce.Application.Features.ProductReviews.Commands.ApproveProductReview;

/// <summary>
/// Ürün değerlendirmesi onaylama komutu
/// </summary>
public class ApproveProductReviewCommand : ICommand<bool>
{
    /// <summary>
    /// Değerlendirme ID'si
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Onaylayan kullanıcı ID'si
    /// </summary>
    public Guid ApprovedByUserId { get; set; }
}

/// <summary>
/// Ürün değerlendirmesi onaylama komut işleyicisi
/// </summary>
public class ApproveProductReviewCommandHandler : ICommandHandler<ApproveProductReviewCommand, bool>
{
    private readonly IProductReviewService _productReviewService;
    private readonly ILogger<ApproveProductReviewCommandHandler> _logger;

    public ApproveProductReviewCommandHandler(
        IProductReviewService productReviewService,
        ILogger<ApproveProductReviewCommandHandler> logger)
    {
        _productReviewService = productReviewService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ApproveProductReviewCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ürün değerlendirmesi onaylama komutu işleniyor. ReviewId: {ReviewId}, ApprovedByUserId: {ApprovedByUserId}", 
                request.ReviewId, request.ApprovedByUserId);

            var result = await _productReviewService.ApproveReviewAsync(
                request.ReviewId,
                request.ApprovedByUserId,
                cancellationToken);

            if (!result)
            {
                _logger.LogWarning("Ürün değerlendirmesi onaylanamadı. ReviewId: {ReviewId}", request.ReviewId);
                return Result.Failure<bool>(Error.NotFound("ProductReview", request.ReviewId.ToString()));
            }

            _logger.LogInformation("Ürün değerlendirmesi başarıyla onaylandı. ReviewId: {ReviewId}", request.ReviewId);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmesi onaylama sırasında hata oluştu. ReviewId: {ReviewId}", request.ReviewId);
            return Result.Failure<bool>(Error.Failure("ApproveProductReview.Failed", $"Ürün değerlendirmesi onaylanırken hata oluştu: {ex.Message}"));
        }
    }
}

/// <summary>
/// Ürün değerlendirmesi onaylama komut validator'ı
/// </summary>
public class ApproveProductReviewCommandValidator : AbstractValidator<ApproveProductReviewCommand>
{
    public ApproveProductReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId)
            .NotEmpty()
            .WithMessage("Değerlendirme ID'si boş olamaz");

        RuleFor(x => x.ApprovedByUserId)
            .NotEmpty()
            .WithMessage("Onaylayan kullanıcı ID'si boş olamaz");
    }
}
