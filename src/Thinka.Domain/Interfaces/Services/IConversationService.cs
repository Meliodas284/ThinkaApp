using Thinka.Domain.Dto.Chat;

namespace Thinka.Domain.Interfaces.Services;

public interface IConversationService
{
    Task<List<ConversationDto>> GetUserConversations();
}