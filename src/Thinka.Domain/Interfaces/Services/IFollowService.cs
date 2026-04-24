using Thinka.Domain.Dto;
using Thinka.Domain.Dto.User;

namespace Thinka.Domain.Interfaces.Services;

public interface IFollowService
{
    Task FollowAsync(Guid targetUserId);
    Task UnfollowAsync(Guid targetUserId);
    Task<List<FollowUserDto>> GetFollowersAsync(Guid userId, PaginationQuery query);
    Task<List<FollowUserDto>> GetFollowingAsync(Guid userId, PaginationQuery query);
}
