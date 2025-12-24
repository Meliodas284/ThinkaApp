using System;
using System.Threading.Tasks;
using Thinka.Domain.Entities;

namespace Thinka.Domain.Interfaces.Repositories;

public interface ILikeRepository
{
    Task AddAsync(Like like);
    
    Task DeleteAsync(Like like);
}
