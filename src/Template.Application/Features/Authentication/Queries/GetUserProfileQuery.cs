using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Queries;
using Template.Application.Common.Models;
using Template.Application.Features.Authentication.DTOs;
using Template.Domain.Identity;

namespace Template.Application.Features.Authentication.Queries;

public class GetUserProfileQuery : BaseQuery<Result<UserProfileResponseDto>>
{
    public required string UserId { get; set; }
}

public class GetUserProfileHandler : IQueryHandler<GetUserProfileQuery, Result<UserProfileResponseDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetUserProfileHandler> _logger;

    public GetUserProfileHandler(UserManager<ApplicationUser> userManager, ILogger<GetUserProfileHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<UserProfileResponseDto>> HandleAsync(GetUserProfileQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Processing get user profile request for user: {UserId}", query.UserId);

            var user = await _userManager.FindByIdAsync(query.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found for ID: {UserId}", query.UserId);
                return Result<UserProfileResponseDto>.Failure("User not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var response = new UserProfileResponseDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName ?? string.Empty,
                EmailConfirmed = user.EmailConfirmed,
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive,
                Roles = userRoles.ToList()
            };

            _logger.LogInformation("User profile retrieved successfully for user: {UserId}", query.UserId);
            return Result<UserProfileResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving user profile for user: {UserId}", query.UserId);
            return Result<UserProfileResponseDto>.Failure("An error occurred while retrieving profile");
        }
    }
}
