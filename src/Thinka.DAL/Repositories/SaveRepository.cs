using Microsoft.EntityFrameworkCore;
using Thinka.Domain.Entities;
using Thinka.Domain.Interfaces.Repositories;

namespace Thinka.DAL.Repositories;

public class SaveRepository : ISaveRepository
{
    private readonly ThinkaDbContext _context;

    public SaveRepository(ThinkaDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Save save)
    {
        await _context.Saves.AddAsync(save);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Save save)
    {
        _context.Saves.Remove(save);
        await _context.SaveChangesAsync();
    }

    public async Task<Save?> GetAsync(Guid ideaId, Guid userId)
    {
        return await _context.Saves.FirstOrDefaultAsync(s => s.IdeaId.Equals(ideaId) && s.UserId.Equals(userId));
    }

    public async Task<List<Idea>> GetSavedIdeasAsync(Guid userId, int pageNumber, int pageSize)
    {
        return await _context.Saves
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreationDate)
            .Select(s => s.Idea)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
