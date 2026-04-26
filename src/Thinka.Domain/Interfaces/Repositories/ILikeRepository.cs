using System;
using System.Threading.Tasks;
using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.Repositories;

public interface ILikeRepository
{
    Task<Like?> GetByIdeaAndUserAsync(Guid ideaId, Guid userId);
    Task AddAsync(Like like);
    
    Task DeleteAsync(Like like);
    
    Task<int> CountLikesByAuthorIdAsync(Guid authorId);
}
