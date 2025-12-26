using Thinka.Domain.Dto;

namespace Thinka.Domain.Interfaces.Services;

public interface ICommentService
{
    Task CreateComment(CreateCommentDto createCommentDto);
    
    Task<IEnumerable<CommentDto>> GetComments(Guid ideaId, int page, int pageSize);
    
    Task UpdateComment(Guid commentId, UpdateCommentDto updateCommentDto);
    
    Task DeleteComment(Guid commentId);
}
