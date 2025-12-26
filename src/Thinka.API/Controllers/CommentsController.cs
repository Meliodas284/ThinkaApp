using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thinka.Domain.Dto;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.API.Controllers;

[ApiController]
[Route("api/comments")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto createCommentDto)
    {
        await _commentService.CreateComment(createCommentDto);
        return Ok();
    }

    [HttpGet("/api/ideas/{ideaId}/comments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments(Guid ideaId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var comments = await _commentService.GetComments(ideaId, page, pageSize);
        return Ok(comments);
    }
}