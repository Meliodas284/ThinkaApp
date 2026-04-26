using Thinka.Domain.Dto;

namespace Thinka.Application.Common;

public static class PaginationQueryExtensions
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    public static int GetNormalizedPage(this PaginationQuery query)
    {
        return query.Page < DefaultPage ? DefaultPage : query.Page;
    }

    public static int GetNormalizedPageSize(this PaginationQuery query)
    {
        if (query.PageSize < 1)
        {
            return DefaultPageSize;
        }

        return query.PageSize > MaxPageSize ? MaxPageSize : query.PageSize;
    }
}
