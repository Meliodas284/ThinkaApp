using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thinka.Domain.Dto;
using Thinka.Domain.Dto.Chat;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.API.Controllers;

[Authorize]
[ApiController]
[Route("api/conversations")]
public class ConversationsController : ControllerBase
{
    private readonly IConversationService _conversationService;
    private readonly IMessageService _messageService;
    private readonly ICurrentUserService _currentUserService;

    public ConversationsController(IConversationService conversationService, IMessageService messageService, ICurrentUserService currentUserService)
    {
        _conversationService = conversationService;
        _messageService = messageService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetConversations()
    {
        var conversations = await _conversationService.GetUserConversations();
        return Ok(conversations);
    }

    [HttpGet("{id:guid}/messages")]
    public async Task<IActionResult> GetMessages([FromRoute] Guid id, [FromQuery] PaginationQuery pagination)
    {
        var messages = await _messageService.GetMessages(_currentUserService.UserId, id, pagination);
        return Ok(messages);
    }

    [HttpPost("/api/messages")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageDto message)
    {
        var newMessage = await _messageService.SendMessage(_currentUserService.UserId, message);
        return CreatedAtAction(nameof(GetMessages), new { id = newMessage.ConversationId }, newMessage);
    }
}