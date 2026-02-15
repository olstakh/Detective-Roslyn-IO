using DetectiveRoslynIO.Data.Entities;
using DetectiveRoslynIO.Models;

namespace DetectiveRoslynIO.Services;

public interface IProgressService
{
    Task<UserProgressViewModel> GetUserProgressAsync(string userId);
    Task<UserProgress?> GetChallengeProgressAsync(string userId, int challengeId);
    Task<bool> MarkChallengeCompletedAsync(string userId, int challengeId);
    Task UpdateProgressAttemptsAsync(string userId, int challengeId);
    Task IncrementHintUsageAsync(string userId, int challengeId);
}
