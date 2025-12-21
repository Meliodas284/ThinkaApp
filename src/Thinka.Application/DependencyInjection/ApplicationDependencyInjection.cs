using Microsoft.Extensions.DependencyInjection;
using Thinka.Application.Services;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
