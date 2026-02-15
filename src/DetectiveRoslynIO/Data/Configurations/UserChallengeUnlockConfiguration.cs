using DetectiveRoslynIO.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DetectiveRoslynIO.Data.Configurations;

public class UserChallengeUnlockConfiguration : IEntityTypeConfiguration<UserChallengeUnlock>
{
    public void Configure(EntityTypeBuilder<UserChallengeUnlock> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Challenge)
            .WithMany()
            .HasForeignKey(u => u.ChallengeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint: each user can only unlock a challenge once
        builder.HasIndex(u => new { u.UserId, u.ChallengeId })
            .IsUnique();

        builder.HasIndex(u => u.UserId);
        builder.HasIndex(u => u.ChallengeId);
        builder.HasIndex(u => u.UnlockedAt);
    }
}
