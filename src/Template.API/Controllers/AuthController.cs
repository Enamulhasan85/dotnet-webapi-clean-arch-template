using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Template.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;
using Template.Domain.Identity;
using Template.API.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.RateLimiting;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            ITokenService tokenService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="model">User registration details</param>
        /// <returns>Registration result</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<RegisterResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    _logger.LogWarning("Registration failed due to validation errors for email: {Email}", model.Email);
                    return BadRequest(new ApiResponse(errors));
                }

                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration attempt for existing email: {Email}", model.Email);
                    return Conflict(new ApiResponse("User with this email already exists"));
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    EmailConfirmed = true, // Set to false if email confirmation is required
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    _logger.LogWarning("User registration failed for email: {Email}. Errors: {Errors}", 
                        model.Email, string.Join(", ", errors));
                    return BadRequest(new ApiResponse(errors));
                }

                var response = new RegisterResponse
                {
                    UserId = user.Id,
                    Email = user.Email!,
                    FullName = user.FullName!,
                    CreatedAt = user.CreatedAt,
                    RequiresEmailConfirmation = false // Set based on your configuration
                };

                _logger.LogInformation("User registered successfully: {Email}", model.Email);
                return CreatedAtAction(nameof(GetProfile), new { }, new ApiResponse<RegisterResponse>(response, "User registered successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user registration for email: {Email}", model.Email);
                return StatusCode(500, new ApiResponse("An error occurred during registration"));
            }
        }

        /// <summary>
        /// Authenticate user and return JWT token
        /// </summary>
        /// <param name="model">User login credentials</param>
        /// <returns>Authentication result with JWT token</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    _logger.LogWarning("Login failed due to validation errors for email: {Email}", model.Email);
                    return BadRequest(new ApiResponse(errors));
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login attempt for non-existent email: {Email}", model.Email);
                    return Unauthorized(new ApiResponse("Invalid email or password"));
                }

                // Check if account is active
                if (!user.IsActive)
                {
                    _logger.LogWarning("Login attempt for inactive account: {Email}", model.Email);
                    return Unauthorized(new ApiResponse("Account is inactive. Please contact support."));
                }

                // Check if account is locked
                if (await _userManager.IsLockedOutAsync(user))
                {
                    _logger.LogWarning("Login attempt for locked account: {Email}", model.Email);
                    return Unauthorized(new ApiResponse("Account is temporarily locked. Please try again later."));
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
                
                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("Account locked out for email: {Email}", model.Email);
                        return Unauthorized(new ApiResponse("Account is temporarily locked due to multiple failed attempts"));
                    }
                    
                    _logger.LogWarning("Invalid login attempt for email: {Email}", model.Email);
                    return Unauthorized(new ApiResponse("Invalid email or password"));
                }

                // Reset access failed count on successful login
                await _userManager.ResetAccessFailedCountAsync(user);

                // Update last login time
                user.LastLoginAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                var token = await _tokenService.GenerateJwtTokenAsync(user);
                var expiresAt = DateTime.UtcNow.AddHours(24); // Adjust based on your JWT settings
                var userRoles = await _userManager.GetRolesAsync(user);
                
                var response = new LoginResponse
                {
                    AccessToken = token,
                    TokenType = "Bearer",
                    ExpiresIn = 86400, // 24 hours in seconds
                    ExpiresAt = expiresAt,
                    RefreshToken = _tokenService.GenerateRefreshToken(), // If you implement refresh tokens
                    User = new UserInfo
                    {
                        Id = user.Id,
                        Email = user.Email ?? string.Empty,
                        FullName = user.FullName ?? string.Empty,
                        EmailConfirmed = user.EmailConfirmed,
                        Roles = userRoles.ToList()
                    }
                };

                _logger.LogInformation("User logged in successfully: {Email}", model.Email);
                return Ok(new ApiResponse<LoginResponse>(response, "Login successful"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for email: {Email}", model.Email);
                return StatusCode(500, new ApiResponse("An error occurred during login"));
            }
        }

        /// <summary>
        /// Get current authenticated user's profile
        /// </summary>
        /// <returns>User profile information</returns>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(ApiResponse<UserProfileResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Failed to extract user ID from token");
                    return Unauthorized(new ApiResponse("Invalid token"));
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found for ID: {UserId}", userId);
                    return NotFound(new ApiResponse("User not found"));
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var response = new UserProfileResponse
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName ?? string.Empty,
                    EmailConfirmed = user.EmailConfirmed,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    IsActive = user.IsActive,
                    Roles = userRoles.ToList()
                };

                return Ok(new ApiResponse<UserProfileResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user profile");
                return StatusCode(500, new ApiResponse("An error occurred while retrieving profile"));
            }
        }

        /// <summary>
        /// Logout user (if using server-side session management)
        /// </summary>
        /// <returns>Logout confirmation</returns>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation("User logged out successfully: {UserId}", userId);
                
                return Ok(new ApiResponse("Logged out successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout");
                return StatusCode(500, new ApiResponse("An error occurred during logout"));
            }
        }

        /// <summary>
        /// Refresh JWT token using refresh token
        /// </summary>
        /// <param name="model">Token refresh request</param>
        /// <returns>New access token</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResponse<RefreshTokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(new ApiResponse(errors));
                }

                // Validate the refresh token (implement your refresh token validation logic)
                var principal = _tokenService.GetPrincipalFromExpiredToken(model.AccessToken);
                if (principal == null)
                {
                    return Unauthorized(new ApiResponse("Invalid access token"));
                }

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse("Invalid token"));
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || !user.IsActive)
                {
                    return Unauthorized(new ApiResponse("User not found or inactive"));
                }

                // TODO: Validate refresh token against stored refresh tokens
                // For now, we'll generate new tokens

                var newAccessToken = await _tokenService.GenerateJwtTokenAsync(user);
                var expiresAt = DateTime.UtcNow.AddHours(24);

                var response = new RefreshTokenResponse
                {
                    AccessToken = newAccessToken,
                    TokenType = "Bearer",
                    ExpiresIn = 86400, // 24 hours in seconds
                    ExpiresAt = expiresAt,
                    RefreshToken = _tokenService.GenerateRefreshToken() // Optionally issue new refresh token
                };

                _logger.LogInformation("Token refreshed successfully for user: {UserId}", userId);
                return Ok(new ApiResponse<RefreshTokenResponse>(response, "Token refreshed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during token refresh");
                return StatusCode(500, new ApiResponse("An error occurred during token refresh"));
            }
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="model">Password change request</param>
        /// <returns>Password change confirmation</returns>
        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(new ApiResponse(errors));
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse("Invalid token"));
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Unauthorized(new ApiResponse("User not found"));
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    _logger.LogWarning("Password change failed for user: {UserId}. Errors: {Errors}", 
                        userId, string.Join(", ", errors));
                    return BadRequest(new ApiResponse(errors));
                }

                _logger.LogInformation("Password changed successfully for user: {UserId}", userId);
                return Ok(new ApiResponse("Password changed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during password change");
                return StatusCode(500, new ApiResponse("An error occurred during password change"));
            }
        }

        /// <summary>
        /// Initiate forgot password process
        /// </summary>
        /// <param name="model">Forgot password request</param>
        /// <returns>Forgot password confirmation</returns>
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(new ApiResponse(errors));
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    _logger.LogWarning("Password reset requested for non-existent email: {Email}", model.Email);
                    return Ok(new ApiResponse("If the email exists, a password reset link has been sent"));
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                
                // TODO: Send email with reset token
                // For now, just log it (remove in production)
                _logger.LogInformation("Password reset token generated for user: {Email}. Token: {Token}", 
                    model.Email, token);

                return Ok(new ApiResponse("If the email exists, a password reset link has been sent"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during forgot password process");
                return StatusCode(500, new ApiResponse("An error occurred during forgot password process"));
            }
        }

        /// <summary>
        /// Reset password using reset token
        /// </summary>
        /// <param name="model">Password reset request</param>
        /// <returns>Password reset confirmation</returns>
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(new ApiResponse(errors));
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return BadRequest(new ApiResponse("Invalid reset token"));
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    _logger.LogWarning("Password reset failed for user: {Email}. Errors: {Errors}", 
                        model.Email, string.Join(", ", errors));
                    return BadRequest(new ApiResponse(errors));
                }

                _logger.LogInformation("Password reset successfully for user: {Email}", model.Email);
                return Ok(new ApiResponse("Password reset successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during password reset");
                return StatusCode(500, new ApiResponse("An error occurred during password reset"));
            }
        }
    }
}
