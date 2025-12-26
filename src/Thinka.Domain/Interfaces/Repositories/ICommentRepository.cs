using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.Repositories;

public interface ICommentRepository
{
    Task AddAsync(Comment comment);

    Task<IEnumerable<Comment>> GetAllByIdeaIdAsync(Guid ideaId, int page, int pageSize);
}
