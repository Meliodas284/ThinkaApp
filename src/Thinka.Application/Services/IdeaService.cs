using Thinka.Domain.Dto.Ideas;
using Thinka.Domain.Entities;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class IdeaService : IIdeaService
{
    private readonly IIdeaRepository _ideaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public IdeaService(
        IIdeaRepository ideaRepository, 
        ICurrentUserService currentUserService, 
        IUnitOfWork unitOfWork)
    {
        _ideaRepository = ideaRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<FullIdeaDto> CreateIdeaAsync(CreateIdeaDto createIdeaDto)
    {
        var authorId = _currentUserService.UserId;
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
        await _unitOfWork.SaveChangesAsync();

        return new FullIdeaDto
        {
            Id = idea.Id,
            Title = idea.Title,
            ShortDescription = idea.ShortDescription,
            FullDescription = idea.FullDescription,
            Category = idea.Category.ToString(),
            Author = new AuthorDto
            {
                Id = authorId,
                Username = string.Empty
            }
        };
    }

    public async Task<List<IdeaDto>> GetUserIdeasAsync(int pageNumber, int pageSize, Guid? userId = null)
    {
        var authorId = userId ?? _currentUserService.UserId;
        var ideas = await _ideaRepository.GetByAuthorIdAsync(authorId, pageNumber, pageSize);
        return ideas.Select(idea => new IdeaDto
        {
            Id = idea.Id,
            Title = idea.Title,
            ShortDescription = idea.ShortDescription,
            Author = new AuthorDto
            {
                Id = idea.Author.Id,
                Username = idea.Author.UserName ?? string.Empty
            },
            LikesCount = idea.Likes.Count,
            CommentsCount = idea.Comments.Count
        }).ToList();
    }

    public async Task UpdateIdeaAsync(Guid ideaId, UpdateIdeaDto updateIdeaDto)
    {
        var idea = await _ideaRepository.GetByIdAsync(ideaId);
        var userId = _currentUserService.UserId;

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
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteIdeaAsync(Guid ideaId)
    {
        var idea = await _ideaRepository.GetByIdAsync(ideaId);
        var userId = _currentUserService.UserId;
        
        if (idea == null)
        {
            return;
        }

        if (idea.AuthorId != userId)
        {
            throw new ForbiddenException("User is not authorized to delete this idea.");
        }

        await _ideaRepository.DeleteAsync(idea);
        await _unitOfWork.SaveChangesAsync();
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
            Author = new AuthorDto
            {
                Id = idea.Author.Id,
                Username = idea.Author.UserName ?? string.Empty
            },
            LikesCount = idea.Likes.Count,
            CommentsCount = idea.Comments.Count
        };
    }
}