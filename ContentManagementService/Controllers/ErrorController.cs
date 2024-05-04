using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ContentManagementService.API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleErrorDevelopment(
            [FromServices] IHostEnvironment hostEnvironment)
        {
            var exceptionHandlerFeature =
                HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            return Problem(
                detail: exceptionHandlerFeature.Error.StackTrace,
                title: exceptionHandlerFeature.Error.Message,
                type: null);
        }
    }
}
