using Thinka.Domain.Dto;
using Thinka.Domain.Dto.User;
using Thinka.Domain.Entities;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class FollowService : IFollowService
{
    private readonly IFollowRepository _followRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public FollowService(
        IFollowRepository followRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _followRepository = followRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task FollowAsync(Guid targetUserId)
    {
        var currentUserId = _currentUserService.UserId;

        if (currentUserId == targetUserId)
        {
            throw new ForbiddenException("User cannot follow themselves.");
        }

        var targetUser = await _userRepository.GetByIdAsync(targetUserId);
        if (targetUser is null)
        {
            throw new NotFoundException("User not found");
        }

        var existingFollow = await _followRepository.GetAsync(currentUserId, targetUserId);
        if (existingFollow is not null)
        {
            throw new ConflictException("User is already followed.");
        }

        await _followRepository.AddAsync(new Follow
        {
            FollowerId = currentUserId,
            FollowingId = targetUserId
        });

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UnfollowAsync(Guid targetUserId)
    {
        var targetUser = await _userRepository.GetByIdAsync(targetUserId);
        if (targetUser is null)
        {
            throw new NotFoundException("User not found");
        }

        var currentUserId = _currentUserService.UserId;
        var follow = await _followRepository.GetAsync(currentUserId, targetUserId);

        if (follow is null)
        {
            return;
        }

        await _followRepository.DeleteAsync(follow);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<FollowUserDto>> GetFollowersAsync(Guid userId, PaginationQuery query)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var followers = await _followRepository.GetFollowersAsync(userId, query.Page, query.PageSize);

        return followers.Select(user => new FollowUserDto
        {
            Id = user.Id,
            UserName = user.UserName
        }).ToList();
    }

    public async Task<List<FollowUserDto>> GetFollowingAsync(Guid userId, PaginationQuery query)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var following = await _followRepository.GetFollowingAsync(userId, query.Page, query.PageSize);

        return following.Select(user => new FollowUserDto
        {
            Id = user.Id,
            UserName = user.UserName
        }).ToList();
    }
}
