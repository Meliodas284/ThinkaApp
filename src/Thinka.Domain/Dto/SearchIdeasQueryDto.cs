using Thinka.Domain.Enums;

namespace Thinka.Domain.Dto;

public class SearchIdeasQueryDto : PaginationQuery
{
    public string Query { get; init; } = null!;

    public Category? Category { get; init; }

    public Guid? AuthorId { get; init; }
}
