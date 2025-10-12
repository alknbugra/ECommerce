using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Application.Features.Products.Commands.DeleteProductImage;

/// <summary>
/// Ürün resmi silme komutu
/// </summary>
public class DeleteProductImageCommand : ICommand<bool>
{
    /// <summary>
    /// Ürün resmi ID'si
    /// </summary>
    public Guid ImageId { get; set; }

    /// <summary>
    /// İstek yapan kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }
}
