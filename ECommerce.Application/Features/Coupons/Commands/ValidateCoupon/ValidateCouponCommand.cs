using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace ECommerce.Application.Features.Coupons.Commands.ValidateCoupon;

/// <summary>
/// Kupon doğrulama komutu
/// </summary>
public class ValidateCouponCommand : ICommand<CouponValidationResultDto>
{
    /// <summary>
    /// Kupon doğrulama DTO'su
    /// </summary>
    public ValidateCouponDto ValidateCouponDto { get; set; } = null!;
}

/// <summary>
/// Kupon doğrulama komut işleyicisi
/// </summary>
public class ValidateCouponCommandHandler : ICommandHandler<ValidateCouponCommand, CouponValidationResultDto>
{
    private readonly ICouponService _couponService;
    private readonly ILogger<ValidateCouponCommandHandler> _logger;

    public ValidateCouponCommandHandler(
        ICouponService couponService,
        ILogger<ValidateCouponCommandHandler> logger)
    {
        _couponService = couponService;
        _logger = logger;
    }

    public async Task<Result<CouponValidationResultDto>> Handle(ValidateCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kupon doğrulama komutu işleniyor. Code: {Code}, UserId: {UserId}", 
                request.ValidateCouponDto.Code, request.ValidateCouponDto.UserId);

            var result = await _couponService.ValidateCouponAsync(
                request.ValidateCouponDto,
                cancellationToken);

            _logger.LogInformation("Kupon doğrulama tamamlandı. Code: {Code}, IsValid: {IsValid}", 
                request.ValidateCouponDto.Code, result.IsValid);

            return Result.Success<CouponValidationResultDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon doğrulama sırasında hata oluştu. Code: {Code}", request.ValidateCouponDto.Code);
            return Result.Failure<CouponValidationResultDto>(Error.Problem("Coupon.ValidationError", "Kupon doğrulama sırasında bir hata oluştu."));
        }
    }
}

/// <summary>
/// Kupon doğrulama komut validator'ı
/// </summary>
public class ValidateCouponCommandValidator : AbstractValidator<ValidateCouponCommand>
{
    public ValidateCouponCommandValidator()
    {
        RuleFor(x => x.ValidateCouponDto)
            .NotNull()
            .WithMessage("Kupon doğrulama bilgileri boş olamaz");

        RuleFor(x => x.ValidateCouponDto.Code)
            .NotEmpty()
            .WithMessage("Kupon kodu boş olamaz");

        RuleFor(x => x.ValidateCouponDto.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID'si boş olamaz");

        RuleFor(x => x.ValidateCouponDto.OrderAmount)
            .GreaterThan(0)
            .WithMessage("Sipariş tutarı 0'dan büyük olmalıdır");
    }
}
