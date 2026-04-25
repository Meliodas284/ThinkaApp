using Microsoft.EntityFrameworkCore;
using Thinka.Domain.Entities;
using Thinka.Domain.Interfaces.Repositories;

namespace Thinka.DAL.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ThinkaDbContext _context;

    public CommentRepository(ThinkaDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
    }

    public async Task<Comment?> GetByIdAsync(Guid commentId)
    {
        return await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId);
    }

    public async Task<IEnumerable<Comment>> GetAllByIdeaIdAsync(Guid ideaId, int page, int pageSize)
    {
        return await _context.Comments
            .AsNoTracking()
            .Include(c => c.Author)
            .Where(c => c.IdeaId == ideaId)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task UpdateAsync(Comment comment)
    {
        _context.Comments.Update(comment);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Comment comment)
    {
        _context.Comments.Remove(comment);
        await Task.CompletedTask;
    }
}
