using System.ComponentModel.DataAnnotations;

namespace Thinka.Domain.Dto.Chat;

public class SendMessageDto
{
    [Required]
    public Guid RecipientId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Text { get; set; } = null!;
}