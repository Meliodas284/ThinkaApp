using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> IsEmailTakenAsync(string email);
}
