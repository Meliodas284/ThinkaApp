using Thinka.Domain.Dto.User;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IIdeaRepository _ideaRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly ISaveRepository _saveRepository;
    private readonly IFollowRepository _followRepository;
    private readonly ICurrentUserService _currentUserService;

    public UserService(
        IUserRepository userRepository,
        IIdeaRepository ideaRepository,
        ILikeRepository likeRepository,
        ISaveRepository saveRepository,
        IFollowRepository followRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _ideaRepository = ideaRepository;
        _likeRepository = likeRepository;
        _saveRepository = saveRepository;
        _followRepository = followRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UserProfileDto> GetUserProfileAsync(Guid? userId = null)
    {
        var finalUserId = userId ?? _currentUserService.UserId;
        
        var user = await _userRepository.GetByIdAsync(finalUserId);

        if (user is null)
            throw new NotFoundException("User not found");

        var ideasCount = await _ideaRepository.CountByAuthorIdAsync(user.Id);
        var likesCount = await _likeRepository.CountLikesByAuthorIdAsync(user.Id);
        var savesCount = await _saveRepository.CountSavesByUserIdAsync(user.Id);
        var followersCount = await _followRepository.CountFollowersAsync(user.Id);
        var followingCount = await _followRepository.CountFollowingAsync(user.Id);

        return new UserProfileDto
        {
            UserName = user.UserName,
            Email = user.Email,
            IdeasCount = ideasCount,
            LikesCount = likesCount,
            SavesCount = savesCount,
            FollowersCount = followersCount,
            FollowingCount = followingCount
        };
    }
}
