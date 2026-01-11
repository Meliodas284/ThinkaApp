using Thinka.Domain.Enums;

namespace Thinka.Domain.Dto;

public class SearchIdeasQueryDto
{
    public string Query { get; init; } = null!;

    public Category? Category { get; init; }

    public Guid? AuthorId { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}
