using DetectiveRoslynIO.Data;
using DetectiveRoslynIO.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DetectiveRoslynIO.Services;

public class UnlockService : IUnlockService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public UnlockService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<Challenge>> GetUnlockedChallengesAsync(string userId, int? trackId = null)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var unlocksQuery = context.UserChallengeUnlocks
            .Include(u => u.Challenge)
                .ThenInclude(c => c.Track)
            .Where(u => u.UserId == userId);

        if (trackId.HasValue)
        {
            unlocksQuery = unlocksQuery.Where(u => u.Challenge.TrackId == trackId.Value);
        }

        var unlocks = await unlocksQuery.ToListAsync();

        return unlocks
            .Select(u => u.Challenge)
            .Where(c => c.IsActive)
            .OrderBy(c => c.TrackId)
            .ThenBy(c => c.SequenceNumber)
            .ToList();
    }

    public async Task<bool> IsChallengeUnlockedAsync(string userId, int challengeId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.UserChallengeUnlocks
            .AnyAsync(u => u.UserId == userId && u.ChallengeId == challengeId);
    }

    public async Task<bool> UnlockChallengeAsync(string userId, int challengeId, bool isAutoUnlock = false)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        // Check if already unlocked
        var existingUnlock = await context.UserChallengeUnlocks
            .FirstOrDefaultAsync(u => u.UserId == userId && u.ChallengeId == challengeId);

        if (existingUnlock != null)
        {
            return false; // Already unlocked
        }

        // Create unlock record
        var unlock = new UserChallengeUnlock
        {
            UserId = userId,
            ChallengeId = challengeId,
            UnlockedAt = DateTime.UtcNow,
            IsAutoUnlocked = isAutoUnlock
        };

        context.UserChallengeUnlocks.Add(unlock);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task UnlockNextChallengeAsync(string userId, int completedChallengeId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        // Get the completed challenge
        var completedChallenge = await context.Challenges
            .FirstOrDefaultAsync(c => c.Id == completedChallengeId);

        if (completedChallenge == null || !completedChallenge.TrackId.HasValue)
        {
            return; // No track associated, nothing to unlock
        }

        // Find the next challenge in the same track
        var nextChallenge = await context.Challenges
            .Where(c => c.TrackId == completedChallenge.TrackId
                     && c.SequenceNumber == completedChallenge.SequenceNumber + 1
                     && c.IsActive)
            .FirstOrDefaultAsync();

        if (nextChallenge != null)
        {
            await UnlockChallengeAsync(userId, nextChallenge.Id, isAutoUnlock: true);
        }
    }

    public async Task UnlockFirstChallengesForNewUserAsync(string userId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        // Get all first challenges in each active track
        var firstChallenges = await context.Challenges
            .Where(c => c.SequenceNumber == 1 && c.TrackId.HasValue && c.IsActive)
            .ToListAsync();

        foreach (var challenge in firstChallenges)
        {
            await UnlockChallengeAsync(userId, challenge.Id, isAutoUnlock: true);
        }
    }

    public async Task<Dictionary<int, bool>> GetUnlockStatusMapAsync(string userId, int trackId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var challenges = await context.Challenges
            .Where(c => c.TrackId == trackId && c.IsActive)
            .Select(c => c.Id)
            .ToListAsync();

        var unlockedChallengeIds = await context.UserChallengeUnlocks
            .Where(u => u.UserId == userId && challenges.Contains(u.ChallengeId))
            .Select(u => u.ChallengeId)
            .ToListAsync();

        return challenges.ToDictionary(
            challengeId => challengeId,
            challengeId => unlockedChallengeIds.Contains(challengeId)
        );
    }
}
