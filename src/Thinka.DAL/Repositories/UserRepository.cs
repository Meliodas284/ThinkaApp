using Microsoft.EntityFrameworkCore;
using Thinka.Domain.Entities;
using Thinka.Domain.Interfaces.Repositories;

namespace Thinka.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ThinkaDbContext _context;

    public UserRepository(ThinkaDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> IsEmailTakenAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}
