using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Common.Settings;
using Template.Application.Features.Authentication.DTOs;
using Template.Domain.Identity;

namespace Template.Application.Features.Authentication.Commands;

public class LoginCommand : BaseCommand<Result<LoginResponseDto>>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool RememberMe { get; set; } = false;
}

public class LoginHandler : ICommandHandler<LoginCommand, Result<LoginResponseDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IOptions<JwtSettings> jwtOptions,
        ILogger<LoginHandler> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _jwtSettings = jwtOptions.Value;
        _logger = logger;
    }

    public async Task<Result<LoginResponseDto>> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Processing login request for email: {Email}", command.Email);

            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt for non-existent email: {Email}", command.Email);
                return Result<LoginResponseDto>.Failure("Invalid email or password");
            }

            // Check if account is active
            if (!user.IsActive)
            {
                _logger.LogWarning("Login attempt for inactive account: {Email}", command.Email);
                return Result<LoginResponseDto>.Failure("Account is inactive. Please contact support.");
            }

            // Check if account is locked
            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogWarning("Login attempt for locked account: {Email}", command.Email);
                return Result<LoginResponseDto>.Failure("Account is temporarily locked. Please try again later.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Account locked out for email: {Email}", command.Email);
                    return Result<LoginResponseDto>.Failure("Account is temporarily locked due to multiple failed attempts");
                }

                _logger.LogWarning("Invalid login attempt for email: {Email}", command.Email);
                return Result<LoginResponseDto>.Failure("Invalid email or password");
            }

            // Reset access failed count on successful login
            await _userManager.ResetAccessFailedCountAsync(user);

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = await _tokenService.GenerateTokenAsync(user.Id, user.Email!, userRoles, command.RememberMe);
            
            // Calculate expiration based on RememberMe flag and JWT settings
            var expiresInMinutes = command.RememberMe ? _jwtSettings.RememberMeExpiryMinutes : _jwtSettings.ExpiryMinutes;
            var expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);
            var expiresInSeconds = (int)TimeSpan.FromMinutes(expiresInMinutes).TotalSeconds;

            var response = new LoginResponseDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = expiresInSeconds,
                ExpiresAt = expiresAt,
                RefreshToken = await _tokenService.GenerateRefreshTokenAsync(),
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName ?? string.Empty,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = userRoles.ToList()
                }
            };

            _logger.LogInformation("User logged in successfully: {Email}", command.Email);
            return Result<LoginResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during login for email: {Email}", command.Email);
            return Result<LoginResponseDto>.Failure("An error occurred during login");
        }
    }
}
