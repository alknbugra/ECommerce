using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Users.Queries.GetUsers;

/// <summary>
/// Kullanıcıları getirme sorgu handler'ı
/// </summary>
public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUsersQueryHandler> _logger;

    public GetUsersQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetUsersQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Kullanıcılar getiriliyor - SearchTerm: {SearchTerm}, IsActive: {IsActive}", request.SearchTerm, request.IsActive);

            var users = await _unitOfWork.Users.GetAllAsync();

            // Filtreleme
            var filteredUsers = users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                filteredUsers = filteredUsers.Where(u => 
                    u.Email.ToLower().Contains(searchTerm) ||
                    u.FirstName.ToLower().Contains(searchTerm) ||
                    u.LastName.ToLower().Contains(searchTerm));
            }

            if (request.IsActive.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.IsActive == request.IsActive.Value);
            }

            if (request.IsLocked.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.IsLocked == request.IsLocked.Value);
            }

            if (request.EmailConfirmed.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.EmailConfirmed == request.EmailConfirmed.Value);
            }

            if (request.StartDate.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.CreatedAt >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.CreatedAt <= request.EndDate.Value);
            }

            // Sıralama
            filteredUsers = request.SortBy?.ToLower() switch
            {
                "email" => request.SortDirection.ToLower() == "asc"
                    ? filteredUsers.OrderBy(u => u.Email)
                    : filteredUsers.OrderByDescending(u => u.Email),
                "firstname" => request.SortDirection.ToLower() == "asc"
                    ? filteredUsers.OrderBy(u => u.FirstName)
                    : filteredUsers.OrderByDescending(u => u.FirstName),
                "lastname" => request.SortDirection.ToLower() == "asc"
                    ? filteredUsers.OrderBy(u => u.LastName)
                    : filteredUsers.OrderByDescending(u => u.LastName),
                "isactive" => request.SortDirection.ToLower() == "asc"
                    ? filteredUsers.OrderBy(u => u.IsActive)
                    : filteredUsers.OrderByDescending(u => u.IsActive),
                _ => request.SortDirection.ToLower() == "asc"
                    ? filteredUsers.OrderBy(u => u.CreatedAt)
                    : filteredUsers.OrderByDescending(u => u.CreatedAt)
            };

            // Sayfalama
            var pagedUsers = filteredUsers
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // DTO'ya dönüştür
            var userDtos = new List<UserDto>();
            foreach (var user in pagedUsers)
            {
                // Kullanıcı rollerini al
                var userRoles = await _unitOfWork.UserRoles.GetUserRolesAsync(user.Id);
                var roleNames = userRoles.Select(ur => ur.Role.Name).ToList();

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    IsLocked = user.IsLocked,
                    IsActive = user.IsActive,
                    FullName = user.FullName,
                    Roles = roleNames,
                    CreatedAt = user.CreatedAt
                });
            }

            _logger.LogInformation("{Count} kullanıcı getirildi", userDtos.Count);

            return Result.Success(userDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcılar getirilirken hata oluştu");
            return Result.Failure<List<UserDto>>(Error.Failure("GetUsers.Failed", $"Kullanıcılar getirilirken hata oluştu: {ex.Message}"));
        }
    }
}
