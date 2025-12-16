using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Thinka.DAL;

public class ThinkaDbContext : DbContext
{
    public ThinkaDbContext(DbContextOptions<ThinkaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
