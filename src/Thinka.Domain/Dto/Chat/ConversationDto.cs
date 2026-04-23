using Thinka.Domain.Dto.Ideas;

namespace Thinka.Domain.Dto.Chat;

public class ConversationDto
{
    public Guid Id { get; set; }
    public AuthorDto Participant { get; set; } = null!;
    public MessageDto? LastMessage { get; set; }
}