using System.ComponentModel.DataAnnotations;

namespace Template.API.Models
{
    /// <summary>
    /// Request model for user authentication
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User's email address
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public required string Email { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        public required string Password { get; set; }

        /// <summary>
        /// Whether to remember the user (for extended session)
        /// </summary>
        public bool RememberMe { get; set; } = false;
    }

    /// <summary>
    /// Response model for successful authentication
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// JWT access token
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
        /// Refresh token for getting new access tokens
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// User information
        /// </summary>
        public UserInfo User { get; set; } = new();
    }

    /// <summary>
    /// User information included in login response
    /// </summary>
    public class UserInfo
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
        /// User's roles
        /// </summary>
        public List<string> Roles { get; set; } = new();
    }
}
