using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.Repositories;

public interface ICommentRepository
{
    Task AddAsync(Comment comment);
    
    Task<Comment?> GetByIdAsync(Guid commentId);

    Task<IEnumerable<Comment>> GetAllByIdeaIdAsync(Guid ideaId, int page, int pageSize);
    
    Task UpdateAsync(Comment comment);
    
    Task DeleteAsync(Comment comment);
}
