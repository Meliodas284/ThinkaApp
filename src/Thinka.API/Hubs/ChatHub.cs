using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Thinka.Domain.Interfaces.Repositories;

namespace Thinka.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IConversationRepository _conversationRepository;

    public ChatHub(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task JoinConversation(Guid conversationId)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var parsedUserId))
            throw new HubException("User is not authorized.");

        var conversation = await _conversationRepository.GetById(conversationId);
        if (conversation is null)
            throw new HubException("Conversation not found.");

        if (!conversation.HasParticipant(parsedUserId))
            throw new HubException("You do not have access to this conversation.");

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            conversationId.ToString());
    }

    public async Task LeaveConversation(Guid conversationId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            conversationId.ToString());
    }
}
