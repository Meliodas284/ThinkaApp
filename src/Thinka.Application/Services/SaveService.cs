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
    private readonly ICurrentUserService _currentUserService;

    public SaveService(ISaveRepository saveRepository, IIdeaRepository ideaRepository, ICurrentUserService currentUserService)
    {
        _saveRepository = saveRepository;
        _ideaRepository = ideaRepository;
        _currentUserService = currentUserService;
    }

    public async Task ToggleSaveAsync(Guid ideaId)
    {
        var idea = await _ideaRepository.GetByIdAsync(ideaId);
        if (idea is null)
        {
            throw new NotFoundException("Idea is not found");
        }

        var userId = _currentUserService.UserId;
        var save = await _saveRepository.GetAsync(ideaId, userId);

        if (save is null)
        {
            save = new Save
            {
                IdeaId = ideaId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
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
        var userId = _currentUserService.UserId;
        var ideas = await _saveRepository.GetSavedIdeasAsync(userId, pageNumber, pageSize);
        
        return ideas.Select(idea => new IdeaDto
        {
            Id = idea.Id,
            Title = idea.Title,
            ShortDescription = idea.ShortDescription,
            Author = new AuthorDto
            {
                Id = idea.AuthorId,
                Username = idea.Author?.UserName ?? string.Empty
            },
            LikesCount = idea.Likes.Count(),
            CommentsCount = idea.Comments.Count()
        }).ToList();
    }
}