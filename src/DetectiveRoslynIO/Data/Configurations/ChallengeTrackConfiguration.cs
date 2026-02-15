using DetectiveRoslynIO.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DetectiveRoslynIO.Data.Configurations;

public class ChallengeTrackConfiguration : IEntityTypeConfiguration<ChallengeTrack>
{
    public void Configure(EntityTypeBuilder<ChallengeTrack> builder)
    {
        builder.HasKey(ct => ct.Id);

        builder.Property(ct => ct.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ct => ct.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(ct => ct.IconClass)
            .HasMaxLength(100);

        builder.HasMany(ct => ct.Challenges)
            .WithOne(c => c.Track)
            .HasForeignKey(c => c.TrackId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ct => ct.OrderIndex);
        builder.HasIndex(ct => ct.IsActive);
    }
}
