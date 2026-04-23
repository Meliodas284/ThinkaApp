using Microsoft.AspNetCore.SignalR;
using Thinka.API.Hubs;
using Thinka.Domain.Dto.Chat;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.API.Services;

public class ChatHubService : IChatHubService
{
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatHubService(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task BroadcastMessage(MessageDto message)
    {
        await _hubContext.Clients
            .Group(message.ConversationId.ToString())
            .SendAsync("MessageReceived", message);
    }
}