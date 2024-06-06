namespace ContentManagementService.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                var response = context.Response;
                if (response.HasStarted)
                {
                    return;
                }

                switch (response.StatusCode)
                {
                    case 404:
                        await WriteJsonResponseAsync(response, new { message = "Not Found" });
                        break;
                    case 403:
                        await WriteJsonResponseAsync(response, new
                        {
                            error = "insufficient_permissions",
                            error_description = "Insufficient permissions to access resource",
                            message = "Permission denied"
                        });
                        break;
                    case 401:
                        await WriteJsonResponseAsync(response, new
                        {
                            message = context.Request.Headers.ContainsKey("Authorization")
                                ? "Bad credentials"
                                : "Requires authentication"
                        });
                        break;
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task WriteJsonResponseAsync(HttpResponse response, object content)
        {
            response.ContentType = "application/json";
            await response.WriteAsJsonAsync(content);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            context.Response.StatusCode = 500;
            await WriteJsonResponseAsync(context.Response, new { message = "An unexpected error occurred. Please try again later." });
        }
    }

    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}

