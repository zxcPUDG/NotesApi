using Microsoft.AspNetCore.Mvc; 
using NotesApi.Exceptions;      
using FluentValidation;
using NotesApi.Dtos;

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
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation failed: {Message}",ex.Message);

            context.Response.StatusCode = 400;

            Dictionary<string, string[]> errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            ValidationProblemDetails problem = new()
            {
                Status = 400,
                Title = "Validation failed",
                Detail = ex.Message,
                Errors = errors
            };
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch(BadRequestException ex)
        {   
            _logger.LogWarning("Bad Request: {Message}",ex.Message);

            await WriteProblemAsync(context,400,"Bad Request",ex.Message);

        }
        catch (NotFoundException ex)
        {   
            _logger.LogWarning("Not found: {Message}", ex.Message);

             await WriteProblemAsync(context,404,"Not Found",ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            await WriteProblemAsync(context,500,"Internal Server Error","Internal Server Error");
        }
    }

    private async Task WriteProblemAsync(HttpContext context, int status, string title, string detail)
    {       
            
            context.Response.StatusCode = status;

            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail
            };

            await context.Response.WriteAsJsonAsync(problem);
    }
}