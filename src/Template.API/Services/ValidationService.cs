using System.Text.RegularExpressions;

namespace Template.API.Services
{
    public interface IValidationService
    {
        bool IsValidEmail(string email);
        bool IsStrongPassword(string password);
        bool IsSafeName(string name);
    }

    public class ValidationService : IValidationService
    {
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(ILogger<ValidationService> logger)
        {
            _logger = logger;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.IgnoreCase);
                return emailRegex.IsMatch(email) && email.Length <= 255;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating email: {Email}", email);
                return false;
            }
        }

        public bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // At least 8 characters, 1 uppercase, 1 lowercase, 1 digit, 1 special character
            var strongPasswordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return strongPasswordRegex.IsMatch(password) && password.Length <= 100;
        }

        public bool IsSafeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            // Allow letters, spaces, hyphens, apostrophes, but prevent XSS and SQL injection attempts
            var safeNameRegex = new Regex(@"^[a-zA-Z\s\-'\.]{2,100}$");
            return safeNameRegex.IsMatch(name) && 
                   !name.Contains("<script", StringComparison.OrdinalIgnoreCase) &&
                   !name.Contains("javascript:", StringComparison.OrdinalIgnoreCase) &&
                   !name.Contains("'", StringComparison.OrdinalIgnoreCase) &&
                   !name.Contains("\"", StringComparison.OrdinalIgnoreCase);
        }
    }
}
