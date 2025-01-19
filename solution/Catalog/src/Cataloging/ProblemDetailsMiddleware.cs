using FluentValidation;
using Microsoft.AspNetCore.Mvc;

public class ProblemDetailsMiddleware
{
    private readonly RequestDelegate _next;

    public ProblemDetailsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            var extensions = new Dictionary<string, object?>();

            var problemDetails = new ProblemDetails
            {
                Status = 400,
                Title = "One or more validation errors occurred.",
                Extensions = extensions
            };

            var errors = new Dictionary<string, object>();

            foreach (var error in ex.Errors)
            {
                var errorDetails = new
                {
                    Message = error.ErrorMessage,
                    Code = error.ErrorCode
                };
                
                errors.Add(error.PropertyName, errorDetails);
            }

            problemDetails.Extensions.Add("errors", errors);

            context.Response.StatusCode = 400;
            
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception ex)
        {
            var problemDetails = new ProblemDetails
            {
                Status = 500,
                Title = "Internal Server Error",
            };
            context.Response.StatusCode = 500;
            
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}