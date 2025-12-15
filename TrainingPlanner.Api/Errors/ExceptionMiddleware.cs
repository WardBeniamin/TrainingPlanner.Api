using System.Text.Json;

namespace TrainingPlanner.Api.Errors
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // 1) Default: okänt fel = 500
                var statusCode = 500;
                var message = "Something went wrong. Please try again later.";

                // 2) Om det är ett "förväntat" fel från oss (ApiException)
                if (ex is ApiException apiEx)
                {
                    statusCode = apiEx.StatusCode;
                    message = apiEx.Message;

                    _logger.LogWarning(ex,
                        "Handled API exception. StatusCode: {StatusCode}. TraceId: {TraceId}",
                        statusCode,
                        context.TraceIdentifier);
                }
                else
                {
                    // Okänt fel -> loggas som error
                    _logger.LogError(ex,
                        "Unhandled exception. TraceId: {TraceId}",
                        context.TraceIdentifier);
                }

                var apiError = new ApiError
                {
                    StatusCode = statusCode,
                    Message = message,
                    TraceId = context.TraceIdentifier,
                    Details = _env.IsDevelopment() ? ex.ToString() : null
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var json = JsonSerializer.Serialize(apiError, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
        }
    }
}
