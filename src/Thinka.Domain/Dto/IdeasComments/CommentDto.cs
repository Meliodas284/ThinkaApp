namespace Thinka.Domain.Dto.IdeasComments;

public class CommentDto
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public Guid AuthorId { get; set; }
    
    public string AuthorUsername { get; set; } = null!;

    public Guid IdeaId { get; set; }
}
