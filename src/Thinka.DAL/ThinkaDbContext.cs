using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Thinka.Domain.Entities;

namespace Thinka.DAL;

public class ThinkaDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Idea> Ideas { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Save> Saves { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }

    public ThinkaDbContext(DbContextOptions<ThinkaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
