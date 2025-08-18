using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Template.API.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError(ILogger<ErrorController> logger)
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            
            logger.LogError(exception, "An unhandled exception occurred");
            
            return Problem(
                title: "An error occurred while processing your request.",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}