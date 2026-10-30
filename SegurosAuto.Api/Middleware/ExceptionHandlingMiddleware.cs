using Microsoft.AspNetCore.Mvc;
using SegurosAuto.Api.Exceptions;

namespace SegurosAuto.Api.Middleware;

/// <summary>
/// Convierte las excepciones del dominio en respuestas HTTP semánticas
/// (formato ProblemDetails), así los controllers quedan limpios sin try/catch.
/// <list type="bullet">
///   <item><see cref="NotFoundException"/> → 404</item>
///   <item><see cref="BusinessException"/> → 400</item>
///   <item>Cualquier otra → 500 (se registra en el log)</item>
/// </list>
/// </summary>
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
        catch (NotFoundException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status404NotFound, "Recurso no encontrado", ex.Message);
        }
        catch (BusinessException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Solicitud inválida", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado");
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Error interno", "Ocurrió un error inesperado.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int status, string title, string detail)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        var problem = new ProblemDetails { Status = status, Title = title, Detail = detail };
        await context.Response.WriteAsJsonAsync(problem);
    }
}