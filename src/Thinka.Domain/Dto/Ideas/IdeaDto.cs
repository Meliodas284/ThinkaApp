namespace Thinka.Domain.Dto.Ideas;

public class IdeaDto
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public AuthorDto Author { get; set; } = null!;

    public int LikesCount { get; set; }

    public int CommentsCount { get; set; }
}
