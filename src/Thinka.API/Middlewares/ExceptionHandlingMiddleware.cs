using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Thinka.Domain.Exceptions;

namespace Thinka.API.Middlewares;

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
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning(exception, "The response has already started, the exception middleware will not handle the error.");
                throw;
            }

            _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            FormatException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int)HttpStatusCode.NotFound,
            ConflictException => (int)HttpStatusCode.Conflict,
            UnauthorizedException => (int)HttpStatusCode.Unauthorized,
            ForbiddenException => (int)HttpStatusCode.Forbidden,
            SecurityTokenException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = statusCode == (int)HttpStatusCode.InternalServerError ? "An unexpected error occurred." : exception.Message,
            Instance = context.Request.Path
        };
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        var problemDetailsService = context.RequestServices.GetRequiredService<IProblemDetailsService>();
        var wasHandled = await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            ProblemDetails = problemDetails,
            Exception = exception
        });

        if (!wasHandled)
        {
            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
        }
    }

    private static string GetTitle(int statusCode) =>
        statusCode switch
        {
            (int)HttpStatusCode.NotFound => "Resource Not Found",
            (int)HttpStatusCode.Conflict => "Conflict",
            (int)HttpStatusCode.BadRequest => "Bad Request",
            (int)HttpStatusCode.Unauthorized => "Unauthorized",
            (int)HttpStatusCode.Forbidden => "Forbidden",
            _ => "An internal server error occurred."
        };
}
