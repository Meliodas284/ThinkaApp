using System;
using System.Threading.Tasks;

namespace Thinka.Domain.Interfaces.Services;

public interface ILikeService
{
    Task ToggleLikeAsync(Guid ideaId);
}
