using DetectiveRoslynIO.Data.Entities;

namespace DetectiveRoslynIO.Services;

public interface IUnlockService
{
    Task<List<Challenge>> GetUnlockedChallengesAsync(string userId, int? trackId = null);
    Task<bool> IsChallengeUnlockedAsync(string userId, int challengeId);
    Task<bool> UnlockChallengeAsync(string userId, int challengeId, bool isAutoUnlock = false);
    Task UnlockNextChallengeAsync(string userId, int completedChallengeId);
    Task UnlockFirstChallengesForNewUserAsync(string userId);
    Task<Dictionary<int, bool>> GetUnlockStatusMapAsync(string userId, int trackId);
}
