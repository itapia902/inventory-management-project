using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TransactionService.Api.Shared;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {        
        if (exception is not ArgumentException && exception is not InvalidOperationException)
        {
            logger.LogError(exception, "Excepción no controlada en {Method} {Path}",
                httpContext.Request.Method, httpContext.Request.Path);
        }
        else
        {
            logger.LogWarning("Regla de negocio no cumplida: {Message}", exception.Message);
        }
        
        var (status, title, detail) = exception switch
        {
            DbUpdateConcurrencyException => (
                StatusCodes.Status409Conflict,
                "Conflicto de concurrencia",
                "El registro fue modificado por otro proceso simultáneamente."),
            
            ArgumentException => (
                StatusCodes.Status400BadRequest,
                "Error de validación",
                exception.Message), 

            InvalidOperationException => (
                StatusCodes.Status400BadRequest,
                "Regla de negocio no cumplida",
                exception.Message), 

            
            _ => (
                StatusCodes.Status500InternalServerError,
                "Error interno del servidor",
                "Ha ocurrido un error inesperado, consulte con el administrador.")
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail, 
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        problem.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}