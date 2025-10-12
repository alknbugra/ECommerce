using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Users.Commands.UpdateUserProfile;

/// <summary>
/// Kullanıcı profil güncelleme komutu
/// </summary>
public class UpdateUserProfileCommand : ICommand<UserDto>
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Ad
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Soyad
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Telefon numarası
    /// </summary>
    public string? PhoneNumber { get; set; }
}
