using Microsoft.AspNetCore.Mvc; 
using NotesApi.Exceptions;      

namespace NotesApi.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
        catch(ValidationException ex)
        {
            context.Response.StatusCode = 400;

            var problem = new ProblemDetails
            {
                Status = 400,
                Title = "Bad Request",
                Detail = ex.Message
            };

            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = 404;

            var problem = new ProblemDetails
            {
                Status = 404,
                Title = "Not Found",
                Detail = ex.Message
            };

            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = 500;

            var problem = new ProblemDetails
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = "Internal server error"
            };

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}