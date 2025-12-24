using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.DAL.Repositories;

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
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IIdeaRepository, IdeaRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();

        return services;
    }
}
