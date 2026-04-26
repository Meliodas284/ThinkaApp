using Microsoft.AspNetCore.Mvc;
using Thinka.Domain.Dto;
using Thinka.Domain.Dto.User;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        await _authService.RegisterAsync(userRegisterDto);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var tokenDto = await _authService.LoginAsync(userLoginDto);
        return Ok(tokenDto);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
    {
        var resultTokenDto = await _authService.RefreshTokenAsync(tokenDto);
        return Ok(resultTokenDto);
    }
}

