using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Application.Features.Users.Commands.ChangePassword;

/// <summary>
/// Şifre değiştirme komutu
/// </summary>
public class ChangePasswordCommand : ICommand<bool>
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Mevcut şifre
    /// </summary>
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// Yeni şifre
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;
}
