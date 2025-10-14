using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Auth.Commands.Login;
using ECommerce.Application.Features.Auth.Commands.Register;
using ECommerce.Application.Features.Auth.Commands.RefreshToken;
using ECommerce.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Authentication endpoint'leri
/// </summary>
public static class AuthEndpoints
{
    /// <summary>
    /// Auth endpoint'lerini kaydet
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Authentication")
            .WithOpenApi();

        // Kullanıcı girişi
        group.MapPost("/login", Login)
            .WithName("Login")
            .WithSummary("Kullanıcı girişi")
            .WithDescription("Email ve şifre ile kullanıcı girişi yapar")
            .Produces<AuthResponseDto>(200)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // Kullanıcı kaydı
        group.MapPost("/register", Register)
            .WithName("Register")
            .WithSummary("Kullanıcı kaydı")
            .WithDescription("Yeni kullanıcı kaydı oluşturur")
            .Produces<AuthResponseDto>(201)
            .Produces(400)
            .Produces(500);

        // Token yenileme
        group.MapPost("/refresh", RefreshToken)
            .WithName("RefreshToken")
            .WithSummary("Token yenileme")
            .WithDescription("Refresh token ile yeni access token oluşturur")
            .Produces<AuthResponseDto>(200)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // Şifre gereksinimlerini getir
        group.MapGet("/password-requirements", GetPasswordRequirements)
            .WithName("GetPasswordRequirements")
            .WithSummary("Şifre gereksinimleri")
            .WithDescription("Şifre güçlülük gereksinimlerini getirir")
            .Produces<List<string>>(200);
    }

    /// <summary>
    /// Kullanıcı girişi
    /// </summary>
    private static async Task<IResult> Login(
        [FromBody] LoginCommand command,
        [FromServices] ICommandHandler<LoginCommand, AuthResponseDto> handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(command, cancellationToken);
        
        return result.Match(
            success => Results.Ok(success),
            failure => CustomResults.Problem(result));
    }

    /// <summary>
    /// Kullanıcı kaydı
    /// </summary>
    private static async Task<IResult> Register(
        [FromBody] RegisterCommand command,
        [FromServices] ICommandHandler<RegisterCommand, AuthResponseDto> handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(command, cancellationToken);
        
        return result.Match(
            success => Results.CreatedAtRoute("Login", success),
            failure => CustomResults.Problem(result));
    }

    /// <summary>
    /// Token yenileme
    /// </summary>
    private static async Task<IResult> RefreshToken(
        [FromBody] RefreshTokenDto request,
        [FromServices] ICommandHandler<RefreshTokenCommand, AuthResponseDto> handler,
        CancellationToken cancellationToken = default)
    {
        var command = new RefreshTokenCommand
        {
            RefreshToken = request.RefreshToken
        };

        var result = await handler.Handle(command, cancellationToken);
        
        return result.Match(
            success => Results.Ok(success),
            failure => CustomResults.Problem(result));
    }

    /// <summary>
    /// Şifre gereksinimlerini getir
    /// </summary>
    private static IResult GetPasswordRequirements(
        [FromServices] IPasswordService passwordService)
    {
        var requirements = passwordService.GetPasswordRequirements();
        return Results.Ok(requirements);
    }
}
