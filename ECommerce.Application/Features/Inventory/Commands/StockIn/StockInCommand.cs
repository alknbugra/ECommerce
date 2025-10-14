using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Application.Features.Inventory.Commands.StockIn;

/// <summary>
/// Stok girişi komutu
/// </summary>
public class StockInCommand : ICommand<bool>
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Miktar
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Neden
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid? UserId { get; set; }
}
