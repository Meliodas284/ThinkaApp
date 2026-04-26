using System.ComponentModel.DataAnnotations;

namespace Thinka.Domain.Dto.IdeasComments;

public class CreateCommentDto
{
    [Required]
    [MaxLength(500)]
    public string Content { get; set; } = null!;

    [Required]
    public Guid IdeaId { get; set; }
}
