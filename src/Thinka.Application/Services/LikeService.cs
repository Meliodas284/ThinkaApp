using Thinka.Domain.Entities;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;
    private readonly IIdeaRepository _ideaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public LikeService(
        ILikeRepository likeRepository, 
        IIdeaRepository ideaRepository, 
        ICurrentUserService currentUserService, 
        IUnitOfWork unitOfWork)
    {
        _likeRepository = likeRepository;
        _ideaRepository = ideaRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task ToggleLikeAsync(Guid ideaId)
    {
        var userId = _currentUserService.UserId;

        if (!await _ideaRepository.ExistsAsync(ideaId))
        {
            throw new NotFoundException("Idea not found");
        }

        var existingLike = await _likeRepository.GetByIdeaAndUserAsync(ideaId, userId);

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

        await _unitOfWork.SaveChangesAsync();
    }
}
