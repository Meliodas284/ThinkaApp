namespace Thinka.Domain.Entities;

public class Save
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid IdeaId { get; set; }
    public Idea Idea { get; set; }
    public DateTime CreationDate { get; set; }
}
