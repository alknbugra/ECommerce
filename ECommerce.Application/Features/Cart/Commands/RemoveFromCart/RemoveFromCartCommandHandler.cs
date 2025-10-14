using AutoMapper;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Cart.Commands.RemoveFromCart;

/// <summary>
/// Sepetten ürün çıkarma komut işleyicisi
/// </summary>
public class RemoveFromCartCommandHandler : ICommandHandler<RemoveFromCartCommand, CartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<RemoveFromCartCommandHandler> _logger;

    public RemoveFromCartCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<RemoveFromCartCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<CartDto>> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sepetten ürün çıkarma işlemi başlatıldı. CartItemId: {CartItemId}", request.CartItemId);

            // Sepet ürününü bul
            var cartItem = await _unitOfWork.CartItems
                .GetAll()
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Id == request.CartItemId, cancellationToken);

            if (cartItem == null)
            {
                return Result.Failure<CartDto>(Error.NotFound("Cart.ItemNotFound", $"Sepet ürünü bulunamadı: {request.CartItemId}"));
            }

            // Sepet sahipliğini kontrol et
            var ownershipResult = await ValidateCartOwnershipAsync(cartItem.Cart, request.UserId, request.SessionId, cancellationToken);
            if (ownershipResult.IsFailure)
            {
                return Result.Failure<CartDto>(ownershipResult.Error);
            }

            // Sepet ürününü sil
            await _unitOfWork.CartItems.DeleteAsync(cartItem);

            // Sepeti güncelle
            cartItem.Cart.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Carts.UpdateAsync(cartItem.Cart);

            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Ürün sepetten başarıyla çıkarıldı. CartItemId: {CartItemId}", request.CartItemId);

            // Güncellenmiş sepeti döndür
            var cartDto = await GetCartWithItemsAsync(cartItem.CartId, cancellationToken);
            return Result.Success<CartDto>(cartDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sepetten ürün çıkarma sırasında hata oluştu. CartItemId: {CartItemId}", request.CartItemId);
            return Result.Failure<CartDto>(Error.Problem("Cart.RemoveFromCartError", "Sepetten ürün çıkarma sırasında bir hata oluştu."));
        }
    }

    private async Task<Result> ValidateCartOwnershipAsync(Domain.Entities.Cart cart, Guid? userId, string? sessionId, CancellationToken cancellationToken)
    {
        if (userId.HasValue && cart.UserId != userId)
        {
            return Result.Failure(Error.Problem("Cart.AccessDenied", "Bu sepete erişim yetkiniz yok."));
        }

        if (!userId.HasValue && cart.SessionId != sessionId)
        {
            return Result.Failure(Error.Problem("Cart.AccessDenied", "Bu sepete erişim yetkiniz yok."));
        }

        return Result.Success();
    }

    private async Task<CartDto> GetCartWithItemsAsync(Guid cartId, CancellationToken cancellationToken)
    {
        var cart = await _unitOfWork.Carts
            .GetAll()
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId, cancellationToken);

        if (cart == null)
        {
            throw new NotFoundException($"Sepet bulunamadı: {cartId}");
        }

        var cartDto = _mapper.Map<CartDto>(cart);

        // CartItem DTO'larını manuel olarak map et
        cartDto.CartItems = cart.CartItems.Select(ci => new CartItemDto
        {
            Id = ci.Id,
            CartId = ci.CartId,
            ProductId = ci.ProductId,
            ProductName = ci.Product.Name,
            ProductSku = ci.Product.Sku,
            ProductImageUrl = ci.Product.MainImageUrl,
            Quantity = ci.Quantity,
            UnitPrice = ci.UnitPrice,
            TotalPrice = ci.TotalPrice,
            AddedAt = ci.AddedAt,
            UpdatedAt = ci.UpdatedAt,
            IsInStock = ci.Product.InStock,
            AvailableStock = ci.Product.StockQuantity
        }).ToList();

        return cartDto;
    }
}
