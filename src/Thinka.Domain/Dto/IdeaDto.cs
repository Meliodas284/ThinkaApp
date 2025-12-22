using Thinka.Domain.Enums;

namespace Thinka.Domain.Dto;

public class IdeaDto
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;
    
    public string FullDescription { get; set; } = null!;

    public string Category { get; set; } = null!;

    public Guid AuthorId { get; set; }
}
