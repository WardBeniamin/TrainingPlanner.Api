using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrainingPlanner.Api.Errors;

namespace TrainingPlanner.Api.Controllers
{
    // Den här controllern används bara av global error handling
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // gömmer den i Swagger-listan
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        // Denna route anropas automatiskt av UseExceptionHandler("/error")
        [Route("/error")]
        public IActionResult HandleError()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionFeature?.Error;

            // Logga felet (om det finns något)
            if (exception != null)
            {
                _logger.LogError(exception, "Unhandled exception occurred");
            }
            else
            {
                _logger.LogError("Unhandled error occurred, but no exception was found in the context.");
            }

            var apiError = new ApiError
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "An unexpected error occurred. Please try again later."
            };

            return StatusCode(apiError.StatusCode, apiError);
        }
    }
}
