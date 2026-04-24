using Thinka.Domain.Interfaces.Common;

namespace Thinka.Domain.Entities;

public class Follow : IAuditable
{
    public Guid FollowerId { get; set; }

    public User Follower { get; set; } = null!;

    public Guid FollowingId { get; set; }

    public User Following { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
