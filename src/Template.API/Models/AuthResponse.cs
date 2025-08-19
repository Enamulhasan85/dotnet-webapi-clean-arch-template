using System.ComponentModel.DataAnnotations;

namespace Template.API.Models
{
    /// <summary>
    /// Legacy auth response (kept for backward compatibility)
    /// </summary>
    [Obsolete("Use LoginResponse instead")]
    public class AuthResponse
    {
        public string? Token { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    /// <summary>
    /// Response model for user profile information
    /// </summary>
    public class UserProfileResponse
    {
        /// <summary>
        /// User's unique identifier
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// User's email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's full name
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Whether email is confirmed
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Account creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last login timestamp
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Whether account is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// User's roles
        /// </summary>
        public List<string> Roles { get; set; } = new();
    }

    /// <summary>
    /// Request model for token refresh
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// Expired access token
        /// </summary>
        [Required(ErrorMessage = "Access token is required")]
        public required string AccessToken { get; set; }

        /// <summary>
        /// Valid refresh token
        /// </summary>
        [Required(ErrorMessage = "Refresh token is required")]
        public required string RefreshToken { get; set; }
    }

    /// <summary>
    /// Response model for token refresh
    /// </summary>
    public class RefreshTokenResponse
    {
        /// <summary>
        /// New JWT access token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Token type (usually "Bearer")
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Token expiration time in seconds
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Token expiration timestamp
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// New refresh token (optional)
        /// </summary>
        public string? RefreshToken { get; set; }
    }

    /// <summary>
    /// Request model for password change
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Current password
        /// </summary>
        [Required(ErrorMessage = "Current password is required")]
        public required string CurrentPassword { get; set; }

        /// <summary>
        /// New password
        /// </summary>
        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        public required string NewPassword { get; set; }

        /// <summary>
        /// Confirm new password
        /// </summary>
        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match")]
        public required string ConfirmNewPassword { get; set; }
    }

    /// <summary>
    /// Request model for forgot password
    /// </summary>
    public class ForgotPasswordRequest
    {
        /// <summary>
        /// User's email address
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }
    }

    /// <summary>
    /// Request model for password reset
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        /// User's email address
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        /// <summary>
        /// Password reset token
        /// </summary>
        [Required(ErrorMessage = "Reset token is required")]
        public required string Token { get; set; }

        /// <summary>
        /// New password
        /// </summary>
        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        public required string NewPassword { get; set; }

        /// <summary>
        /// Confirm new password
        /// </summary>
        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match")]
        public required string ConfirmNewPassword { get; set; }
    }
}
