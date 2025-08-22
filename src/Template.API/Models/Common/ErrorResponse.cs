namespace Template.API.Models.Common
{
    /// <summary>
    /// Represents an error response for API endpoints
    /// </summary>
    public class ErrorResponse
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Detail { get; set; } = string.Empty;
        public string? Instance { get; set; }
        public string TraceId { get; set; } = string.Empty;
        public Dictionary<string, object>? Extensions { get; set; }

        public ErrorResponse() { }

        public ErrorResponse(string type, string title, int status, string detail, string? instance = null)
        {
            Type = type;
            Title = title;
            Status = status;
            Detail = detail;
            Instance = instance;
        }

        /// <summary>
        /// Creates a standard validation error response
        /// </summary>
        public static ErrorResponse ValidationError(string detail, Dictionary<string, string[]>? errors = null)
        {
            var response = new ErrorResponse("https://tools.ietf.org/html/rfc7231#section-6.5.1", "Validation Error", 400, detail);
            
            if (errors != null && errors.Any())
            {
                response.Extensions = new Dictionary<string, object> { { "errors", errors } };
            }

            return response;
        }

        /// <summary>
        /// Creates a standard not found error response
        /// </summary>
        public static ErrorResponse NotFound(string detail)
        {
            return new ErrorResponse("https://tools.ietf.org/html/rfc7231#section-6.5.4", "Not Found", 404, detail);
        }

        /// <summary>
        /// Creates a standard internal server error response
        /// </summary>
        public static ErrorResponse InternalServerError(string detail)
        {
            return new ErrorResponse("https://tools.ietf.org/html/rfc7231#section-6.6.1", "Internal Server Error", 500, detail);
        }

        /// <summary>
        /// Creates a standard unauthorized error response
        /// </summary>
        public static ErrorResponse Unauthorized(string detail = "Unauthorized access")
        {
            return new ErrorResponse("https://tools.ietf.org/html/rfc7235#section-3.1", "Unauthorized", 401, detail);
        }

        /// <summary>
        /// Creates a standard forbidden error response
        /// </summary>
        public static ErrorResponse Forbidden(string detail = "Access forbidden")
        {
            return new ErrorResponse("https://tools.ietf.org/html/rfc7231#section-6.5.3", "Forbidden", 403, detail);
        }
    }
}
