using FluentValidation;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder;

/// <summary>
/// CreateOrder komutu için FluentValidation kuralları
/// </summary>
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı ID'si gereklidir.");

        RuleFor(x => x.ShippingAddressId)
            .NotEmpty()
            .WithMessage("Teslimat adresi gereklidir.");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("Ödeme yöntemi gereklidir.")
            .Must(BeValidPaymentMethod)
            .WithMessage("Geçerli bir ödeme yöntemi seçiniz.");

        RuleFor(x => x.OrderItems)
            .NotEmpty()
            .WithMessage("Sipariş en az bir ürün içermelidir.")
            .Must(HaveValidOrderItems)
            .WithMessage("Sipariş ürünleri geçerli olmalıdır.");

        RuleFor(x => x.ShippingCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Kargo ücreti negatif olamaz.");

        RuleFor(x => x.TaxAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vergi tutarı negatif olamaz.");

        RuleFor(x => x.DiscountAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("İndirim tutarı negatif olamaz.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notlar en fazla 500 karakter olabilir.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }

    private static bool BeValidPaymentMethod(string paymentMethod)
    {
        var validMethods = new[] { "CreditCard", "DebitCard", "BankTransfer", "CashOnDelivery", "PayPal" };
        return validMethods.Contains(paymentMethod);
    }

    private static bool HaveValidOrderItems(List<CreateOrderItemDto> orderItems)
    {
        if (orderItems == null || !orderItems.Any())
            return false;

        return orderItems.All(item => 
            item.ProductId != Guid.Empty &&
            item.Quantity > 0 &&
            item.UnitPrice > 0 &&
            item.DiscountAmount >= 0);
    }
}
