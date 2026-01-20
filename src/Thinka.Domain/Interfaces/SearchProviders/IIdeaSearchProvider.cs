using Thinka.Domain.Dto.Ideas;
using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.SearchProviders;

public interface IIdeaSearchProvider
{
    Task<(IEnumerable<Idea> Ideas, int TotalCount)> SearchIdeasAsync(SearchIdeasQueryDto query);
}
