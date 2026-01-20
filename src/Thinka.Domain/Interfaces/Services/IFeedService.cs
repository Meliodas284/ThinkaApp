using Thinka.Domain.Dto;

namespace Thinka.Domain.Interfaces.Services;

public interface IFeedService
{
    Task<List<IdeaFeedDto>> GetFeedAsync(PaginationQuery query);
}
