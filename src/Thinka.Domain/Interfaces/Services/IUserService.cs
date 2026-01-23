using Thinka.Domain.Dto.User;

namespace Thinka.Domain.Interfaces.Services;

public interface IUserService
{
    Task<UserProfileDto> GetUserProfileAsync(Guid? userId = null);
}