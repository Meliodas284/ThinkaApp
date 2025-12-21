using Microsoft.AspNetCore.Authentication.JwtBearer;
using Thinka.API.OptionsSetup;

namespace Thinka.API.DependencyInjection;

public static class AuthDependencyInjection
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddAuthorization();

        return services;
    }
}
