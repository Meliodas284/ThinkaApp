using Microsoft.EntityFrameworkCore;
using Thinka.Domain.Entities;
using Thinka.Domain.Interfaces.Repositories;

namespace Thinka.DAL.Repositories;

public class FollowRepository : IFollowRepository
{
    private readonly ThinkaDbContext _context;

    public FollowRepository(ThinkaDbContext context)
    {
        _context = context;
    }

    public async Task<Follow?> GetAsync(Guid followerId, Guid followingId)
    {
        return await _context.Follows.FindAsync(followerId, followingId);
    }

    public async Task AddAsync(Follow follow)
    {
        await _context.Follows.AddAsync(follow);
    }

    public async Task DeleteAsync(Follow follow)
    {
        _context.Follows.Remove(follow);
        await Task.CompletedTask;
    }

    public async Task<List<User>> GetFollowersAsync(Guid userId, int pageNumber, int pageSize)
    {
        return await _context.Follows
            .AsNoTracking()
            .Where(f => f.FollowingId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => f.Follower)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<User>> GetFollowingAsync(Guid userId, int pageNumber, int pageSize)
    {
        return await _context.Follows
            .AsNoTracking()
            .Where(f => f.FollowerId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => f.Following)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<int> CountFollowersAsync(Guid userId)
    {
        return _context.Follows.CountAsync(f => f.FollowingId == userId);
    }

    public Task<int> CountFollowingAsync(Guid userId)
    {
        return _context.Follows.CountAsync(f => f.FollowerId == userId);
    }

    public async Task<List<Guid>> GetFollowingIdsAsync(Guid userId)
    {
        return await _context.Follows
            .AsNoTracking()
            .Where(f => f.FollowerId == userId)
            .Select(f => f.FollowingId)
            .ToListAsync();
    }
}
