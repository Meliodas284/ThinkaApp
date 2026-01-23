using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thinka.Domain.Dto.Ideas;
using Thinka.Domain.Enums;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.API.Controllers;

/// <summary>
///     Контроллер для работы с идеями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IdeasController : ControllerBase
{
    private readonly IIdeaService _ideaService;
    private readonly ILikeService _likeService;
    private readonly ICommentService _commentService;
    private readonly ISaveService _saveService;
    private readonly IIdeasSearchService _ideasSearchService;

    public IdeasController(
        IIdeaService ideaService, 
        ILikeService likeService, 
        ICommentService commentService, 
        ISaveService saveService, 
        IIdeasSearchService ideasSearchService)
    {
        _ideaService = ideaService;
        _likeService = likeService;
        _commentService = commentService;
        _saveService = saveService;
        _ideasSearchService = ideasSearchService;
    }

    /// <summary>
    ///     Создать свою идею.
    /// </summary>
    /// <param name="createIdeaDto">Дто создания идеи.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateIdea([FromBody] CreateIdeaDto createIdeaDto)
    {
        var newIdea = await _ideaService.CreateIdeaAsync(createIdeaDto);
        return CreatedAtAction(nameof(GetIdea), new { id = newIdea.Id }, newIdea);
    }
    
    /// <summary>
    ///     Поставить или убрать лайк для идеи.
    /// </summary>
    /// <remarks>
    ///     Если лайк уже был поставлен, то повторный запрос его уберет.
    /// </remarks>
    /// <param name="id">Идентификатор идеи.</param>
    /// <returns></returns>
    [HttpPost("{id:guid}/like")]
    public async Task<IActionResult> ToggleLike(Guid id)
    {
        await _likeService.ToggleLikeAsync(id);
        return Ok();
    }

    /// <summary>
    ///     Добавить идею в сохраненные.
    /// </summary>
    /// <remarks>
    ///     Если идея уже была сохранена, то повторный запрос ее удалит из сохраненных.
    /// </remarks>
    /// <param name="id">Идентификатор идеи.</param>
    /// <returns></returns>
    [HttpPost("{id:guid}/save")]
    public async Task<IActionResult> ToggleSave(Guid id)
    {
        await _saveService.ToggleSaveAsync(id);
        return Ok();
    }
    
    /// <summary>
    ///     Получить свои идеи из сохраненных.
    /// </summary>
    /// <param name="pageNumber">Номер страницы.</param>
    /// <param name="pageSize">Количество элементов на страницу.</param>
    /// <returns>Список сохраненных идей.</returns>
    [HttpGet("saves")]
    public async Task<IActionResult> GetSavedIdeas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var ideas = await _saveService.GetSavedIdeasAsync(pageNumber, pageSize);
        return Ok(ideas);
    }

    /// <summary>
    ///     Получить свои идеи.
    /// </summary>
    /// <param name="pageNumber">Номер страницы.</param>
    /// <param name="pageSize">Количество элементов на страницу.</param>
    /// <returns>Список своих идей.</returns>
    [HttpGet]
    public async Task<IActionResult> GetUserIdeas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var ideas = await _ideaService.GetUserIdeasAsync(pageNumber, pageSize);
        return Ok(ideas);
    }

    /// <summary>
    ///     Получить идеи определенного пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="pageNumber">Номер страницы.</param>
    /// <param name="pageSize">Количество элементов на страницу.</param>
    /// <returns>Список идей определенного пользователя.</returns>
    [HttpGet("user/{userId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserIdeas(Guid userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var ideas = await _ideaService.GetUserIdeasAsync(pageNumber, pageSize, userId);
        return Ok(ideas);
    }

    /// <summary>
    ///     Получить идею по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор идеи.</param>
    /// <returns>Идея с указанным идентификатором.</returns>
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
    
    /// <summary>
    ///     Получить комментарии к определенной идее.
    /// </summary>
    /// <param name="ideaId">Идентификатор идеи.</param>
    /// <param name="page">Номер страницы.</param>
    /// <param name="pageSize">Количество элементов на страницу.</param>
    /// <returns>Список комментариев для определенной идее.</returns>
    [HttpGet("{ideaId:guid}/comments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments(Guid ideaId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var comments = await _commentService.GetComments(ideaId, page, pageSize);
        return Ok(comments);
    }

    /// <summary>
    ///     Обновить идею.
    /// </summary>
    /// <param name="id">Идентификатор идеи.</param>
    /// <param name="updateIdeaDto">Дто изменения идеи.</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateIdea(Guid id, [FromBody] UpdateIdeaDto updateIdeaDto)
    {
        await _ideaService.UpdateIdeaAsync(id, updateIdeaDto);
        return NoContent();
    }

    /// <summary>
    ///     Удалить идею.
    /// </summary>
    /// <param name="id">Идентификатор удаляемой идеи.</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteIdea(Guid id)
    {
        await _ideaService.DeleteIdeaAsync(id);
        return NoContent();
    }

    /// <summary>
    ///     Поиск идей по запросу.
    /// </summary>
    /// <param name="query">Текстовый запрос для поиска.</param>
    /// <param name="category">Категория идей, среди которых искать.</param>
    /// <param name="authorId">Идентификатор пользователя, среди идей которого искать.</param>
    /// <param name="page">Номер страницы.</param>
    /// <param name="pageSize">Количество элементов на страницу.</param>
    /// <returns>Список идей, соответствующих поиску.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchIdeas(
    [FromQuery] string query,
    [FromQuery] Category? category,
    [FromQuery] Guid? authorId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        var result = await _ideasSearchService.SearchAsync(new SearchIdeasQueryDto
        {
            Query = query,
            Category = category,
            AuthorId = authorId,
            Page = page,
            PageSize = pageSize
        });

        return Ok(result);
    }
}
