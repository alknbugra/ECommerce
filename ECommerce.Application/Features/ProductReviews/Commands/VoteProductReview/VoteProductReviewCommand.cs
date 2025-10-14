using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace ECommerce.Application.Features.ProductReviews.Commands.VoteProductReview;

/// <summary>
/// Ürün değerlendirmesi oylama komutu
/// </summary>
public class VoteProductReviewCommand : ICommand<bool>
{
    /// <summary>
    /// Değerlendirme ID'si
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Oy türü
    /// </summary>
    public string VoteType { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcı IP adresi
    /// </summary>
    public string? UserIpAddress { get; set; }
}

/// <summary>
/// Ürün değerlendirmesi oylama komut işleyicisi
/// </summary>
public class VoteProductReviewCommandHandler : ICommandHandler<VoteProductReviewCommand, bool>
{
    private readonly IProductReviewService _productReviewService;
    private readonly ILogger<VoteProductReviewCommandHandler> _logger;

    public VoteProductReviewCommandHandler(
        IProductReviewService productReviewService,
        ILogger<VoteProductReviewCommandHandler> logger)
    {
        _productReviewService = productReviewService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(VoteProductReviewCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Ürün değerlendirmesi oylama komutu işleniyor. ReviewId: {ReviewId}, UserId: {UserId}, VoteType: {VoteType}", 
                request.ReviewId, request.UserId, request.VoteType);

            var result = await _productReviewService.VoteReviewAsync(
                request.ReviewId,
                request.UserId,
                request.VoteType,
                request.UserIpAddress,
                cancellationToken);

            if (!result)
            {
                _logger.LogWarning("Ürün değerlendirmesi oylanamadı. ReviewId: {ReviewId}, UserId: {UserId}", 
                    request.ReviewId, request.UserId);
                return Result.Failure<bool>(Error.Validation("ProductReview", "Ürün değerlendirmesi oylanamadı."));
            }

            _logger.LogInformation("Ürün değerlendirmesi başarıyla oylandı. ReviewId: {ReviewId}, UserId: {UserId}", 
                request.ReviewId, request.UserId);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün değerlendirmesi oylama sırasında hata oluştu. ReviewId: {ReviewId}", request.ReviewId);
            return Result.Failure<bool>(Error.Failure("VoteProductReview.Failed", $"Ürün değerlendirmesi oylanırken hata oluştu: {ex.Message}"));
        }
    }
}

/// <summary>
/// Ürün değerlendirmesi oylama komut validator'ı
/// </summary>
public class VoteProductReviewCommandValidator : AbstractValidator<VoteProductReviewCommand>
{
    public VoteProductReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId)
            .NotEmpty()
            .WithMessage("Değerlendirme ID'si boş olamaz");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID'si boş olamaz");

        RuleFor(x => x.VoteType)
            .NotEmpty()
            .WithMessage("Oy türü boş olamaz")
            .Must(BeValidVoteType)
            .WithMessage("Geçersiz oy türü");
    }

    private bool BeValidVoteType(string voteType)
    {
        var validTypes = new[] { "Helpful", "NotHelpful" };
        return validTypes.Contains(voteType);
    }
}
