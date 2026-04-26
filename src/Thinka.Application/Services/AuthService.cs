using System.Security.Claims;
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
        var normalizedEmail = NormalizeEmail(userRegisterDto.Email);
        var userName = userRegisterDto.UserName.Trim();

        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("Username cannot be empty.");
        }

        if (await _userRepository.IsEmailTakenAsync(normalizedEmail))
        {
            throw new ConflictException("Email is already taken.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = normalizedEmail,
            UserName = userName
        };

        user.PasswordHash = BC.HashPassword(userRegisterDto.Password);
        
        var refreshToken = _tokenService.CreateRefreshToken();
        _tokenService.UpdateUserRefreshToken(user, refreshToken);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<TokenDto> LoginAsync(UserLoginDto userLoginDto)
    {
        var normalizedEmail = NormalizeEmail(userLoginDto.Email);
        var user = await _userRepository.GetByEmailAsync(normalizedEmail);

        if (user == null || !BC.Verify(userLoginDto.Password, user.PasswordHash))
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
        if (string.IsNullOrWhiteSpace(userEmail))
        {
            throw new SecurityTokenException("Invalid access token.");
        }

        var user = await _userRepository.GetByEmailAsync(NormalizeEmail(userEmail));

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

    private static string NormalizeEmail(string email)
    {
        var normalizedEmail = email.Trim();
        if (string.IsNullOrWhiteSpace(normalizedEmail))
        {
            throw new ArgumentException("Email cannot be empty.");
        }

        return normalizedEmail;
    }
}