namespace Thinka.Domain.Dto.Ideas;

public class FullIdeaDto : IdeaDto
{
    public string FullDescription { get; set; } = null!;

    public string Category { get; set; } = null!;
}