using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Thinka.Domain.Dto;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IIdeaRepository _ideaRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly ISaveRepository _saveRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IUserRepository userRepository, IIdeaRepository ideaRepository, ILikeRepository likeRepository, ISaveRepository saveRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _ideaRepository = ideaRepository;
        _likeRepository = likeRepository;
        _saveRepository = saveRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserProfileDto> GetUserProfileAsync(Guid? userId = null)
    {
        var finalUserId = userId ?? GetCurrentUserId();
        
        var user = await _userRepository.GetByIdAsync(finalUserId);

        if (user is null)
            throw new Exception("User not found");

        var ideasCount = await _ideaRepository.CountByAuthorIdAsync(user.Id);
        var likesCount = await _likeRepository.CountLikesByAuthorIdAsync(user.Id);
        var savesCount = await _saveRepository.CountSavesByUserIdAsync(user.Id);

        return new UserProfileDto
        {
            UserName = user.UserName,
            Email = user.Email,
            IdeasCount = ideasCount,
            LikesCount = likesCount,
            SavesCount = savesCount
        };
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