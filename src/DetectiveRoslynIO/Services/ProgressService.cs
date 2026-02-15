using DetectiveRoslynIO.Data;
using DetectiveRoslynIO.Data.Entities;
using DetectiveRoslynIO.Models;
using Microsoft.EntityFrameworkCore;

namespace DetectiveRoslynIO.Services;

public class ProgressService : IProgressService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly IUnlockService _unlockService;

    public ProgressService(IDbContextFactory<ApplicationDbContext> contextFactory, IUnlockService unlockService)
    {
        _contextFactory = contextFactory;
        _unlockService = unlockService;
    }

    public async Task<UserProgressViewModel> GetUserProgressAsync(string userId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var challenges = await context.Challenges
            .Where(c => c.IsActive)
            .OrderBy(c => c.OrderIndex)
            .ToListAsync();

        var userProgress = await context.UserProgress
            .Where(up => up.UserId == userId)
            .ToListAsync();

        var challengeProgressList = challenges.Select(challenge =>
        {
            var progress = userProgress.FirstOrDefault(up => up.ChallengeId == challenge.Id);
            return new ChallengeProgress
            {
                Challenge = challenge,
                IsCompleted = progress?.IsCompleted ?? false,
                CompletedAt = progress?.CompletedAt,
                TotalAttempts = progress?.TotalAttempts ?? 0,
                HintsUsed = progress?.HintsUsed ?? 0
            };
        }).ToList();

        return new UserProgressViewModel
        {
            TotalChallenges = challenges.Count,
            CompletedChallenges = userProgress.Count(up => up.IsCompleted),
            Challenges = challengeProgressList
        };
    }

    public async Task<UserProgress?> GetChallengeProgressAsync(string userId, int challengeId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.UserProgress
            .FirstOrDefaultAsync(up => up.UserId == userId && up.ChallengeId == challengeId);
    }

    public async Task<bool> MarkChallengeCompletedAsync(string userId, int challengeId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var progress = await context.UserProgress
            .FirstOrDefaultAsync(up => up.UserId == userId && up.ChallengeId == challengeId);

        if (progress == null)
        {
            progress = new UserProgress
            {
                UserId = userId,
                ChallengeId = challengeId,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow,
                StartedAt = DateTime.UtcNow,
                TotalAttempts = 1,
                HintsUsed = 0
            };
            context.UserProgress.Add(progress);
            await context.SaveChangesAsync();

            // Trigger next challenge unlock
            await _unlockService.UnlockNextChallengeAsync(userId, challengeId);

            return true;
        }

        if (!progress.IsCompleted)
        {
            progress.IsCompleted = true;
            progress.CompletedAt = DateTime.UtcNow;
            progress.TotalAttempts++;
            await context.SaveChangesAsync();

            // Trigger next challenge unlock
            await _unlockService.UnlockNextChallengeAsync(userId, challengeId);

            return true;
        }

        return false;
    }

    public async Task UpdateProgressAttemptsAsync(string userId, int challengeId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var progress = await context.UserProgress
            .FirstOrDefaultAsync(up => up.UserId == userId && up.ChallengeId == challengeId);

        if (progress == null)
        {
            progress = new UserProgress
            {
                UserId = userId,
                ChallengeId = challengeId,
                IsCompleted = false,
                StartedAt = DateTime.UtcNow,
                TotalAttempts = 1,
                HintsUsed = 0
            };
            context.UserProgress.Add(progress);
        }
        else
        {
            progress.TotalAttempts++;
        }

        await context.SaveChangesAsync();
    }

    public async Task IncrementHintUsageAsync(string userId, int challengeId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var progress = await context.UserProgress
            .FirstOrDefaultAsync(up => up.UserId == userId && up.ChallengeId == challengeId);

        if (progress == null)
        {
            progress = new UserProgress
            {
                UserId = userId,
                ChallengeId = challengeId,
                IsCompleted = false,
                StartedAt = DateTime.UtcNow,
                TotalAttempts = 0,
                HintsUsed = 1
            };
            context.UserProgress.Add(progress);
        }
        else
        {
            progress.HintsUsed++;
        }

        await context.SaveChangesAsync();
    }
}
