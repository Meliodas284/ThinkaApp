using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Thinka.Domain.Entities;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;
    private readonly IIdeaRepository _ideaRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LikeService(ILikeRepository likeRepository, IIdeaRepository ideaRepository, IHttpContextAccessor httpContextAccessor)
    {
        _likeRepository = likeRepository;
        _ideaRepository = ideaRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task ToggleLikeAsync(Guid ideaId)
    {
        var userId = GetCurrentUserId();
        
        var idea = await _ideaRepository.GetByIdWithLikesAsync(ideaId);
        if (idea is null)
        {
            throw new Exception("Idea not found");
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