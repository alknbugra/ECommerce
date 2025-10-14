using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Users.Commands.UpdateUserProfile;
using ECommerce.Application.Features.Users.Commands.ChangePassword;
using ECommerce.Application.Features.Users.Commands.UpdateUserStatus;
using ECommerce.Application.Features.Users.Queries.GetUserById;
using ECommerce.Application.Features.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Kullanıcı endpoint'leri
/// </summary>
public static class UsersEndpoints
{
    /// <summary>
    /// Kullanıcı endpoint'lerini kaydet
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users")
            .WithOpenApi();

        // Tüm kullanıcıları getir (Admin)
        group.MapGet("/", GetUsers)
            .WithName("GetUsers")
            .WithSummary("Kullanıcıları getir")
            .WithDescription("Filtreleme, arama ve sayfalama ile kullanıcıları getirir")
            .RequireAuthorization()
            .Produces<List<UserDto>>(200)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // ID'ye göre kullanıcı getir
        group.MapGet("/{id:guid}", GetUserById)
            .WithName("GetUserById")
            .WithSummary("ID'ye göre kullanıcı getir")
            .WithDescription("Belirtilen ID'ye sahip kullanıcıyı getirir")
            .RequireAuthorization()
            .Produces<UserDto>(200)
            .Produces(401)
            .Produces(404)
            .Produces(500);

        // Mevcut kullanıcı profilini getir
        group.MapGet("/me", GetMyProfile)
            .WithName("GetMyProfile")
            .WithSummary("Mevcut kullanıcı profilini getir")
            .WithDescription("Giriş yapmış kullanıcının profil bilgilerini getirir")
            .RequireAuthorization()
            .Produces<UserDto>(200)
            .Produces(401)
            .Produces(500);

        // Kullanıcı profilini güncelle
        group.MapPut("/me", UpdateMyProfile)
            .WithName("UpdateMyProfile")
            .WithSummary("Kullanıcı profilini güncelle")
            .WithDescription("Giriş yapmış kullanıcının profil bilgilerini günceller")
            .RequireAuthorization()
            .Produces<UserDto>(200)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // Şifre değiştir
        group.MapPut("/me/change-password", ChangePassword)
            .WithName("ChangePassword")
            .WithSummary("Şifre değiştir")
            .WithDescription("Giriş yapmış kullanıcının şifresini değiştirir")
            .RequireAuthorization()
            .Produces<bool>(200)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // Kullanıcı durumu güncelle (Admin)
        group.MapPut("/{id:guid}/status", UpdateUserStatus)
            .WithName("UpdateUserStatus")
            .WithSummary("Kullanıcı durumu güncelle")
            .WithDescription("Kullanıcının durum bilgilerini günceller (Admin)")
            .RequireAuthorization()
            .Produces<UserDto>(200)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);
    }

    /// <summary>
    /// Kullanıcıları getir (Admin)
    /// </summary>
    private static async Task<IResult> GetUsers(
        [AsParameters] GetUsersRequest request,
        [FromServices] IQueryHandler<GetUsersQuery, List<UserDto>> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUsersQuery
        {
            SearchTerm = request.SearchTerm,
            IsActive = request.IsActive,
            IsLocked = request.IsLocked,
            EmailConfirmed = request.EmailConfirmed,
            RoleName = request.RoleName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var users = await handler.Handle(query, cancellationToken);
        return Results.Ok(users);
    }

    /// <summary>
    /// ID'ye göre kullanıcı getir
    /// </summary>
    private static async Task<IResult> GetUserById(
        Guid id,
        [FromServices] IQueryHandler<GetUserByIdQuery, UserDto?> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var requestingUserId = GetUserIdFromContext(context);
        var query = new GetUserByIdQuery 
        { 
            Id = id,
            RequestingUserId = requestingUserId
        };

        var user = await handler.Handle(query, cancellationToken);
        if (user == null)
        {
            return Results.NotFound($"ID'si {id} olan kullanıcı bulunamadı.");
        }

        return Results.Ok(user);
    }

    /// <summary>
    /// Mevcut kullanıcı profilini getir
    /// </summary>
    private static async Task<IResult> GetMyProfile(
        [FromServices] IQueryHandler<GetUserByIdQuery, UserDto?> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        var query = new GetUserByIdQuery 
        { 
            Id = userId.Value,
            RequestingUserId = userId
        };

        var user = await handler.Handle(query, cancellationToken);
        if (user == null)
        {
            return Results.NotFound("Kullanıcı bulunamadı.");
        }

        return Results.Ok(user);
    }

    /// <summary>
    /// Kullanıcı profilini güncelle
    /// </summary>
    private static async Task<IResult> UpdateMyProfile(
        [FromBody] UpdateUserProfileCommand command,
        [FromServices] ICommandHandler<UpdateUserProfileCommand, UserDto> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        command.UserId = userId.Value;
        var user = await handler.Handle(command, cancellationToken);
        return Results.Ok(user);
    }

    /// <summary>
    /// Şifre değiştir
    /// </summary>
    private static async Task<IResult> ChangePassword(
        [FromBody] ChangePasswordCommand command,
        [FromServices] ICommandHandler<ChangePasswordCommand, bool> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        command.UserId = userId.Value;
        var result = await handler.Handle(command, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// Kullanıcı durumu güncelle (Admin)
    /// </summary>
    private static async Task<IResult> UpdateUserStatus(
        Guid id,
        [FromBody] UpdateUserStatusRequest request,
        [FromServices] ICommandHandler<UpdateUserStatusCommand, UserDto> handler,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        var command = new UpdateUserStatusCommand
        {
            UserId = id,
            IsActive = request.IsActive,
            IsLocked = request.IsLocked,
            EmailConfirmed = request.EmailConfirmed,
            PhoneNumberConfirmed = request.PhoneNumberConfirmed,
            Notes = request.Notes,
            UpdatedByUserId = userId.Value
        };

        var user = await handler.Handle(command, cancellationToken);
        return Results.Ok(user);
    }

    /// <summary>
    /// HttpContext'ten kullanıcı ID'sini al
    /// </summary>
    private static Guid? GetUserIdFromContext(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        return null;
    }

    /// <summary>
    /// GetUsers request modeli
    /// </summary>
    public record GetUsersRequest(
        string? SearchTerm = null,
        bool? IsActive = null,
        bool? IsLocked = null,
        bool? EmailConfirmed = null,
        string? RoleName = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        int PageNumber = 1,
        int PageSize = 10,
        string? SortBy = "CreatedAt",
        string SortDirection = "desc");

    /// <summary>
    /// UpdateUserStatus request modeli
    /// </summary>
    public record UpdateUserStatusRequest(
        bool IsActive,
        bool IsLocked,
        bool EmailConfirmed,
        bool PhoneNumberConfirmed,
        string? Notes = null);
}
