using Thinka.Domain.Enums;
using NpgsqlTypes;
using Thinka.Domain.Interfaces.Common;

namespace Thinka.Domain.Entities;

public class Idea : IAuditable
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;
    
    public string FullDescription { get; set; } = null!;

    public Category Category { get; set; }

    public Guid AuthorId { get; set; }

    public User Author { get; set; } = null!;
    
    public ICollection<Like> Likes { get; set; } = new List<Like>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    public ICollection<Save> Saves { get; set; } = new List<Save>();

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public NpgsqlTsVector SearchVector { get; private set; } = null!;
}
