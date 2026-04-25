using Microsoft.EntityFrameworkCore;
using Thinka.Domain.Dto.Ideas;
using Thinka.Domain.Entities;
using Thinka.Domain.Enums;
using Thinka.Domain.Interfaces.SearchProviders;

namespace Thinka.DAL.SearchProviders;

public class IdeaSearchProvider : IIdeaSearchProvider
{
    private readonly ThinkaDbContext _context;

    public IdeaSearchProvider(ThinkaDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Idea> Ideas, int TotalCount)> SearchIdeasAsync(SearchIdeasQueryDto query)
    {
        var ideasQuery = _context.Ideas
            .AsNoTracking()
            .Include(i => i.Author)
            .Include(i => i.Likes)
            .Include(i => i.Comments)
            .AsQueryable();

        var hasSearchQuery = !string.IsNullOrWhiteSpace(query.Query);

        if (hasSearchQuery)
        {
            ideasQuery = ideasQuery.Where(i =>
                i.SearchVector.Matches(
                    EF.Functions.WebSearchToTsQuery("russian", query.Query)
                ));
        }

        if (query.Category.HasValue)
            ideasQuery = ideasQuery.Where(i => i.Category == query.Category.Value);

        if (query.AuthorId.HasValue)
            ideasQuery = ideasQuery.Where(i => i.AuthorId == query.AuthorId.Value);

        var totalCount = await ideasQuery.CountAsync();

        var orderedIdeasQuery = hasSearchQuery
            ? ideasQuery
                .OrderByDescending(i => i.SearchVector.Rank(
                    EF.Functions.WebSearchToTsQuery("russian", query.Query)
                ))
                .ThenByDescending(i => i.CreatedAt)
            : ideasQuery.OrderByDescending(i => i.CreatedAt);

        var pagedIdeas = await orderedIdeasQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (pagedIdeas, totalCount);
    }
}
