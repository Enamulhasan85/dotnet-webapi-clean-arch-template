using Microsoft.AspNetCore.Mvc;
using Template.API.Models;
using Template.API.Models.Common;

namespace Template.API.Controllers.Common
{
    /// <summary>
    /// Base controller with common functionality for all entity controllers
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Creates a standardized success response
        /// </summary>
        protected IActionResult SuccessResponse<T>(T data, string message = "Success")
        {
            return Ok(new ApiResponse<T>(data, message));
        }

        /// <summary>
        /// Creates a standardized success response without data
        /// </summary>
        protected IActionResult SuccessResponse(string message = "Success")
        {
            return Ok(new ApiResponse(message));
        }

        /// <summary>
        /// Creates a standardized not found response
        /// </summary>
        protected IActionResult NotFoundResponse(string message = "Resource not found")
        {
            return NotFound(new ApiResponse(message));
        }

        /// <summary>
        /// Creates a standardized bad request response
        /// </summary>
        protected IActionResult BadRequestResponse(string message = "Bad request")
        {
            return BadRequest(new ApiResponse(message));
        }

        /// <summary>
        /// Creates a standardized bad request response with validation errors
        /// </summary>
        protected IActionResult BadRequestResponse(IEnumerable<string> errors)
        {
            return BadRequest(new ApiResponse(errors.ToList()));
        }

        /// <summary>
        /// Creates a standardized internal server error response
        /// </summary>
        protected IActionResult InternalServerErrorResponse(string message = "An internal server error occurred")
        {
            return StatusCode(500, new ApiResponse(message));
        }

        /// <summary>
        /// Creates a standardized created response for resource creation
        /// </summary>
        protected IActionResult CreatedResponse<T>(T data, string message = "Resource created successfully")
        {
            return StatusCode(201, new ApiResponse<T>(data, message));
        }

        /// <summary>
        /// Creates a standardized no content response for successful operations without data
        /// </summary>
        protected IActionResult NoContentResponse()
        {
            return NoContent();
        }

        /// <summary>
        /// Handles entity not found scenarios consistently
        /// </summary>
        protected IActionResult HandleEntityNotFound(string entityName, object id)
        {
            return NotFoundResponse($"{entityName} with ID {id} not found");
        }

        /// <summary>
        /// Handles successful entity retrieval
        /// </summary>
        protected IActionResult HandleEntityFound<T>(T entity, string entityName)
        {
            return SuccessResponse(entity);
        }

        /// <summary>
        /// Handles successful entity creation
        /// </summary>
        protected IActionResult HandleEntityCreated<T>(T entity, string entityName)
        {
            return CreatedResponse(entity, $"{entityName} created successfully");
        }

        /// <summary>
        /// Handles successful entity update
        /// </summary>
        protected IActionResult HandleEntityUpdated<T>(T entity, string entityName)
        {
            return SuccessResponse(entity, $"{entityName} updated successfully");
        }

        /// <summary>
        /// Handles successful entity deletion
        /// </summary>
        protected IActionResult HandleEntityDeleted(string entityName)
        {
            return SuccessResponse($"{entityName} deleted successfully");
        }
    }
}
