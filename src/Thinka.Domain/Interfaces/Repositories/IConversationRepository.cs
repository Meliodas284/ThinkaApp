using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.Repositories;

public interface IConversationRepository
{
    Task<Conversation?> GetById(Guid id);
    Task<Conversation?> GetBetweenUsers(Guid user1Id, Guid user2Id);
    Task<List<Conversation>> GetForUser(Guid userId);
    Task Add(Conversation conversation);
}