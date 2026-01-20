using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Thinka.Domain.Dto.Ideas;
using Thinka.Domain.Entities;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class IdeaService : IIdeaService
{
    private readonly IIdeaRepository _ideaRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdeaService(IIdeaRepository ideaRepository, IHttpContextAccessor httpContextAccessor)
    {
        _ideaRepository = ideaRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<FullIdeaDto> CreateIdeaAsync(CreateIdeaDto createIdeaDto)
    {
        var authorId = GetCurrentUserId();
        var idea = new Idea
        {
            Id = Guid.NewGuid(),
            Title = createIdeaDto.Title,
            ShortDescription = createIdeaDto.ShortDescription,
            FullDescription = createIdeaDto.FullDescription,
            Category = createIdeaDto.Category,
            AuthorId = authorId
        };

        await _ideaRepository.AddAsync(idea);

        return new FullIdeaDto
        {
            Id = idea.Id,
            Title = idea.Title,
            ShortDescription = idea.ShortDescription,
            FullDescription = idea.FullDescription,
            Category = idea.Category.ToString(),
            AuthorId = idea.AuthorId
        };
    }

    public async Task<List<IdeaDto>> GetUserIdeasAsync(int pageNumber, int pageSize, Guid? userId = null)
    {
        var authorId = userId ?? GetCurrentUserId();
        var ideas = await _ideaRepository.GetByAuthorIdAsync(authorId, pageNumber, pageSize);
        return ideas.Select(idea => new IdeaDto
        {
            Id = idea.Id,
            Title = idea.Title,
            ShortDescription = idea.ShortDescription,
            AuthorId = idea.AuthorId
        }).ToList();
    }

    public async Task UpdateIdeaAsync(Guid ideaId, UpdateIdeaDto updateIdeaDto)
    {
        var idea = await _ideaRepository.GetByIdAsync(ideaId);
        var userId = GetCurrentUserId();

        if (idea == null)
        {
            throw new NotFoundException("Idea is not found");
        }

        if (idea.AuthorId != userId)
        {
            throw new ForbiddenException("User is not authorized to update this idea.");
        }

        idea.Title = updateIdeaDto.Title;
        idea.ShortDescription = updateIdeaDto.ShortDescription;
        idea.FullDescription = updateIdeaDto.FullDescription;
        idea.Category = updateIdeaDto.Category;

        await _ideaRepository.UpdateAsync(idea);
    }

    public async Task DeleteIdeaAsync(Guid ideaId)
    {
        var idea = await _ideaRepository.GetByIdAsync(ideaId);
        var userId = GetCurrentUserId();
        
        if (idea == null)
        {
            return;
        }

        if (idea.AuthorId != userId)
        {
            throw new ForbiddenException("User is not authorized to delete this idea.");
        }

        await _ideaRepository.DeleteAsync(idea);
    }

    public async Task<FullIdeaDto?> GetIdeaByIdAsync(Guid ideaId)
    {
        var idea = await _ideaRepository.GetByIdAsync(ideaId);

        if (idea == null)
        {
            return null;
        }

        return new FullIdeaDto
        {
            Id = idea.Id,
            Title = idea.Title,
            ShortDescription = idea.ShortDescription,
            FullDescription = idea.FullDescription,
            Category = idea.Category.ToString(),
            AuthorId = idea.AuthorId
        };
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
