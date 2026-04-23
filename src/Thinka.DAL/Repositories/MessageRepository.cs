using Microsoft.EntityFrameworkCore;
using Thinka.Domain.Dto;
using Thinka.Domain.Entities;
using Thinka.Domain.Interfaces.Repositories;

namespace Thinka.DAL.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ThinkaDbContext _context;

    public MessageRepository(ThinkaDbContext context)
    {
        _context = context;
    }

    public async Task Add(Message message)
    {
        await _context.Messages.AddAsync(message);
    }

    public async Task<List<Message>> GetByConversation(Guid conversationId, PaginationQuery pagination)
    {
        return await _context.Messages
            .AsNoTracking()
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.CreatedAt)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
    }
}