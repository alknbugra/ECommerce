using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Users.Queries.GetUsers;

/// <summary>
/// Kullanıcıları getirme sorgusu
/// </summary>
public class GetUsersQuery : IQuery<List<UserDto>>
{
    /// <summary>
    /// Arama terimi (email, ad, soyad)
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Aktif kullanıcılar mı?
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Kilitli kullanıcılar mı?
    /// </summary>
    public bool? IsLocked { get; set; }

    /// <summary>
    /// E-posta doğrulanmış kullanıcılar mı?
    /// </summary>
    public bool? EmailConfirmed { get; set; }

    /// <summary>
    /// Rol adı
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// Başlangıç tarihi
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Bitiş tarihi
    /// </summary>
    public DateTime? EndDate { get; set; }

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
    public string? SortBy { get; set; } = "CreatedAt";

    /// <summary>
    /// Sıralama yönü
    /// </summary>
    public string SortDirection { get; set; } = "desc";
}
