using System.Net;
using System.Text.Json;

namespace EnergyMixApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occured: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, title, detail) = exception switch
        {
            HttpRequestException => (
                HttpStatusCode.BadGateway,
                "External API Error",
                "Failed to fetch data from external API"
            ),
            ArgumentException => (
                HttpStatusCode.BadRequest,
                "Bad Request",
                exception.Message
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred"
            )
        };

        var problemDetails = new
        {
            type = $"https://tools.ietf.org/html/rfc7231#section-{GetRfcSection(statusCode)}",
            title,
            status = (int)statusCode,
            detail
        };

        var json = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(json);
    }

    private static string GetRfcSection(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.BadRequest => "6.5.1",
            HttpStatusCode.BadGateway => "6.6.3",
            HttpStatusCode.InternalServerError => "6.6.1",
            _ => "6.6.1"
        };
    }
}

