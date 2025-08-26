using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Domain.Identity;

namespace Template.Application.Features.Authentication.Commands;

public class ChangePasswordCommand : BaseCommand<Result<bool>>
{
    public required string UserId { get; set; }
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}

public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ChangePasswordHandler> _logger;

    public ChangePasswordHandler(UserManager<ApplicationUser> userManager, ILogger<ChangePasswordHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<bool>> HandleAsync(ChangePasswordCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Processing change password request for user: {UserId}", command.UserId);

            var user = await _userManager.FindByIdAsync(command.UserId);
            if (user == null)
            {
                return Result<bool>.Failure("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogWarning("Password change failed for user: {UserId}. Errors: {Errors}",
                    command.UserId, string.Join(", ", errors));
                return Result<bool>.Failure(errors);
            }

            _logger.LogInformation("Password changed successfully for user: {UserId}", command.UserId);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during password change for user: {UserId}", command.UserId);
            return Result<bool>.Failure("An error occurred during password change");
        }
    }
}
