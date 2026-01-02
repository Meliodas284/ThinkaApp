using Thinka.Domain.Dto;

namespace Thinka.Domain.Interfaces.Services;

public interface IIdeaService
{
    Task<IdeaDto> CreateIdeaAsync(CreateIdeaDto createIdeaDto);
    Task<List<IdeaDto>> GetUserIdeasAsync(int pageNumber, int pageSize, Guid? userId = null);
    Task UpdateIdeaAsync(Guid ideaId, UpdateIdeaDto updateIdeaDto);
    Task DeleteIdeaAsync(Guid ideaId);
    Task<IdeaDto?> GetIdeaByIdAsync(Guid ideaId);
}
