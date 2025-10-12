using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Users.Commands.UpdateUserStatus;

/// <summary>
/// Kullanıcı durumu güncelleme komutu (Admin)
/// </summary>
public class UpdateUserStatusCommand : ICommand<UserDto>
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Kullanıcı aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Hesap kilitli mi?
    /// </summary>
    public bool IsLocked { get; set; }

    /// <summary>
    /// E-posta doğrulandı mı?
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// Telefon doğrulandı mı?
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// Güncelleme notu
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Güncelleyen kullanıcı ID'si (Admin)
    /// </summary>
    public Guid UpdatedByUserId { get; set; }
}
