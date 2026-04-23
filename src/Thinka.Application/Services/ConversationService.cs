using Thinka.Domain.Dto.Chat;
using Thinka.Domain.Dto.Ideas;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class ConversationService : IConversationService
{
    private readonly IConversationRepository _conversationRepository;
    private readonly ICurrentUserService _currentUserService;

    public ConversationService(
        IConversationRepository conversationRepository, 
        ICurrentUserService currentUserService)
    {
        _conversationRepository = conversationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<ConversationDto>> GetUserConversations()
    {
        var userId = _currentUserService.UserId;

        var conversations = await _conversationRepository.GetForUser(userId);

        return conversations.Select(c =>
        {
            var participant = c.User1Id == userId ? c.User2 : c.User1;

            var lastMessage = c.Messages
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefault();

            return new ConversationDto
            {
                Id = c.Id,
                Participant = new AuthorDto
                {
                    Id = participant.Id,
                    Username = participant.UserName ?? string.Empty
                },
                LastMessage = lastMessage == null ? null : new MessageDto
                {
                    Id = lastMessage.Id,
                    ConversationId = lastMessage.ConversationId,
                    SenderId = lastMessage.SenderId,
                    Text = lastMessage.Text,
                    CreatedAt = lastMessage.CreatedAt
                }
            };
        }).ToList();
    }
}