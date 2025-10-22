using Domain.Exceptions;

using Infrastructure.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Shared.Exceptions
{
    public static class ExceptionHandler
    {
        public static async Task HandleAsync(HttpContext context, Exception ex, ILogger logger)
        {
            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case DomainException domainEx:
                    logger.LogWarning(domainEx, "Domain exception occurred");
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = domainEx.Message });
                    break;

                case InfrastructureException infraEx:
                    logger.LogError(infraEx, "Infrastructure exception");
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    await context.Response.WriteAsJsonAsync(new { error = "Service unavailable." });
                    break;

                default:
                    logger.LogCritical(ex, "Unhandled exception");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(new { error = "Internal server error." });
                    break;
            }
        }
    }
}

