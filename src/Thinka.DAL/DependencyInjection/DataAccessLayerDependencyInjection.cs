using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Thinka.DAL.DependencyInjection;

public static class DataAccessLayerDependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ThinkaDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
