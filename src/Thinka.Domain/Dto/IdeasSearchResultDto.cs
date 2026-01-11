namespace Thinka.Domain.Dto;

public class IdeasSearchResultDto
{
    public List<IdeaDto> Ideas { get; set; } = new();

    public int TotalCount { get; set; }
}
