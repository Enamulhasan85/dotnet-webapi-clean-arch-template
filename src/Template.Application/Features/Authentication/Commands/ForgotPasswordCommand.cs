using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Domain.Identity;

namespace Template.Application.Features.Authentication.Commands;

public class ForgotPasswordCommand : BaseCommand<Result<bool>>
{
    public required string Email { get; set; }
}

public class ForgotPasswordHandler : ICommandHandler<ForgotPasswordCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ForgotPasswordHandler> _logger;

    public ForgotPasswordHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<ForgotPasswordHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<bool>> HandleAsync(ForgotPasswordCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Processing forgot password request for email: {Email}", command.Email);

            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                // For security reasons, we don't reveal if the email exists
                _logger.LogWarning("Forgot password attempted for non-existent email: {Email}", command.Email);
                return Result<bool>.Success(true);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // TODO: Send email with reset token
            // This would typically involve an email service
            
            _logger.LogInformation("Password reset token generated for user: {UserId}", user.Id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during forgot password for email: {Email}", command.Email);
            return Result<bool>.Failure("An error occurred during forgot password process");
        }
    }
}
