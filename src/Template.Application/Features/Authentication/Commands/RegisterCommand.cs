using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Features.Authentication.DTOs;
using Template.Domain.Identity;

namespace Template.Application.Features.Authentication.Commands;

public class RegisterCommand : BaseCommand<Result<RegisterResponseDto>>
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterHandler : ICommandHandler<RegisterCommand, Result<RegisterResponseDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<RegisterHandler> _logger;

    public RegisterHandler(UserManager<ApplicationUser> userManager, ILogger<RegisterHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<RegisterResponseDto>> HandleAsync(RegisterCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Processing registration request for email: {Email}", command.Email);

            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(command.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration attempt for existing email: {Email}", command.Email);
                return Result<RegisterResponseDto>.Failure("User with this email already exists");
            }

            var user = new ApplicationUser
            {
                UserName = command.Email,
                Email = command.Email,
                FullName = command.FullName,
                EmailConfirmed = true, // Set to false if email confirmation is required
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogWarning("User registration failed for email: {Email}. Errors: {Errors}",
                    command.Email, string.Join(", ", errors));
                return Result<RegisterResponseDto>.Failure(errors);
            }

            var response = new RegisterResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                FullName = user.FullName!,
                RequiresEmailConfirmation = false // Set based on your configuration
            };

            _logger.LogInformation("User registered successfully: {Email}", command.Email);
            return Result<RegisterResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during registration for email: {Email}", command.Email);
            return Result<RegisterResponseDto>.Failure("An error occurred during registration");
        }
    }
}
