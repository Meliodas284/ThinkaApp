using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thinka.Domain.Dto;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FeedController : ControllerBase
{
    private readonly IFeedService _feedService;

    public FeedController(IFeedService feedService)
    {
        _feedService = feedService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFeed([FromQuery] PaginationQuery query)
    {
        var ideas = await _feedService.GetFeedAsync(query);
        return Ok(ideas);
    }
}
