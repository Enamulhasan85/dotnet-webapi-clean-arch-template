namespace Template.API.Models.Common
{
    /// <summary>
    /// Represents a validation error response
    /// </summary>
    public class ValidationErrorResponse
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public object? AttemptedValue { get; set; }

        public ValidationErrorResponse() { }

        public ValidationErrorResponse(string field, string message, object? attemptedValue = null)
        {
            Field = field;
            Message = message;
            AttemptedValue = attemptedValue;
        }
    }

    /// <summary>
    /// Collection of validation errors
    /// </summary>
    public class ValidationErrorsResponse
    {
        public List<ValidationErrorResponse> Errors { get; set; } = new();
        public string Message { get; set; } = "Validation failed";

        public ValidationErrorsResponse() { }

        public ValidationErrorsResponse(List<ValidationErrorResponse> errors, string? message = null)
        {
            Errors = errors;
            if (!string.IsNullOrEmpty(message))
                Message = message;
        }
    }
}
