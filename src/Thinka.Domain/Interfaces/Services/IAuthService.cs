using Thinka.Domain.Dto;
using Thinka.Domain.Dto.User;

namespace Thinka.Domain.Interfaces.Services;

public interface IAuthService
{
    Task RegisterAsync(UserRegisterDto userRegisterDto);
    Task<TokenDto> LoginAsync(UserLoginDto userLoginDto);
    Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);
}
