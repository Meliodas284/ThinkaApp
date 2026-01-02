namespace Thinka.Domain.Entities;

public class Save
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid IdeaId { get; set; }
    public Idea Idea { get; set; } = null!;
    public DateTime CreationDate { get; set; }
}
