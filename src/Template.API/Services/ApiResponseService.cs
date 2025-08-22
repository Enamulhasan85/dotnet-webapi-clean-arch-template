using Template.API.Models.Common;

namespace Template.API.Services
{
    /// <summary>
    /// Service for creating standardized API responses
    /// </summary>
    public interface IApiResponseService
    {
        ApiResponse<T> Success<T>(T data, string? message = null);
        ApiResponse Success(string message = "Operation completed successfully");
        ApiResponse Error(string message);
        ApiResponse Error(List<string> errors);
        ApiResponse<T> Error<T>(string message);
        PaginatedResponse<T> Paginated<T>(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize);
    }

    /// <summary>
    /// Implementation of API response service
    /// </summary>
    public class ApiResponseService : IApiResponseService
    {
        public ApiResponse<T> Success<T>(T data, string? message = null)
        {
            return new ApiResponse<T>(data, message);
        }

        public ApiResponse Success(string message = "Operation completed successfully")
        {
            return new ApiResponse(message);
        }

        public ApiResponse Error(string message)
        {
            return new ApiResponse(message);
        }

        public ApiResponse Error(List<string> errors)
        {
            return new ApiResponse(errors);
        }

        public ApiResponse<T> Error<T>(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Errors = new List<string> { message }
            };
        }

        public PaginatedResponse<T> Paginated<T>(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            return new PaginatedResponse<T>(items, totalCount, pageNumber, pageSize);
        }
    }
}
