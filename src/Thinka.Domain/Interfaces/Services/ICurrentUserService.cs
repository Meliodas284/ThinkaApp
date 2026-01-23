using System;

namespace Thinka.Domain.Interfaces.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }
}
