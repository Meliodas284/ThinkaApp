using Thinka.Domain.Dto.Ideas;

namespace Thinka.Domain.Interfaces.Services;

public interface IIdeaService
{
    Task<FullIdeaDto> CreateIdeaAsync(CreateIdeaDto createIdeaDto);
    Task<List<IdeaDto>> GetUserIdeasAsync(int pageNumber, int pageSize, Guid? userId = null);
    Task UpdateIdeaAsync(Guid ideaId, UpdateIdeaDto updateIdeaDto);
    Task DeleteIdeaAsync(Guid ideaId);
    Task<FullIdeaDto?> GetIdeaByIdAsync(Guid ideaId);
}
