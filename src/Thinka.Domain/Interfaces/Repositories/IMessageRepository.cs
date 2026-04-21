using Thinka.Domain.Dto;
using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.Repositories;

public interface IMessageRepository
{
    Task<List<Message>> GetByConversation(Guid conversationId, PaginationQuery pagination);
    Task Add(Message message);
}