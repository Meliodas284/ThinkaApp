using Thinka.Application.Common;
using Thinka.Domain.Dto;
using Thinka.Domain.Dto.IdeasComments;
using Thinka.Domain.Entities;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IIdeaRepository _ideaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CommentService(
        ICommentRepository commentRepository, 
        IIdeaRepository ideaRepository,
        ICurrentUserService currentUserService, 
        IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _ideaRepository = ideaRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateComment(CreateCommentDto createCommentDto)
    {
        var ideaExists = await _ideaRepository.ExistsAsync(createCommentDto.IdeaId);
        if (!ideaExists)
        {
            throw new NotFoundException("Idea is not found");
        }

        var authorId = _currentUserService.UserId;
        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            AuthorId = authorId,
            Content = createCommentDto.Content.Trim(),
            IdeaId = createCommentDto.IdeaId,
        };

        await _commentRepository.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<CommentDto>> GetComments(Guid ideaId, int page, int pageSize)
    {
        var ideaExists = await _ideaRepository.ExistsAsync(ideaId);
        if (!ideaExists)
        {
            throw new NotFoundException("Idea is not found");
        }

        var pagination = new PaginationQuery
        {
            Page = page,
            PageSize = pageSize
        };
        var comments = await _commentRepository.GetAllByIdeaIdAsync(ideaId, pagination.GetNormalizedPage(), pagination.GetNormalizedPageSize());

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
        var userId = _currentUserService.UserId;
        var comment = await _commentRepository.GetByIdAsync(commentId);

        if (comment is null)
        {
            throw new NotFoundException("Comment not found");
        }

        if (comment.AuthorId != userId)
        {
            throw new ForbiddenException("User is not authorized to update this comment.");
        }

        comment.Content = updateCommentDto.Content.Trim();
        await _commentRepository.UpdateAsync(comment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteComment(Guid commentId)
    {
        var userId = _currentUserService.UserId;
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
        await _unitOfWork.SaveChangesAsync();
    }
}
