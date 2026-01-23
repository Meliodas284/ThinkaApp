using Thinka.Domain.Dto.Ideas;

namespace Thinka.Domain.Interfaces.Services;

public interface IIdeasSearchService
{
    Task<IdeasSearchResultDto> SearchAsync(SearchIdeasQueryDto query);
}