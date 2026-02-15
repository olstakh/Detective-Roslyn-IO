using DetectiveRoslynIO.Data.Configurations;
using DetectiveRoslynIO.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DetectiveRoslynIO.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Challenge> Challenges => Set<Challenge>();
    public DbSet<Hint> Hints => Set<Hint>();
    public DbSet<UserSubmission> UserSubmissions => Set<UserSubmission>();
    public DbSet<UserProgress> UserProgress => Set<UserProgress>();
    public DbSet<ChallengeTrack> ChallengeTracks => Set<ChallengeTrack>();
    public DbSet<UserChallengeUnlock> UserChallengeUnlocks => Set<UserChallengeUnlock>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ChallengeConfiguration());
        builder.ApplyConfiguration(new UserSubmissionConfiguration());
        builder.ApplyConfiguration(new UserProgressConfiguration());
        builder.ApplyConfiguration(new ChallengeTrackConfiguration());
        builder.ApplyConfiguration(new UserChallengeUnlockConfiguration());
    }
}
