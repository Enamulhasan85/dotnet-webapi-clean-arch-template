using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Template.API.Controllers.Common;

namespace Template.API.Controllers
{
    [ApiController]
    public class ErrorController : BaseController
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