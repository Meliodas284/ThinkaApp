using System.ComponentModel.DataAnnotations;
using Thinka.Domain.Enums;

namespace Thinka.Domain.Dto;

public class CreateIdeaDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(250)]
    public string ShortDescription { get; set; } = null!;
    
    [Required]
    public string FullDescription { get; set; } = null!;

    [Required]
    public Category Category { get; set; }
}
