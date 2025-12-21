using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Thinka.Domain.Entities;

namespace Thinka.DAL;

public class ThinkaDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ThinkaDbContext(DbContextOptions<ThinkaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
