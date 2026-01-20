using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Thinka.Domain.Dto.Ideas;
using Thinka.Domain.Entities;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class SaveService : ISaveService
{
    private readonly ISaveRepository _saveRepository;
    private readonly IIdeaRepository _ideaRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SaveService(ISaveRepository saveRepository, IIdeaRepository ideaRepository, IHttpContextAccessor httpContextAccessor)
    {
        _saveRepository = saveRepository;
        _ideaRepository = ideaRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task ToggleSaveAsync(Guid ideaId)
    {
        var idea = await _ideaRepository.GetByIdAsync(ideaId);
        if (idea is null)
        {
            throw new NotFoundException("Idea is not found");
        }

        var userId = GetCurrentUserId();
        var save = await _saveRepository.GetAsync(ideaId, userId);

        if (save is null)
        {
            save = new Save
            {
                IdeaId = ideaId,
                UserId = userId,
                CreationDate = DateTime.UtcNow
            };
            await _saveRepository.AddAsync(save);
        }
        else
        {
            await _saveRepository.DeleteAsync(save);
        }
    }

    public async Task<List<IdeaDto>> GetSavedIdeasAsync(int pageNumber, int pageSize)
    {
        var userId = GetCurrentUserId();
        var ideas = await _saveRepository.GetSavedIdeasAsync(userId, pageNumber, pageSize);
        
        return ideas.Select(idea => new IdeaDto
        {
            Id = idea.Id,
            Title = idea.Title,
            ShortDescription = idea.ShortDescription,
            AuthorId = idea.AuthorId
        }).ToList();
    }

    private Guid GetCurrentUserId()
    {
        var userIdValue = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
        {
            throw new UnauthorizedException("User ID not found or invalid in token.");
        }
        return userId;
    }
}
