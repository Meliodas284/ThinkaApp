using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Thinka.Domain.Exceptions;

namespace Thinka.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task JoinConversation(Guid conversationId)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedException("User is not authorized");

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