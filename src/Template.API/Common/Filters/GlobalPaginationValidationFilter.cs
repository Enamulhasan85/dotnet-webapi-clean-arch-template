using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Template.API.Models.Common;

namespace Template.API.Common.Filters
{
    /// <summary>
    /// Global filter that automatically validates pagination parameters for any action
    /// that has 'pageNumber' and 'pageSize' parameters
    /// </summary>
    public class GlobalPaginationValidationFilter : IActionFilter
    {
        private readonly int _maxPageSize;

        public GlobalPaginationValidationFilter(int maxPageSize = 100)
        {
            _maxPageSize = maxPageSize;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Skip validation for non-controller actions
            if (context.ActionDescriptor is not Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor actionDescriptor)
                return;

            // Check if action has pagination parameters
            var parameters = actionDescriptor.MethodInfo.GetParameters();
            var hasPageNumber = parameters.Any(p => string.Equals(p.Name, "pageNumber", StringComparison.OrdinalIgnoreCase));
            var hasPageSize = parameters.Any(p => string.Equals(p.Name, "pageSize", StringComparison.OrdinalIgnoreCase));

            if (!hasPageNumber || !hasPageSize)
                return;

            // Get parameter values
            var pageNumber = GetParameterValue(context.ActionArguments, "pageNumber");
            var pageSize = GetParameterValue(context.ActionArguments, "pageSize");

            // Validate pageNumber
            if (pageNumber is not int pn || pn < 1)
            {
                context.Result = new BadRequestObjectResult(new ApiResponse("Page number must be greater than 0"));
                return;
            }

            // Validate pageSize
            if (pageSize is not int ps || ps < 1 || ps > _maxPageSize)
            {
                context.Result = new BadRequestObjectResult(new ApiResponse($"Page size must be between 1 and {_maxPageSize}"));
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed after execution
        }

        private static object? GetParameterValue(IDictionary<string, object?> arguments, string parameterName)
        {
            return arguments.FirstOrDefault(kvp =>
                string.Equals(kvp.Key, parameterName, StringComparison.OrdinalIgnoreCase)).Value;
        }
    }
}
