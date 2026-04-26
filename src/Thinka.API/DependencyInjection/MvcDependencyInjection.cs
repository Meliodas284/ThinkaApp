using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Thinka.API.DependencyInjection;

public static class MvcDependencyInjection
{
    public static IServiceCollection AddApiLayer(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddProblemDetails();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                    context.HttpContext,
                    context.ModelState,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Validation Failed",
                    detail: "One or more validation errors occurred.");

                problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
            };
        });

        return services;
    }
}
