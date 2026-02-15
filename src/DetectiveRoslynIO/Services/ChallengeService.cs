using DetectiveRoslynIO.Data;
using DetectiveRoslynIO.Data.Entities;
using DetectiveRoslynIO.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DetectiveRoslynIO.Services;

public class ChallengeService : IChallengeService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public ChallengeService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<Challenge>> GetAllChallengesAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Challenges
            .Include(c => c.Hints)
            .OrderBy(c => c.OrderIndex)
            .ToListAsync();
    }

    public async Task<List<Challenge>> GetActiveChallengesAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Challenges
            .Include(c => c.Hints)
            .Where(c => c.IsActive)
            .OrderBy(c => c.OrderIndex)
            .ToListAsync();
    }

    public async Task<List<Challenge>> GetChallengesByCategoryAsync(ChallengeCategory category)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Challenges
            .Include(c => c.Hints)
            .Where(c => c.Category == category && c.IsActive)
            .OrderBy(c => c.OrderIndex)
            .ToListAsync();
    }

    public async Task<List<Challenge>> GetChallengesByDifficultyAsync(DifficultyLevel difficulty)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Challenges
            .Include(c => c.Hints)
            .Where(c => c.Difficulty == difficulty && c.IsActive)
            .OrderBy(c => c.OrderIndex)
            .ToListAsync();
    }

    public async Task<Challenge?> GetChallengeByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Challenges
            .Include(c => c.Hints.OrderBy(h => h.OrderIndex))
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Challenge> CreateChallengeAsync(Challenge challenge)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Challenges.Add(challenge);
        await context.SaveChangesAsync();
        return challenge;
    }

    public async Task<Challenge> UpdateChallengeAsync(Challenge challenge)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Challenges.Update(challenge);
        await context.SaveChangesAsync();
        return challenge;
    }

    public async Task DeleteChallengeAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var challenge = await context.Challenges.FindAsync(id);
        if (challenge != null)
        {
            context.Challenges.Remove(challenge);
            await context.SaveChangesAsync();
        }
    }

    public async Task<List<Hint>> GetChallengeHintsAsync(int challengeId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Hints
            .Where(h => h.ChallengeId == challengeId)
            .OrderBy(h => h.OrderIndex)
            .ToListAsync();
    }

    public async Task<List<ChallengeTrack>> GetActiveTracksAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.ChallengeTracks
            .Where(t => t.IsActive)
            .OrderBy(t => t.OrderIndex)
            .Include(t => t.Challenges)
            .ToListAsync();
    }

    public async Task<ChallengeTrack?> GetTrackByIdAsync(int trackId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.ChallengeTracks
            .Include(t => t.Challenges.OrderBy(c => c.SequenceNumber))
            .FirstOrDefaultAsync(t => t.Id == trackId);
    }

    public async Task<List<Challenge>> GetChallengesByTrackAsync(int trackId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Challenges
            .Where(c => c.TrackId == trackId && c.IsActive)
            .OrderBy(c => c.SequenceNumber)
            .Include(c => c.Hints)
            .ToListAsync();
    }
}
