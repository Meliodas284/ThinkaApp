using Thinka.Application.Common;
using Thinka.Domain.Dto;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public SaveService(
        ISaveRepository saveRepository, 
        IIdeaRepository ideaRepository, 
        ICurrentUserService currentUserService, 
        IUnitOfWork unitOfWork)
    {
        _saveRepository = saveRepository;
        _ideaRepository = ideaRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task ToggleSaveAsync(Guid ideaId)
    {
        if (!await _ideaRepository.ExistsAsync(ideaId))
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
            };
            await _saveRepository.AddAsync(save);
        }
        else
        {
            await _saveRepository.DeleteAsync(save);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<IdeaDto>> GetSavedIdeasAsync(int pageNumber, int pageSize)
    {
        var userId = _currentUserService.UserId;
        var pagination = new PaginationQuery
        {
            Page = pageNumber,
            PageSize = pageSize
        };
        var ideas = await _saveRepository.GetSavedIdeasAsync(userId, pagination.GetNormalizedPage(), pagination.GetNormalizedPageSize());
        
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
