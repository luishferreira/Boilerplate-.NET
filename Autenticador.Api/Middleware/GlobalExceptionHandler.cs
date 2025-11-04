using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Autenticador.Api.Middleware;
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception,
            "Exception occurred: {Message}", exception.Message);

        var (statusCode, title, detail) = exception switch
        {
            ValidationException validationEx => (
                StatusCodes.Status400BadRequest,
                "Validation Error",
                string.Join("; ", validationEx.Errors
                    .Select(e => e.ErrorMessage))
            ),
            ArgumentException => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                exception.Message
            ),
            KeyNotFoundException => (
                StatusCodes.Status404NotFound,
                "Not Found",
                exception.Message
            ),
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                exception.Message
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                env.IsDevelopment()
                        ? $"[DEV] {exception.Message} \n {exception.StackTrace}"
                        : "An unexpected error occurred"
            )
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path,
            Type = $"https://httpstatuses.io/{statusCode}"
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(
            problemDetails, cancellationToken);

        return true;
    }
}