using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Common.API;

public class ProblemDetailsMiddleware
{
    private readonly RequestDelegate _next;

    public ProblemDetailsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", MessageId = "All exceptions need to be caught and handled.")]
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
        catch (Exception)
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