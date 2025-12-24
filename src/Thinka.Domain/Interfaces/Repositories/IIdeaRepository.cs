using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.Repositories;

public interface IIdeaRepository
{
    Task<Idea?> GetByIdAsync(Guid id);
    Task<Idea?> GetByIdWithLikesAsync(Guid id);
    Task<List<Idea>> GetByAuthorIdAsync(Guid authorId, int pageNumber, int pageSize);
    Task AddAsync(Idea idea);
    Task UpdateAsync(Idea idea);
    Task DeleteAsync(Idea idea);
}
