using System.Net;
using System.Text.Json;

namespace Clientes.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex, "Erro inesperado");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;

        switch (exception)
        {
            case ArgumentException:
                status = HttpStatusCode.BadRequest;
                break;

            case InvalidOperationException:
                status = HttpStatusCode.BadRequest;
                break;

            case KeyNotFoundException:
                status = HttpStatusCode.NotFound;
                break;

            default:
                status = HttpStatusCode.InternalServerError;
                break;
        }

        var response = new
        {
            message = exception.Message,
            status = (int)status
        };

        var payload = JsonSerializer.Serialize(response);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        return context.Response.WriteAsync(payload);
    }
}