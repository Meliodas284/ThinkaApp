using Microsoft.EntityFrameworkCore;
using Thinka.Domain.Entities;
using Thinka.Domain.Interfaces.Repositories;

namespace Thinka.DAL.Repositories;

public class IdeaRepository : IIdeaRepository
{
    private readonly ThinkaDbContext _context;

    public IdeaRepository(ThinkaDbContext context)
    {
        _context = context;
    }

    public async Task<Idea?> GetByIdAsync(Guid id)
    {
        return await _context.Ideas
            .Include(i => i.Author)
            .Include(i => i.Likes)
            .Include(i => i.Comments)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Idea?> GetByIdWithLikesAsync(Guid id)
    {
        return await _context.Ideas
            .AsNoTracking()
            .Include(i => i.Likes)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Idea>> GetByAuthorIdAsync(Guid authorId, int pageNumber, int pageSize)
    {
        return await _context.Ideas
            .AsNoTracking()
            .Include(i => i.Author)
            .Include(i => i.Likes)
            .Include(i => i.Comments)
            .Where(i => i.AuthorId == authorId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Idea>> GetFeedAsync(Guid currentUserId, int pageNumber, int pageSize)
    {
        return await _context.Ideas
            .AsNoTracking()
            .Include(i => i.Author)
            .Include(i => i.Likes)
            .Include(i => i.Comments)
            .Where(i => i.AuthorId != currentUserId)
            .OrderByDescending(i => (i.Likes.Count * 1.5) + (i.Comments.Count * 2))
            .ThenByDescending(i => i.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountByAuthorIdAsync(Guid authorId)
    {
        return await _context.Ideas.CountAsync(i => i.AuthorId == authorId);
    }

    public async Task AddAsync(Idea idea)
    {
        await _context.Ideas.AddAsync(idea);
    }

    public async Task UpdateAsync(Idea idea)
    {
        _context.Ideas.Update(idea);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Idea idea)
    {
        _context.Ideas.Remove(idea);
        await Task.CompletedTask;
    }
}
