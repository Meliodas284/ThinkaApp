using Thinka.Domain.Dto;
using Thinka.Domain.Dto.Ideas;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class FeedService : IFeedService
{
    private readonly IIdeaRepository _ideaRepository;
    private readonly ICurrentUserService _currentUserService;

    public FeedService(IIdeaRepository ideaRepository, ICurrentUserService currentUserService)
    {
        _ideaRepository = ideaRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<IdeaDto>> GetFeedAsync(PaginationQuery query)
    {
        var currentUserId = _currentUserService.UserId;
        var ideas = await _ideaRepository.GetFeedAsync(currentUserId, query.Page, query.PageSize);

        return ideas.Select(idea => new IdeaDto
        {
            Id = idea.Id,
            Title = idea.Title,
            ShortDescription = idea.ShortDescription,
            Author = new AuthorDto
            {
                Id = idea.Author.Id,
                Username = idea.Author?.UserName ?? string.Empty
            },
            LikesCount = idea.Likes.Count,
            CommentsCount = idea.Comments.Count
        }).ToList();
    }
}