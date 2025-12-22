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

    public IdeasController(IIdeaService ideaService)
    {
        _ideaService = ideaService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateIdea([FromBody] CreateIdeaDto createIdeaDto)
    {
        var newIdea = await _ideaService.CreateIdeaAsync(createIdeaDto);
        return CreatedAtAction(nameof(GetIdea), new { id = newIdea.Id }, newIdea);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserIdeas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var ideas = await _ideaService.GetUserIdeasAsync(pageNumber, pageSize);
        return Ok(ideas);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetIdea(Guid id)
    {
        var idea = await _ideaService.GetIdeaByIdAsync(id);
        if (idea == null)
        {
            return NotFound();
        }
        return Ok(idea);
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
