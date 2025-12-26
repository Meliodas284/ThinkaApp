namespace Thinka.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid AuthorId { get; set; }

    public User Author { get; set; } = null!;

    public Guid IdeaId { get; set; }

    public Idea Idea { get; set; } = null!;
}
