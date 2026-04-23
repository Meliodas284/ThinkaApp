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
    }

    public async Task DeleteAsync(Save save)
    {
        _context.Saves.Remove(save);
        await Task.CompletedTask;
    }

    public async Task<Save?> GetAsync(Guid ideaId, Guid userId)
    {
        return await _context.Saves.FirstOrDefaultAsync(s => s.IdeaId.Equals(ideaId) && s.UserId.Equals(userId));
    }
    
    public async Task<int> CountSavesByUserIdAsync(Guid userId)
    {
        return await _context.Saves.CountAsync(s => s.UserId == userId);
    }

    public async Task<List<Idea>> GetSavedIdeasAsync(Guid userId, int pageNumber, int pageSize)
    {
        return await _context.Saves
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => s.Idea)
            .Include(i => i.Author)
            .Include(i => i.Likes)
            .Include(i => i.Comments)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
