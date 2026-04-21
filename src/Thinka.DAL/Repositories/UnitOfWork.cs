using Microsoft.EntityFrameworkCore.Storage;
using Thinka.Domain.Interfaces.Repositories;

namespace Thinka.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ThinkaDbContext _context;

    public UnitOfWork(ThinkaDbContext context)
    {
        _context = context;
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}