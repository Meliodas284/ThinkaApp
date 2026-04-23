using Microsoft.EntityFrameworkCore;
using Thinka.Domain.Entities;
using Thinka.Domain.Interfaces.Repositories;

namespace Thinka.DAL.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly ThinkaDbContext _context;

    public ConversationRepository(ThinkaDbContext context)
    {
        _context = context;
    }

    public async Task Add(Conversation conversation)
    {
        await _context.Conversations.AddAsync(conversation);
    }

    public async Task<Conversation?> GetBetweenUsers(Guid user1Id, Guid user2Id)
    {
        return await _context.Conversations
            .FirstOrDefaultAsync(c => c.User1Id == user1Id && c.User2Id == user2Id);
    }

    public async Task<Conversation?> GetById(Guid id)
    {
        return await _context.Conversations
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Conversation>> GetForUser(Guid userId)
    {
        return await _context.Conversations
            .AsNoTracking()
            .Where(c => c.User1Id == userId || c.User2Id == userId)
            .OrderByDescending(c => c.LastMessageAt)
            .Select(c => new Conversation
            {
                Id = c.Id,
                User1Id = c.User1Id,
                User2Id = c.User2Id,
                LastMessageAt = c.LastMessageAt,
                User1 = c.User1,
                User2 = c.User2,
                Messages = c.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(1)
                    .ToList()
            })
            .ToListAsync();
    }
}