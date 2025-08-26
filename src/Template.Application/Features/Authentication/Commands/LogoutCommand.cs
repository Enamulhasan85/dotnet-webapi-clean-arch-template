using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Domain.Identity;

namespace Template.Application.Features.Authentication.Commands;

public class LogoutCommand : BaseCommand<Result<bool>>
{
    public required string UserId { get; set; }
}

public class LogoutHandler : ICommandHandler<LogoutCommand, Result<bool>>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<LogoutHandler> _logger;

    public LogoutHandler(
        SignInManager<ApplicationUser> signInManager,
        ILogger<LogoutHandler> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<Result<bool>> HandleAsync(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Processing logout request for user: {UserId}", command.UserId);

            await _signInManager.SignOutAsync();
            
            _logger.LogInformation("User logged out successfully: {UserId}", command.UserId);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during logout for user: {UserId}", command.UserId);
            return Result<bool>.Failure("An error occurred during logout");
        }
    }
}
