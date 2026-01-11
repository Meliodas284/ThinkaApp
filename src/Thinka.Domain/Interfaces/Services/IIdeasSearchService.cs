using Thinka.Domain.Dto;

namespace Thinka.Domain.Interfaces.Services;

public interface IIdeasSearchService
{
    Task<IdeasSearchResultDto> SearchAsync(SearchIdeasQueryDto query);
}