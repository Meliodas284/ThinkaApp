using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thinka.Domain.Dto;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IdeasController : ControllerBase
{
    private readonly IIdeaService _ideaService;
    private readonly ILikeService _likeService;
    private readonly ICommentService _commentService;
    private readonly ISaveService _saveService;

    public IdeasController(IIdeaService ideaService, ILikeService likeService, ICommentService commentService, ISaveService saveService)
    {
        _ideaService = ideaService;
        _likeService = likeService;
        _commentService = commentService;
        _saveService = saveService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateIdea([FromBody] CreateIdeaDto createIdeaDto)
    {
        var newIdea = await _ideaService.CreateIdeaAsync(createIdeaDto);
        return CreatedAtAction(nameof(GetIdea), new { id = newIdea.Id }, newIdea);
    }
    
    [HttpPost("{id:guid}/like")]
    public async Task<IActionResult> ToggleLike(Guid id)
    {
        try
        {
            await _likeService.ToggleLikeAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpPost("{id:guid}/save")]
    public async Task<IActionResult> ToggleSave(Guid id)
    {
        try
        {
            await _saveService.ToggleSaveAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpGet("saves")]
    public async Task<IActionResult> GetSavedIdeas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var ideas = await _saveService.GetSavedIdeasAsync(pageNumber, pageSize);
        return Ok(ideas);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserIdeas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var ideas = await _ideaService.GetUserIdeasAsync(pageNumber, pageSize);
        return Ok(ideas);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetIdea(Guid id)
    {
        var idea = await _ideaService.GetIdeaByIdAsync(id);
        if (idea == null)
        {
            return NotFound();
        }
        return Ok(idea);
    }
    
    [HttpGet("{ideaId:guid}/comments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments(Guid ideaId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var comments = await _commentService.GetComments(ideaId, page, pageSize);
        return Ok(comments);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateIdea(Guid id, [FromBody] UpdateIdeaDto updateIdeaDto)
    {
        try
        {
            await _ideaService.UpdateIdeaAsync(id, updateIdeaDto);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteIdea(Guid id)
    {
        try
        {
            await _ideaService.DeleteIdeaAsync(id);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
