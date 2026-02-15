using DetectiveRoslynIO.Data.Entities;
using DetectiveRoslynIO.Models.Enums;

namespace DetectiveRoslynIO.Services;

public interface IChallengeService
{
    Task<List<Challenge>> GetAllChallengesAsync();
    Task<List<Challenge>> GetActiveChallengesAsync();
    Task<List<Challenge>> GetChallengesByCategoryAsync(ChallengeCategory category);
    Task<List<Challenge>> GetChallengesByDifficultyAsync(DifficultyLevel difficulty);
    Task<Challenge?> GetChallengeByIdAsync(int id);
    Task<Challenge> CreateChallengeAsync(Challenge challenge);
    Task<Challenge> UpdateChallengeAsync(Challenge challenge);
    Task DeleteChallengeAsync(int id);
    Task<List<Hint>> GetChallengeHintsAsync(int challengeId);
}
