using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Template.API.Common.Extensions;

namespace Template.API.Common.Filters
{
    /// <summary>
    /// Global filter to automatically handle model validation
    /// Eliminates need for manual ModelState.IsValid checks
    /// </summary>
    public class GlobalValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState.ToApiResponse());
            }

            base.OnActionExecuting(context);
        }
    }
}
