using CadastroFamilias.Core.Abstractions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CadastroFamilias.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Log.Error(exception, "Ocorreu uma exceção não tratada: {Message}", exception.Message);

        var error = exception switch
        {
            ArgumentNullException => Error.NullValue,
            KeyNotFoundException => Error.NotFound,
            FluentValidation.ValidationException validationException => new Error(
                Error.ValidationFailed.Code,
                string.Join(" | ", validationException.Errors.Select(e => e.ErrorMessage))
            ),
            _ => Error.InternalServer
        };

        var problemDetails = new ProblemDetails
        {
            Status = GetStatusCode(exception),
            Title = error.Message,
            Type = exception.GetType().Name,
            Extensions =
            {
                { "code", error.Code },
                { "traceId", httpContext.TraceIdentifier }
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            ArgumentNullException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            InvalidOperationException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
}