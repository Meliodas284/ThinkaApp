using Thinka.Domain.Dto;
using Thinka.Domain.Dto.Ideas;

namespace Thinka.Domain.Interfaces.Services;

public interface IFeedService
{
    Task<List<IdeaDto>> GetFeedAsync(PaginationQuery query);
}
