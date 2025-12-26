using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Thinka.Domain.Dto;
using Thinka.Domain.Entities;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommentService(ICommentRepository commentRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateComment(CreateCommentDto createCommentDto)
    {
        var authorId = GetCurrentUserId();
        var comment = new Comment
        {
            AuthorId = authorId,
            Content = createCommentDto.Content,
            IdeaId = createCommentDto.IdeaId,
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepository.AddAsync(comment);
    }

    public async Task<IEnumerable<CommentDto>> GetComments(Guid ideaId, int page, int pageSize)
    {
        var comments = await _commentRepository.GetAllByIdeaIdAsync(ideaId, page, pageSize);

        return comments.Select(c => new CommentDto
        {
            Id = c.Id,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            AuthorId = c.AuthorId,
            AuthorUsername = c.Author.UserName,
            IdeaId = c.IdeaId
        });
    }
    
    private Guid GetCurrentUserId()
    {
        var userIdValue = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
        {
            throw new InvalidOperationException("User ID not found or invalid in token.");
        }
        return userId;
    }
}

