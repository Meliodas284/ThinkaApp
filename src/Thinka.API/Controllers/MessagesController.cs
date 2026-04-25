using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thinka.Domain.Dto.Chat;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.API.Controllers;

[Authorize]
[ApiController]
[Route("api/messages")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ICurrentUserService _currentUserService;

    public MessagesController(IMessageService messageService, ICurrentUserService currentUserService)
    {
        _messageService = messageService;
        _currentUserService = currentUserService;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageDto message)
    {
        var newMessage = await _messageService.SendMessage(_currentUserService.UserId, message);
        return CreatedAtRoute("GetConversationMessages", new { id = newMessage.ConversationId }, newMessage);
    }
}
