using DetectiveRoslynIO.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DetectiveRoslynIO.Data.Configurations;

public class ChallengeConfiguration : IEntityTypeConfiguration<Challenge>
{
    public void Configure(EntityTypeBuilder<Challenge> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.Instructions)
            .IsRequired();

        builder.Property(c => c.TargetRepoUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.ExpectedAnswer)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.AnswerFormat)
            .HasMaxLength(50);

        builder.Property(c => c.RoslynDocsUrl)
            .HasMaxLength(500);

        builder.HasMany(c => c.Hints)
            .WithOne(h => h.Challenge)
            .HasForeignKey(h => h.ChallengeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Submissions)
            .WithOne(s => s.Challenge)
            .HasForeignKey(s => s.ChallengeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.UserProgress)
            .WithOne(up => up.Challenge)
            .HasForeignKey(up => up.ChallengeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.OrderIndex);
        builder.HasIndex(c => c.Category);
        builder.HasIndex(c => c.Difficulty);
        builder.HasIndex(c => new { c.TrackId, c.SequenceNumber });
    }
}
