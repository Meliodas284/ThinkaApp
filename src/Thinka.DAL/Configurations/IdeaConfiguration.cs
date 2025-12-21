using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thinka.Domain.Entities;
using Thinka.Domain.Enums;

namespace Thinka.DAL.Configurations;

public class IdeaConfiguration : IEntityTypeConfiguration<Idea>
{
    public void Configure(EntityTypeBuilder<Idea> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.ShortDescription)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(i => i.FullDescription)
            .IsRequired();
            
        builder.Property(i => i.Category)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (Category)Enum.Parse(typeof(Category), v));

        builder.HasOne(i => i.Author)
            .WithMany()
            .HasForeignKey(i => i.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
