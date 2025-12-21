using Thinka.Domain.Options;

namespace Thinka.API.DependencyInjection;

public static class OptionsDependencyInjection
{
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        
        return services;
    }
}
