using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Template.API.Models.Common;
using Template.Application.Common.Exceptions;

namespace Template.API.Common.Filters
{
    /// <summary>
    /// Global exception filter for handling application exceptions
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An exception occurred: {Message}", context.Exception.Message);

            var response = context.Exception switch
            {
                NotFoundException notFoundEx => new ApiResponse(notFoundEx.Message)
                {
                    Success = false,
                    Errors = new List<string> { notFoundEx.Message }
                },
                ValidationException validationEx => new ApiResponse(validationEx.Errors.Select(e => e).ToList()),
                ForbiddenAccessException forbiddenEx => new ApiResponse(forbiddenEx.Message)
                {
                    Success = false,
                    Errors = new List<string> { forbiddenEx.Message }
                },
                _ => new ApiResponse("An error occurred while processing your request.")
                {
                    Success = false,
                    Errors = new List<string> { "Internal server error" }
                }
            };

            var statusCode = context.Exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                ForbiddenAccessException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
