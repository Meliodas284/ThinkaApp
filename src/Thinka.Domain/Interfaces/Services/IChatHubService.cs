using Thinka.Domain.Dto.Chat;

namespace Thinka.Domain.Interfaces.Services;

public interface IChatHubService
{
    Task BroadcastMessage(MessageDto message);
}