using Thinka.API.Services;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.API.DependencyInjection;

public static class ChatDependencyInjection
{
    public static IServiceCollection AddChat(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddScoped<IChatHubService, ChatHubService>();
        
        return services;
    }
}