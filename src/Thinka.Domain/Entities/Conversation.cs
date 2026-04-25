using Thinka.Domain.Interfaces.Common;

namespace Thinka.Domain.Entities;

public class Conversation : IAuditable
{
    public Guid Id { get; set; }
    public Guid User1Id { get; set; }
    public User User1 { get; set; } = null!;
    public Guid User2Id { get; set; }
    public User User2 { get; set; } = null!;
    public DateTimeOffset? LastMessageAt { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public static Conversation Create(Guid user1Id, Guid user2Id)
    {
        if (user1Id == user2Id)
            throw new ArgumentException("Cannot create conversation with yourself");

        if (user1Id.CompareTo(user2Id) > 0)
            (user1Id, user2Id) = (user2Id, user1Id);

        return new Conversation
        {
            Id = Guid.NewGuid(),
            User1Id = user1Id,
            User2Id = user2Id
        };
    }

    public bool HasParticipant(Guid userId)
    {
        return User1Id == userId || User2Id == userId;
    }
}
