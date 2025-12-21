using System.Security.Claims;
using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.Services;

public interface ITokenService
{
    string CreateAccessToken(User user);
    string CreateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    void UpdateUserRefreshToken(User user, string refreshToken);
}
