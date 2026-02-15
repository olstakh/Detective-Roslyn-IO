using DetectiveRoslynIO.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DetectiveRoslynIO.Data.Configurations;

public class UserSubmissionConfiguration : IEntityTypeConfiguration<UserSubmission>
{
    public void Configure(EntityTypeBuilder<UserSubmission> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.SubmittedAnswer)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(s => s.User)
            .WithMany(u => u.Submissions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Challenge)
            .WithMany(c => c.Submissions)
            .HasForeignKey(s => s.ChallengeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => s.UserId);
        builder.HasIndex(s => s.ChallengeId);
        builder.HasIndex(s => s.SubmittedAt);
    }
}
