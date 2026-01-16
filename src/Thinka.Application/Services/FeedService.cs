using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Thinka.Domain.Dto;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class FeedService : IFeedService
{
    private readonly IIdeaRepository _ideaRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FeedService(IIdeaRepository ideaRepository, IHttpContextAccessor httpContextAccessor)
    {
        _ideaRepository = ideaRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<IdeaFeedDto>> GetFeedAsync(PaginationQuery query)
    {
        var currentUserId = GetCurrentUserId();
        var ideas = await _ideaRepository.GetFeedAsync(currentUserId, query.Page, query.PageSize);

        return ideas.Select(idea => new IdeaFeedDto
        {
            Id = idea.Id,
            Title = idea.Title,
            ShortDescription = idea.ShortDescription,
            Category = idea.Category.ToString(),
            Author = new AuthorDto
            {
                Id = idea.Author.Id,
                Username = idea.Author.UserName ?? string.Empty
            },
            LikesCount = idea.Likes.Count,
            CommentsCount = idea.Comments.Count,
            CreatedAt = idea.CreatedAt
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
