using Thinka.Domain.Dto;

namespace Thinka.Domain.Interfaces.Services;

public interface ISaveService
{
    Task ToggleSaveAsync(Guid ideaId);
    Task<List<IdeaDto>> GetSavedIdeasAsync(int pageNumber, int pageSize);
}
