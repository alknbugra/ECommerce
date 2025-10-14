using AutoMapper;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Cart.Commands.AddToCart;

/// <summary>
/// Sepete ürün ekleme komut işleyicisi
/// </summary>
public class AddToCartCommandHandler : ICommandHandler<AddToCartCommand, CartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AddToCartCommandHandler> _logger;

    public AddToCartCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AddToCartCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<CartDto>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sepete ürün ekleme işlemi başlatıldı. ProductId: {ProductId}, Quantity: {Quantity}", 
                request.ProductId, request.Quantity);

            // Ürünü kontrol et
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result.Failure<CartDto>(Error.NotFound("Cart.ProductNotFound", $"Ürün bulunamadı: {request.ProductId}"));
            }

            if (!product.IsActive)
            {
                return Result.Failure<CartDto>(Error.Problem("Cart.ProductInactive", $"Ürün aktif değil: {product.Name}"));
            }

            if (product.StockQuantity < request.Quantity)
            {
                return Result.Failure<CartDto>(Error.Problem("Cart.InsufficientStock", $"Yetersiz stok: {product.Name} (Mevcut: {product.StockQuantity}, İstenen: {request.Quantity})"));
            }

            // Sepeti bul veya oluştur
            var cart = await GetOrCreateCartAsync(request.UserId, request.SessionId, cancellationToken);

            // Sepette bu ürün var mı kontrol et
            var existingCartItem = await _unitOfWork.CartItems
                .GetAll()
                .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == request.ProductId, cancellationToken);

            if (existingCartItem != null)
            {
                // Mevcut ürünün miktarını artır
                existingCartItem.Quantity += request.Quantity;
                existingCartItem.CalculateTotalPrice();
                existingCartItem.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.CartItems.UpdateAsync(existingCartItem);
            }
            else
            {
                // Yeni sepet ürünü oluştur
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    UnitPrice = product.DiscountedPrice ?? product.Price,
                    AddedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                cartItem.CalculateTotalPrice();
                await _unitOfWork.CartItems.AddAsync(cartItem);
            }

            // Sepeti güncelle
            cart.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Carts.UpdateAsync(cart);

            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Ürün sepete başarıyla eklendi. CartId: {CartId}, ProductId: {ProductId}", 
                cart.Id, request.ProductId);

            // Güncellenmiş sepeti döndür
            var cartDto = await GetCartWithItemsAsync(cart.Id, cancellationToken);
            return Result.Success<CartDto>(cartDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sepete ürün ekleme sırasında hata oluştu. ProductId: {ProductId}", request.ProductId);
            return Result.Failure<CartDto>(Error.Problem("Cart.AddToCartError", "Sepete ürün ekleme sırasında bir hata oluştu."));
        }
    }

    private async Task<Domain.Entities.Cart> GetOrCreateCartAsync(Guid? userId, string? sessionId, CancellationToken cancellationToken)
    {
        Domain.Entities.Cart? cart = null;

        if (userId.HasValue)
        {
            // Kullanıcıya ait aktif sepeti bul
            cart = await _unitOfWork.Carts
                .GetAll()
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(sessionId))
        {
            // Session'a ait aktif sepeti bul
            cart = await _unitOfWork.Carts
                .GetAll()
                .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.IsActive, cancellationToken);
        }

        if (cart == null)
        {
            // Yeni sepet oluştur
            cart = new Domain.Entities.Cart
            {
                UserId = userId ?? Guid.Empty,
                SessionId = sessionId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        return cart;
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
