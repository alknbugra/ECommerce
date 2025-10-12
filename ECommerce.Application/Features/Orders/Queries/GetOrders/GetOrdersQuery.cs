using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Orders.Queries.GetOrders;

/// <summary>
/// Siparişleri getirme sorgusu
/// </summary>
public class GetOrdersQuery : IQuery<List<OrderDto>>
{
    /// <summary>
    /// Kullanıcı ID'si (null ise tüm siparişler)
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Sipariş durumu
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Ödeme durumu
    /// </summary>
    public string? PaymentStatus { get; set; }

    /// <summary>
    /// Başlangıç tarihi
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Bitiş tarihi
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Arama terimi (sipariş numarası, kullanıcı adı)
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Sayfa numarası
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Sıralama alanı
    /// </summary>
    public string? SortBy { get; set; } = "OrderDate";

    /// <summary>
    /// Sıralama yönü
    /// </summary>
    public string SortDirection { get; set; } = "desc";
}
