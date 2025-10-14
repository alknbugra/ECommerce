using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

/// <summary>
/// Kargo şirketi endpoint'leri
/// </summary>
public static class CargoCompanyEndpoints
{
    /// <summary>
    /// Kargo şirketi endpoint'lerini kaydet
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapCargoCompanyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/cargo-companies")
            .WithTags("Cargo Companies")
            .WithOpenApi();

        // Tüm kargo şirketlerini getir (Public)
        group.MapGet("/", GetCargoCompanies)
            .WithName("GetCargoCompanies")
            .WithSummary("Kargo şirketlerini getir")
            .WithDescription("Tüm aktif kargo şirketlerini getirir")
            .Produces<List<CargoCompanyDto>>(200)
            .Produces(500);

        // Aktif kargo şirketlerini getir (Public)
        group.MapGet("/active", GetActiveCargoCompanies)
            .WithName("GetActiveCargoCompanies")
            .WithSummary("Aktif kargo şirketlerini getir")
            .WithDescription("Sadece aktif kargo şirketlerini getirir")
            .Produces<List<CargoCompanyDto>>(200)
            .Produces(500);

        // ID'ye göre kargo şirketi getir (Public)
        group.MapGet("/{id:guid}", GetCargoCompanyById)
            .WithName("GetCargoCompanyById")
            .WithSummary("ID'ye göre kargo şirketi getir")
            .WithDescription("Belirtilen ID'ye sahip kargo şirketini getirir")
            .Produces<CargoCompanyDto>(200)
            .Produces(404)
            .Produces(500);

        // Koda göre kargo şirketi getir (Public)
        group.MapGet("/code/{code}", GetCargoCompanyByCode)
            .WithName("GetCargoCompanyByCode")
            .WithSummary("Koda göre kargo şirketi getir")
            .WithDescription("Belirtilen koda sahip kargo şirketini getirir")
            .Produces<CargoCompanyDto>(200)
            .Produces(404)
            .Produces(500);

        // Kargo şirketi ara (Public)
        group.MapGet("/search/{name}", SearchCargoCompanies)
            .WithName("SearchCargoCompanies")
            .WithSummary("Kargo şirketi ara")
            .WithDescription("İsme göre kargo şirketlerini arar")
            .Produces<List<CargoCompanyDto>>(200)
            .Produces(500);

