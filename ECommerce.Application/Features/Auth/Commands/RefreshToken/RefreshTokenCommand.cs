using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// Refresh token komutu
/// </summary>
public class RefreshTokenCommand : ICommand<AuthResponseDto>
{
    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}
