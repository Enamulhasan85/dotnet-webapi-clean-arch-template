using Microsoft.AspNetCore.Mvc.ModelBinding;
using Template.API.Models.Common;

namespace Template.API.Common.Extensions
{
    /// <summary>
    /// Extension methods for ModelState
    /// </summary>
    public static class ModelStateExtensions
    {
        /// <summary>
        /// Converts ModelState errors to a simple list of error messages
        /// </summary>
        public static List<string> ToErrorMessages(this ModelStateDictionary modelState)
        {
            return modelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                .ToList();
        }


        /// <summary>
        /// Gets all error messages from ModelState as a list of strings
        /// </summary>
        /// <param name="modelState">The ModelState dictionary</param>
        /// <returns>List of error messages</returns>
        public static List<string> GetErrorMessages(this ModelStateDictionary modelState)
        {
            return modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
        }

        /// <summary>
        /// Checks if ModelState has any validation errors
        /// </summary>
        /// <param name="modelState">The ModelState dictionary</param>
        /// <returns>True if there are validation errors, false otherwise</returns>
        public static bool HasValidationErrors(this ModelStateDictionary modelState)
        {
            return !modelState.IsValid;
        }

        /// <summary>
        /// Creates an ApiResponse with validation errors from ModelState
        /// </summary>
        public static ApiResponse ToApiResponse(this ModelStateDictionary modelState, string? message = null)
        {
            var errors = modelState.ToErrorMessages();
            return new ApiResponse(errors);
        }

        /// <summary>
        /// Checks if ModelState has errors for specific fields
        /// </summary>
        public static bool HasErrorsFor(this ModelStateDictionary modelState, params string[] fieldNames)
        {
            return fieldNames.Any(field => modelState.ContainsKey(field) &&
                                          modelState[field]?.Errors.Count > 0);
        }
    }
}
