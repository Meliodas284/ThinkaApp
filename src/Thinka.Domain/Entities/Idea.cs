using Thinka.Domain.Enums;

namespace Thinka.Domain.Entities;

public class Idea
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;
    
    public string FullDescription { get; set; } = null!;

    public Category Category { get; set; }

    public Guid AuthorId { get; set; }

    public User Author { get; set; } = null!;
    
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}
