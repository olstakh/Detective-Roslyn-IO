using DetectiveRoslynIO.Data.Entities;
using DetectiveRoslynIO.Models;

namespace DetectiveRoslynIO.Services;

public interface ISubmissionService
{
    Task<SubmissionResult> SubmitAnswerAsync(string userId, SubmissionRequest request);
    Task<List<UserSubmission>> GetUserSubmissionsAsync(string userId);
    Task<List<UserSubmission>> GetChallengeSubmissionsAsync(int challengeId);
    Task<int> GetUserAttemptCountAsync(string userId, int challengeId);
}
