using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thinka.Domain.Entities;

namespace Thinka.DAL.Configurations;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(l => l.Id);
        
        builder.HasIndex(l => new { l.UserId, l.IdeaId }).IsUnique();

        builder.HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Idea)
            .WithMany(i => i.Likes)
            .HasForeignKey(l => l.IdeaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}