using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thinka.Domain.Dto;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IFollowService _followService;

    public UsersController(IUserService userService, IFollowService followService)
    {
        _userService = userService;
        _followService = followService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userProfile = await _userService.GetUserProfileAsync();
        return Ok(userProfile);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserProfile(Guid id)
    {
        var userProfile = await _userService.GetUserProfileAsync(id);
        return Ok(userProfile);
    }

    [HttpPost("{id:guid}/follow")]
    public async Task<IActionResult> Follow(Guid id)
    {
        await _followService.FollowAsync(id);
        return NoContent();
    }

    [HttpDelete("{id:guid}/follow")]
    public async Task<IActionResult> Unfollow(Guid id)
    {
        await _followService.UnfollowAsync(id);
        return NoContent();
    }

    [HttpGet("{id:guid}/followers")]
    public async Task<IActionResult> GetFollowers(Guid id, [FromQuery] PaginationQuery query)
    {
        var followers = await _followService.GetFollowersAsync(id, query);
        return Ok(followers);
    }

    [HttpGet("{id:guid}/following")]
    public async Task<IActionResult> GetFollowing(Guid id, [FromQuery] PaginationQuery query)
    {
        var following = await _followService.GetFollowingAsync(id, query);
        return Ok(following);
    }
}
