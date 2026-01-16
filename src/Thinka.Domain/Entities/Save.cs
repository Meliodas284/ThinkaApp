using Thinka.Domain.Interfaces.Common;

namespace Thinka.Domain.Entities;

public class Save : IAuditable
{
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public Guid IdeaId { get; set; }

    public Idea Idea { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
