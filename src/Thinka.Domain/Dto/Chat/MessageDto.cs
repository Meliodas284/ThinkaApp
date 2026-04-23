namespace Thinka.Domain.Dto.Chat;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public Guid SenderId { get; set; }
    public string Text { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
}