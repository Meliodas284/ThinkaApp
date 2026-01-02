using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.Repositories;

public interface ISaveRepository
{
    Task AddAsync(Save save);
    Task DeleteAsync(Save save);
    Task<Save?> GetAsync(Guid ideaId, Guid userId);
    Task<List<Idea>> GetSavedIdeasAsync(Guid userId, int pageNumber, int pageSize);
    Task<int> CountSavesByUserIdAsync(Guid userId);
}
