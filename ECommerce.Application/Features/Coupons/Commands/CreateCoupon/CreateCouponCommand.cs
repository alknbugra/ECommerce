using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace ECommerce.Application.Features.Coupons.Commands.CreateCoupon;

/// <summary>
/// Kupon oluşturma komutu
/// </summary>
public class CreateCouponCommand : ICommand<CouponDto>
{
    /// <summary>
    /// Kupon oluşturma DTO'su
    /// </summary>
    public CreateCouponDto CreateCouponDto { get; set; } = null!;

    /// <summary>
    /// Oluşturan kullanıcı ID'si
    /// </summary>
    public Guid? CreatedByUserId { get; set; }
}

/// <summary>
/// Kupon oluşturma komut işleyicisi
/// </summary>
public class CreateCouponCommandHandler : ICommandHandler<CreateCouponCommand, CouponDto>
{
    private readonly ICouponService _couponService;
    private readonly ILogger<CreateCouponCommandHandler> _logger;

    public CreateCouponCommandHandler(
        ICouponService couponService,
        ILogger<CreateCouponCommandHandler> logger)
    {
        _couponService = couponService;
        _logger = logger;
    }

    public async Task<Result<CouponDto>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kupon oluşturma komutu işleniyor. Code: {Code}", request.CreateCouponDto.Code);

            var result = await _couponService.CreateCouponAsync(
                request.CreateCouponDto,
                request.CreatedByUserId,
                cancellationToken);

            if (result == null)
            {
                _logger.LogWarning("Kupon oluşturulamadı. Code: {Code}", request.CreateCouponDto.Code);
                return Result.Failure<CouponDto>(Error.Problem("Coupon.CreateFailed", "Kupon oluşturulamadı. Kupon kodu zaten mevcut olabilir."));
            }

            _logger.LogInformation("Kupon başarıyla oluşturuldu. Id: {Id}, Code: {Code}", result.Id, result.Code);
            return Result.Success<CouponDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kupon oluşturma sırasında hata oluştu. Code: {Code}", request.CreateCouponDto.Code);
            return Result.Failure<CouponDto>(Error.Problem("Coupon.CreateError", "Kupon oluşturma sırasında bir hata oluştu."));
        }
    }
}

/// <summary>
/// Kupon oluşturma komut validator'ı
/// </summary>
public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
{
    public CreateCouponCommandValidator()
    {
        RuleFor(x => x.CreateCouponDto)
            .NotNull()
            .WithMessage("Kupon bilgileri boş olamaz");

        RuleFor(x => x.CreateCouponDto.Code)
            .NotEmpty()
            .WithMessage("Kupon kodu boş olamaz")
            .MaximumLength(50)
            .WithMessage("Kupon kodu en fazla 50 karakter olabilir");

        RuleFor(x => x.CreateCouponDto.Name)
            .NotEmpty()
            .WithMessage("Kupon adı boş olamaz")
            .MaximumLength(200)
            .WithMessage("Kupon adı en fazla 200 karakter olabilir");

        RuleFor(x => x.CreateCouponDto.DiscountType)
            .NotEmpty()
            .WithMessage("İndirim türü boş olamaz")
            .Must(BeValidDiscountType)
            .WithMessage("Geçersiz indirim türü");

        RuleFor(x => x.CreateCouponDto.DiscountValue)
            .GreaterThan(0)
            .WithMessage("İndirim değeri 0'dan büyük olmalıdır");

        RuleFor(x => x.CreateCouponDto.MinimumOrderAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum sipariş tutarı 0'dan küçük olamaz");

        RuleFor(x => x.CreateCouponDto.MaximumDiscountAmount)
            .GreaterThan(0)
            .When(x => x.CreateCouponDto.MaximumDiscountAmount.HasValue)
            .WithMessage("Maksimum indirim tutarı 0'dan büyük olmalıdır");

        RuleFor(x => x.CreateCouponDto.UsageLimit)
            .GreaterThan(0)
            .When(x => x.CreateCouponDto.UsageLimit.HasValue)
            .WithMessage("Kullanım limiti 0'dan büyük olmalıdır");

        RuleFor(x => x.CreateCouponDto.UsageLimitPerUser)
            .GreaterThan(0)
            .When(x => x.CreateCouponDto.UsageLimitPerUser.HasValue)
            .WithMessage("Kullanıcı başına kullanım limiti 0'dan büyük olmalıdır");

        RuleFor(x => x.CreateCouponDto.StartDate)
            .LessThan(x => x.CreateCouponDto.EndDate)
            .WithMessage("Başlangıç tarihi bitiş tarihinden önce olmalıdır");

        RuleFor(x => x.CreateCouponDto.EndDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Bitiş tarihi gelecekte olmalıdır");
    }

    private bool BeValidDiscountType(string discountType)
    {
        var validTypes = new[] { "Percentage", "FixedAmount", "FreeShipping", "FreeProduct", "BuyOneGetOneHalfPrice", "BuyOneGetOneFree" };
        return validTypes.Contains(discountType);
    }
}
