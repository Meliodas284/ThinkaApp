using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Thinka.Domain.Dto;
using Thinka.Domain.Dto.User;
using Thinka.Domain.Entities;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;
using BC = BCrypt.Net.BCrypt;

namespace Thinka.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository, 
        ITokenService tokenService, 
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task RegisterAsync(UserRegisterDto userRegisterDto)
    {
        if (await _userRepository.IsEmailTakenAsync(userRegisterDto.Email))
        {
            throw new ConflictException("Email is already taken.");
        }

        var user = new User
        {
            Email = userRegisterDto.Email,
            UserName = userRegisterDto.UserName,
        };

        (user.PasswordHash, user.PasswordSalt) = CreatePasswordHash(userRegisterDto.Password);
        
        var refreshToken = _tokenService.CreateRefreshToken();
        _tokenService.UpdateUserRefreshToken(user, refreshToken);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<TokenDto> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);

        if (user == null || !VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new UnauthorizedException("Invalid credentials.");
        }
        
        var refreshToken = _tokenService.CreateRefreshToken();
        _tokenService.UpdateUserRefreshToken(user, refreshToken);

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new TokenDto
        {
            AccessToken = _tokenService.CreateAccessToken(user),
            RefreshToken = refreshToken
        };
    }
    
    public async Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        var userEmail = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        var user = await _userRepository.GetByEmailAsync(userEmail!);

        if (user is null || user.RefreshToken != tokenDto.RefreshToken || user.TokenExpires <= DateTime.UtcNow)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        var newAccessToken = _tokenService.CreateAccessToken(user);
        var newRefreshToken = _tokenService.CreateRefreshToken();
        
        _tokenService.UpdateUserRefreshToken(user, newRefreshToken);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        return new TokenDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    private (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
    {
        var salt = BC.GenerateSalt(12);
        var hash = BC.HashPassword(password, salt);
        return (Encoding.UTF8.GetBytes(hash), Encoding.UTF8.GetBytes(salt));
    }

    private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        var salt = Encoding.UTF8.GetString(storedSalt);
        var hash = Encoding.UTF8.GetString(storedHash);
        return BC.Verify(password, hash);
    }
}
