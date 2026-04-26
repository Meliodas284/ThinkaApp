using Thinka.Application.Common;
using Thinka.Domain.Dto;
using Thinka.Domain.Dto.Ideas;
using Thinka.Domain.Entities;
using Thinka.Domain.Enums;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class IdeaService : IIdeaService
{
    private readonly IIdeaRepository _ideaRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public IdeaService(
        IIdeaRepository ideaRepository, 
        IUserRepository userRepository,
        ICurrentUserService currentUserService, 
        IUnitOfWork unitOfWork)
    {
        _ideaRepository = ideaRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<FullIdeaDto> CreateIdeaAsync(CreateIdeaDto createIdeaDto)
    {
        var authorId = _currentUserService.UserId;

        var author = await _userRepository.GetByIdAsync(authorId)
            ?? throw new NotFoundException("Author not found.");

        if (!Enum.TryParse<Category>(createIdeaDto.Category, true, out var category))
            throw new ArgumentException("Invalid category");

        var idea = new Idea
        {
            Id = Guid.NewGuid(),
            Title = createIdeaDto.Title.Trim(),
            ShortDescription = createIdeaDto.ShortDescription.Trim(),
            FullDescription = createIdeaDto.FullDescription.Trim(),
            Category = category,
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
                Username = author.UserName
            }
        };
    }

    public async Task<List<IdeaDto>> GetUserIdeasAsync(int pageNumber, int pageSize, Guid? userId = null)
    {
        var authorId = userId ?? _currentUserService.UserId;
        var pagination = new PaginationQuery
        {
            Page = pageNumber,
            PageSize = pageSize
        };
        var ideas = await _ideaRepository.GetByAuthorIdAsync(authorId, pagination.GetNormalizedPage(), pagination.GetNormalizedPageSize());
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

        idea.Title = updateIdeaDto.Title.Trim();
        idea.ShortDescription = updateIdeaDto.ShortDescription.Trim();
        idea.FullDescription = updateIdeaDto.FullDescription.Trim();
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
