using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Thinka.Domain.Dto;
using Thinka.Domain.Entities;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommentService(ICommentRepository commentRepository, IHttpContextAccessor httpContextAccessor)
    {
        _commentRepository = commentRepository;
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

    public async Task UpdateComment(Guid commentId, UpdateCommentDto updateCommentDto)
    {
        var userId = GetCurrentUserId();
        var comment = await _commentRepository.GetByIdAsync(commentId);

        if (comment is null)
        {
            throw new NotFoundException("Comment not found");
        }

        if (comment.AuthorId != userId)
        {
            throw new ForbiddenException("User is not authorized to update this comment.");
        }

        comment.Content = updateCommentDto.Content;
        await _commentRepository.UpdateAsync(comment);
    }

    public async Task DeleteComment(Guid commentId)
    {
        var userId = GetCurrentUserId();
        var comment = await _commentRepository.GetByIdAsync(commentId);
        
        if (comment is null)
        {
            return;
        }

        if (comment.AuthorId != userId)
        {
            throw new ForbiddenException("User is not authorized to delete this comment.");
        }

        await _commentRepository.DeleteAsync(comment);
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

