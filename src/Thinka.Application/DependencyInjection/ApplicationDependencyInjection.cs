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
        services.AddScoped<IIdeaService, IdeaService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ISaveService, SaveService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFollowService, FollowService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IIdeasSearchService, IdeasSearchService>();
        services.AddScoped<IFeedService, FeedService>();
        services.AddScoped<IConversationService, ConversationService>();
        services.AddScoped<IMessageService, MessageService>();

        return services;
    }
}
