using System.ComponentModel.DataAnnotations;

namespace Thinka.Domain.Dto.IdeasComments;

public class UpdateCommentDto
{
    [Required]
    [MaxLength(500)]
    public string Content { get; set; } = null!;
}
