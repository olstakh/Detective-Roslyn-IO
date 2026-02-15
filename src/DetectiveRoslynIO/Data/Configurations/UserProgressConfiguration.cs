using DetectiveRoslynIO.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DetectiveRoslynIO.Data.Configurations;

public class UserProgressConfiguration : IEntityTypeConfiguration<UserProgress>
{
    public void Configure(EntityTypeBuilder<UserProgress> builder)
    {
        builder.HasKey(up => up.Id);

        builder.HasOne(up => up.User)
            .WithMany(u => u.Progress)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(up => up.Challenge)
            .WithMany(c => c.UserProgress)
            .HasForeignKey(up => up.ChallengeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(up => new { up.UserId, up.ChallengeId })
            .IsUnique();

        builder.HasIndex(up => up.IsCompleted);
    }
}
