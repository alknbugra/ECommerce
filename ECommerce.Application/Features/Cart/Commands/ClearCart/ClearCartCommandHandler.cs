using AutoMapper;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Cart.Commands.ClearCart;

/// <summary>
/// Sepeti temizleme komut işleyicisi
/// </summary>
public class ClearCartCommandHandler : ICommandHandler<ClearCartCommand, CartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ClearCartCommandHandler> _logger;

    public ClearCartCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<ClearCartCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<CartDto>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sepet temizleme işlemi başlatıldı. UserId: {UserId}, SessionId: {SessionId}", 
                request.UserId, request.SessionId);

            // Sepeti bul
            var cart = await GetCartAsync(request.UserId, request.SessionId, cancellationToken);

            if (cart == null)
            {
                return Result.Failure<CartDto>(Error.NotFound("Cart.NotFound", "Sepet bulunamadı."));
            }

            // Tüm sepet ürünlerini sil
            var cartItems = await _unitOfWork.CartItems
                .GetAll()
                .Where(ci => ci.CartId == cart.Id)
                .ToListAsync(cancellationToken);

            foreach (var cartItem in cartItems)
            {
                await _unitOfWork.CartItems.DeleteAsync(cartItem);
            }

            // Sepeti güncelle
            cart.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Carts.UpdateAsync(cart);

            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Sepet başarıyla temizlendi. CartId: {CartId}", cart.Id);

            // Güncellenmiş sepeti döndür
            var cartDto = await GetCartWithItemsAsync(cart.Id, cancellationToken);
            return Result.Success<CartDto>(cartDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sepet temizleme sırasında hata oluştu. UserId: {UserId}, SessionId: {SessionId}", 
                request.UserId, request.SessionId);
            return Result.Failure<CartDto>(Error.Problem("Cart.ClearCartError", "Sepet temizleme sırasında bir hata oluştu."));
        }
    }

    private async Task<Domain.Entities.Cart?> GetCartAsync(Guid? userId, string? sessionId, CancellationToken cancellationToken)
    {
        if (userId.HasValue)
        {
            return await _unitOfWork.Carts
                .GetAll()
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(sessionId))
        {
            return await _unitOfWork.Carts
                .GetAll()
                .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.IsActive, cancellationToken);
        }

        return null;
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
