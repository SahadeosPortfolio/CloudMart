using Microsoft.AspNetCore.Mvc;
using Products.Application.Exceptions;

namespace Products.Api.Middlewares;

public static class ExceptionHandlerHelper
{
    public static (ProblemDetails, int) MapToProblemDetails(Exception ex, HttpContext context)
    {
        var problem = new ProblemDetails
        {
            Title = "An error occurred while processing your request.",
            Detail = ex.Message,
            Instance = context.Request.Path,
            Type = "https://httpstatuses.org/500"
        };
        int statusCode;

        switch (ex)
        {
            case ValidationException validationException:
                problem.Title = "Validation Error";
                problem.Detail = "One or more validation failures occurred.";
                problem.Extensions["errors"] = validationException.Errors;
                statusCode = StatusCodes.Status422UnprocessableEntity;
                break;

            case KeyNotFoundException keyNotFoundException:
                problem.Title = "Resource Not Found";
                problem.Detail = keyNotFoundException.Message;
                problem.Type = "https://httpstatuses.org/404";
                statusCode = StatusCodes.Status404NotFound;
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        return (problem, statusCode);
    }
}