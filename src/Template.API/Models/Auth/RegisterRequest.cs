using System.ComponentModel.DataAnnotations;

namespace Template.API.Models
{
    /// <summary>
    /// Request model for user registration
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// User's full name
        /// </summary>
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s\-'\.]+$", ErrorMessage = "Full name can only contain letters, spaces, hyphens, apostrophes, and periods")]
        public required string FullName { get; set; }

        /// <summary>
        /// User's email address (will be used as username)
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public required string Email { get; set; }

        /// <summary>
        /// User's password (must meet complexity requirements)
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        public required string Password { get; set; }

        /// <summary>
        /// Confirm password (must match password)
        /// </summary>
        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        public required string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Response model for successful user registration
    /// </summary>
    public class RegisterResponse
    {
        /// <summary>
        /// User's unique identifier
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// User's email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's full name
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Registration timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Whether email confirmation is required
        /// </summary>
        public bool RequiresEmailConfirmation { get; set; }
    }
}
