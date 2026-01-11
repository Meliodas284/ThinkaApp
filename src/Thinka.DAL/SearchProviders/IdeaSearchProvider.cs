using Microsoft.EntityFrameworkCore;
using Thinka.Domain.Dto;
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
        var ideasQuery = _context.Ideas.AsNoTracking().AsQueryable();

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

        if (hasSearchQuery)
        {
            ideasQuery = ideasQuery.OrderByDescending(i =>
                i.SearchVector.Rank(
                    EF.Functions.WebSearchToTsQuery("russian", query.Query)
                ));
        }

        var pagedIdeas = await ideasQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (pagedIdeas, totalCount);
    }
}