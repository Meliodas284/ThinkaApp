using Thinka.Domain.Dto.Ideas;
using Thinka.Domain.Interfaces.SearchProviders;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class IdeasSearchService : IIdeasSearchService
{
    private readonly IIdeaSearchProvider _ideaSearchProvider;

    public IdeasSearchService(IIdeaSearchProvider ideaSearchProvider)
    {
        _ideaSearchProvider = ideaSearchProvider;
    }

    public async Task<IdeasSearchResultDto> SearchAsync(SearchIdeasQueryDto query)
    {
        var (searchIdeas, totalCount) = await _ideaSearchProvider.SearchIdeasAsync(query);

        return new IdeasSearchResultDto
        {
            Ideas = searchIdeas.Select(si => new IdeaDto
            {
                Id = si.Id,
                Author = new AuthorDto
                {
                    Id = si.Author.Id,
                    Username = si.Author?.UserName ?? string.Empty,
                },
                Title = si.Title,
                ShortDescription = si.ShortDescription,
                LikesCount = si.Likes.Count(),
                CommentsCount = si.Comments.Count()
            }).ToList(),

            TotalCount = totalCount
        };
    }
}
