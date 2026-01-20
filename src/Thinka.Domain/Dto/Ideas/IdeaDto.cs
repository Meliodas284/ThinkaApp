namespace Thinka.Domain.Dto.Ideas;

public class IdeaDto
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public Guid AuthorId { get; set; }
}
