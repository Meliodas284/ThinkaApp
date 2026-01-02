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
        return await _context.Ideas.FindAsync(id);
    }

    public async Task<Idea?> GetByIdWithLikesAsync(Guid id)
    {
        return await _context.Ideas
            .Include(i => i.Likes)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Idea>> GetByAuthorIdAsync(Guid authorId, int pageNumber, int pageSize)
    {
        return await _context.Ideas
            .Where(i => i.AuthorId == authorId)
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
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Idea idea)
    {
        _context.Ideas.Update(idea);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Idea idea)
    {
        _context.Ideas.Remove(idea);
        await _context.SaveChangesAsync();
    }
}
