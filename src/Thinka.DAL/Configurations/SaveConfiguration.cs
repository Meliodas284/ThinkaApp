using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thinka.Domain.Entities;

namespace Thinka.DAL.Configurations;

public class SaveConfiguration : IEntityTypeConfiguration<Save>
{
    public void Configure(EntityTypeBuilder<Save> builder)
    {
        builder.HasKey(s => new { s.UserId, s.IdeaId });

        builder.HasOne(s => s.User)
            .WithMany(u => u.Saves)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Idea)
            .WithMany(i => i.Saves)
            .HasForeignKey(s => s.IdeaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
