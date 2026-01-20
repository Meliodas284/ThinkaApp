using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var userIdValue = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                throw new UnauthorizedException("User ID not found or invalid in token.");
            }
            return userId;
        }
    }
}
