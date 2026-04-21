using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Thinka.DAL.Interceptors;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.DAL.Repositories;
using Thinka.Domain.Interfaces.SearchProviders;
using Thinka.DAL.SearchProviders;

namespace Thinka.DAL.DependencyInjection;

public static class DataAccessLayerDependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddSingleton<AuditableEntityInterceptor>();
        
        services.AddDbContext<ThinkaDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            options.UseNpgsql(connectionString).AddInterceptors(interceptor);
        });
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IIdeaRepository, IdeaRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ISaveRepository, SaveRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IIdeaSearchProvider, IdeaSearchProvider>();

        return services;
    }
}
