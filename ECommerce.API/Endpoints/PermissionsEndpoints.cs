using ECommerce.Application.Features.Permissions.Queries.GetPermissions;
using ECommerce.Application.Features.Permissions.Queries.GetUserPermissions;
using ECommerce.Application.Features.RolePermissions.Commands.AssignPermissionToRole;
using ECommerce.Application.Features.RolePermissions.Queries.GetRolePermissions;
using ECommerce.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Permission endpoint'leri
/// </summary>
public static class PermissionsEndpoints
{
    /// <summary>
    /// Permission endpoint'lerini map et
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapPermissionsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/permissions")
            .WithTags("Permissions")
            .WithOpenApi();

        // Tüm yetkileri getir
        group.MapGet("/", async (
            [AsParameters] GetPermissionsQuery query,
            IQueryHandler<GetPermissionsQuery, IEnumerable<ECommerce.Application.DTOs.PermissionDto>> handler,
            CancellationToken cancellationToken) =>
        {
            var permissions = await handler.HandleAsync(query, cancellationToken);
            return Results.Ok(permissions);
        })
        .WithName("GetPermissions")
        .WithSummary("Tüm yetkileri getir")
        .WithDescription("Sistemdeki tüm yetkileri listeler. Kategori filtresi ve aktif durum filtresi uygulanabilir.")
        .Produces<IEnumerable<ECommerce.Application.DTOs.PermissionDto>>(200)
        .Produces(400)
        .Produces(500);

        // Kategoriye göre yetkileri getir
        group.MapGet("/category/{category}", async (
            string category,
            IQueryHandler<GetPermissionsQuery, IEnumerable<ECommerce.Application.DTOs.PermissionDto>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetPermissionsQuery { Category = category };
            var permissions = await handler.HandleAsync(query, cancellationToken);
            return Results.Ok(permissions);
        })
        .WithName("GetPermissionsByCategory")
        .WithSummary("Kategoriye göre yetkileri getir")
        .WithDescription("Belirtilen kategorideki yetkileri listeler.")
        .Produces<IEnumerable<ECommerce.Application.DTOs.PermissionDto>>(200)
        .Produces(400)
        .Produces(500);

        // Rol yetkilerini getir
        group.MapGet("/roles/{roleId:guid}", async (
            Guid roleId,
            IQueryHandler<GetRolePermissionsQuery, IEnumerable<ECommerce.Application.DTOs.RolePermissionDto>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetRolePermissionsQuery { RoleId = roleId };
            var rolePermissions = await handler.HandleAsync(query, cancellationToken);
            return Results.Ok(rolePermissions);
        })
        .WithName("GetRolePermissions")
        .WithSummary("Rol yetkilerini getir")
        .WithDescription("Belirtilen role atanmış yetkileri listeler.")
        .Produces<IEnumerable<ECommerce.Application.DTOs.RolePermissionDto>>(200)
        .Produces(400)
        .Produces(500);

        // Tüm rol yetki ilişkilerini getir
        group.MapGet("/roles", async (
            [AsParameters] GetRolePermissionsQuery query,
            IQueryHandler<GetRolePermissionsQuery, IEnumerable<ECommerce.Application.DTOs.RolePermissionDto>> handler,
            CancellationToken cancellationToken) =>
        {
            var rolePermissions = await handler.HandleAsync(query, cancellationToken);
            return Results.Ok(rolePermissions);
        })
        .WithName("GetAllRolePermissions")
        .WithSummary("Tüm rol yetki ilişkilerini getir")
        .WithDescription("Sistemdeki tüm rol-yetki ilişkilerini listeler.")
        .Produces<IEnumerable<ECommerce.Application.DTOs.RolePermissionDto>>(200)
        .Produces(400)
        .Produces(500);

        // Role yetki ata
        group.MapPost("/roles/assign", async (
            AssignPermissionToRoleCommand command,
            ICommandHandler<AssignPermissionToRoleCommand, bool> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(command, cancellationToken);
            return result ? Results.Ok(new { success = true, message = "Yetki başarıyla atandı." }) 
                         : Results.BadRequest(new { success = false, message = "Yetki atanamadı." });
        })
        .WithName("AssignPermissionToRole")
        .WithSummary("Role yetki ata")
        .WithDescription("Belirtilen role yetki atar.")
        .Produces(200)
        .Produces(400)
        .Produces(500);

        // Kullanıcı yetkilerini getir
        group.MapGet("/users/{userId:guid}", async (
            Guid userId,
            IQueryHandler<GetUserPermissionsQuery, IEnumerable<ECommerce.Application.DTOs.PermissionDto>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUserPermissionsQuery { UserId = userId };
            var permissions = await handler.HandleAsync(query, cancellationToken);
            return Results.Ok(permissions);
        })
        .WithName("GetUserPermissions")
        .WithSummary("Kullanıcı yetkilerini getir")
        .WithDescription("Belirtilen kullanıcının sahip olduğu yetkileri listeler.")
        .Produces<IEnumerable<ECommerce.Application.DTOs.PermissionDto>>(200)
        .Produces(400)
        .Produces(500);
    }
}
