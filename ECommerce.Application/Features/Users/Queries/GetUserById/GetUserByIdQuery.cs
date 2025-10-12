using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// ID'ye göre kullanıcı getirme sorgusu
/// </summary>
public class GetUserByIdQuery : IQuery<UserDto?>
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// İstek yapan kullanıcı ID'si (güvenlik için)
    /// </summary>
    public Guid? RequestingUserId { get; set; }
}
