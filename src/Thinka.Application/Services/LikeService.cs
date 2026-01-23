using Thinka.Domain.Entities;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;
    private readonly IIdeaRepository _ideaRepository;
    private readonly ICurrentUserService _currentUserService;

    public LikeService(ILikeRepository likeRepository, IIdeaRepository ideaRepository, ICurrentUserService currentUserService)
    {
        _likeRepository = likeRepository;
        _ideaRepository = ideaRepository;
        _currentUserService = currentUserService;
    }

    public async Task ToggleLikeAsync(Guid ideaId)
    {
        var userId = _currentUserService.UserId;
        
        var idea = await _ideaRepository.GetByIdWithLikesAsync(ideaId);
        if (idea is null)
        {
            throw new NotFoundException("Idea not found");
        }

        var existingLike = idea.Likes.FirstOrDefault(l => l.UserId == userId);

        if (existingLike is null)
        {
            var newLike = new Like
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IdeaId = ideaId
            };
            await _likeRepository.AddAsync(newLike);
        }
        else
        {
            await _likeRepository.DeleteAsync(existingLike);
        }
    }
}