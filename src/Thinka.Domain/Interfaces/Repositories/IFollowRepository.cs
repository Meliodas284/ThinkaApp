using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.Repositories;

public interface IFollowRepository
{
    Task<Follow?> GetAsync(Guid followerId, Guid followingId);
    Task AddAsync(Follow follow);
    Task DeleteAsync(Follow follow);
    Task<List<User>> GetFollowersAsync(Guid userId, int pageNumber, int pageSize);
    Task<List<User>> GetFollowingAsync(Guid userId, int pageNumber, int pageSize);
    Task<int> CountFollowersAsync(Guid userId);
    Task<int> CountFollowingAsync(Guid userId);
    Task<List<Guid>> GetFollowingIdsAsync(Guid userId);
}
