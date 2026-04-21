using Thinka.Domain.Dto;
using Thinka.Domain.Dto.Chat;

namespace Thinka.Domain.Interfaces.Services;

public interface IMessageService
{
    Task<List<MessageDto>> GetMessages(Guid userId, Guid conversationId, PaginationQuery pagination);
    Task<MessageDto> SendMessage(Guid senderId, SendMessageDto message);
}