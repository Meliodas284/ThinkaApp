namespace Thinka.Domain.Dto;

public class CreateCommentDto
{
    public string Content { get; set; } = null!;

    public Guid IdeaId { get; set; }
}