        // Kargo şirketi oluştur (Admin)
        group.MapPost("/", CreateCargoCompany)
            .WithName("CreateCargoCompany")
            .WithSummary("Kargo şirketi oluştur")
            .WithDescription("Yeni kargo şirketi oluşturur")
            .RequireAuthorization()
            .Produces<CargoCompanyDto>(201)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        // Kargo şirketi güncelle (Admin)
        group.MapPut("/{id:guid}", UpdateCargoCompany)
            .WithName("UpdateCargoCompany")
            .WithSummary("Kargo şirketi güncelle")
            .WithDescription("Mevcut kargo şirketini günceller")
            .RequireAuthorization()
            .Produces<CargoCompanyDto>(200)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);

        // Kargo şirketi aktif durumunu güncelle (Admin)
        group.MapPut("/{id:guid}/active-status", SetCargoCompanyActiveStatus)
            .WithName("SetCargoCompanyActiveStatus")
            .WithSummary("Kargo şirketi aktif durumunu güncelle")
            .WithDescription("Kargo şirketinin aktif/pasif durumunu günceller")
            .RequireAuthorization()
            .Produces<bool>(200)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);

        // Kargo şirketi sil (Admin)
        group.MapDelete("/{id:guid}", DeleteCargoCompany)
            .WithName("DeleteCargoCompany")
            .WithSummary("Kargo şirketi sil")
            .WithDescription("Kargo şirketini siler")
            .RequireAuthorization()
            .Produces<bool>(200)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);
    }

    /// <summary>
    /// Tüm kargo şirketlerini getir
    /// </summary>
    private static async Task<IResult> GetCargoCompanies(
        ICargoCompanyService cargoCompanyService,
        CancellationToken cancellationToken)
    {
        try
        {
            var companies = await cargoCompanyService.GetAllCargoCompaniesAsync(cancellationToken);
            return Results.Ok(companies);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Kargo şirketleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    /// <summary>
    /// Aktif kargo şirketlerini getir
    /// </summary>
    private static async Task<IResult> GetActiveCargoCompanies(
        ICargoCompanyService cargoCompanyService,
        CancellationToken cancellationToken)
    {
        try
        {
            var companies = await cargoCompanyService.GetActiveCargoCompaniesAsync(cancellationToken);
            return Results.Ok(companies);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Aktif kargo şirketleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    /// <summary>
    /// ID'ye göre kargo şirketi getir
    /// </summary>
    private static async Task<IResult> GetCargoCompanyById(
        Guid id,
        ICargoCompanyService cargoCompanyService,
        CancellationToken cancellationToken)
    {
        try
        {
            var company = await cargoCompanyService.GetCargoCompanyByIdAsync(id, cancellationToken);
            return company != null ? Results.Ok(company) : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.Problem($"Kargo şirketi getirilirken hata oluştu: {ex.Message}");
        }
    }

    /// <summary>
    /// Koda göre kargo şirketi getir
    /// </summary>
    private static async Task<IResult> GetCargoCompanyByCode(
        string code,
        ICargoCompanyService cargoCompanyService,
        CancellationToken cancellationToken)
    {
        try
        {
            var company = await cargoCompanyService.GetCargoCompanyByCodeAsync(code, cancellationToken);
            return company != null ? Results.Ok(company) : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.Problem($"Kargo şirketi getirilirken hata oluştu: {ex.Message}");
        }
    }

    /// <summary>
    /// Kargo şirketi ara
    /// </summary>
    private static async Task<IResult> SearchCargoCompanies(
        string name,
        ICargoCompanyService cargoCompanyService,
        CancellationToken cancellationToken)
    {
        try
        {
            var companies = await cargoCompanyService.SearchCargoCompaniesAsync(name, cancellationToken);
            return Results.Ok(companies);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Kargo şirketleri aranırken hata oluştu: {ex.Message}");
        }
    }

    /// <summary>
    /// Kargo şirketi oluştur
    /// </summary>
    private static async Task<IResult> CreateCargoCompany(
        CreateCargoCompanyDto createDto,
        ICargoCompanyService cargoCompanyService,
        CancellationToken cancellationToken)
    {
        try
        {
            var company = await cargoCompanyService.CreateCargoCompanyAsync(createDto, cancellationToken);
            return Results.Created($"/api/cargo-companies/{company.Id}", company);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Kargo şirketi oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    /// <summary>
    /// Kargo şirketi güncelle
    /// </summary>
    private static async Task<IResult> UpdateCargoCompany(
        Guid id,
        UpdateCargoCompanyDto updateDto,
        ICargoCompanyService cargoCompanyService,
        CancellationToken cancellationToken)
    {
        try
        {
            var company = await cargoCompanyService.UpdateCargoCompanyAsync(id, updateDto, cancellationToken);
            return Results.Ok(company);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Kargo şirketi güncellenirken hata oluştu: {ex.Message}");
        }
    }

    /// <summary>
    /// Kargo şirketi aktif durumunu güncelle
    /// </summary>
    private static async Task<IResult> SetCargoCompanyActiveStatus(
        Guid id,
        [FromBody] bool isActive,
        ICargoCompanyService cargoCompanyService,
        CancellationToken cancellationToken)
    {
        try
        {
            var success = await cargoCompanyService.SetCargoCompanyActiveStatusAsync(id, isActive, cancellationToken);
            return success ? Results.Ok(true) : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.Problem($"Kargo şirketi aktif durumu güncellenirken hata oluştu: {ex.Message}");
        }
    }

    /// <summary>
    /// Kargo şirketi sil
    /// </summary>
    private static async Task<IResult> DeleteCargoCompany(
        Guid id,
        ICargoCompanyService cargoCompanyService,
        CancellationToken cancellationToken)
    {
        try
        {
            var success = await cargoCompanyService.DeleteCargoCompanyAsync(id, cancellationToken);
            return success ? Results.Ok(true) : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.Problem($"Kargo şirketi silinirken hata oluştu: {ex.Message}");
        }
    }
}
