using System;

namespace Thinka.Domain.Dto;

public class IdeaFeedDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public AuthorDto Author { get; set; } = null!;
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
