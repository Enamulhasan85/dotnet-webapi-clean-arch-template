using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Domain.Identity;

namespace Template.Application.Features.Authentication.Commands;

public class ResetPasswordCommand : BaseCommand<Result<bool>>
{
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required string NewPassword { get; set; }
}

public class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ResetPasswordHandler> _logger;

    public ResetPasswordHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<ResetPasswordHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<bool>> HandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Processing reset password request for email: {Email}", command.Email);

            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent email: {Email}", command.Email);
                return Result<bool>.Failure("Invalid email or token");
            }

            var result = await _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset successfully for email: {Email}", command.Email);
                return Result<bool>.Success(true);
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            var errorMessage = string.Join(", ", errors);
            _logger.LogWarning("Password reset failed for email: {Email}. Errors: {Errors}", command.Email, errorMessage);
            return Result<bool>.Failure(errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during password reset for email: {Email}", command.Email);
            return Result<bool>.Failure("An error occurred during password reset");
        }
    }
}
