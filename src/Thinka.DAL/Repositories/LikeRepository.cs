using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Thinka.Domain.Entities;
using Thinka.Domain.Interfaces.Repositories;

namespace Thinka.DAL.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly ThinkaDbContext _context;

    public LikeRepository(ThinkaDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Like like)
    {
        await _context.Likes.AddAsync(like);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Like like)
    {
        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();
    }
}
