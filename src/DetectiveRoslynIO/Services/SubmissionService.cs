using DetectiveRoslynIO.Data;
using DetectiveRoslynIO.Data.Entities;
using DetectiveRoslynIO.Models;
using Microsoft.EntityFrameworkCore;

namespace DetectiveRoslynIO.Services;

public class SubmissionService : ISubmissionService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly IProgressService _progressService;

    public SubmissionService(
        IDbContextFactory<ApplicationDbContext> contextFactory,
        IProgressService progressService)
    {
        _contextFactory = contextFactory;
        _progressService = progressService;
    }

    public async Task<SubmissionResult> SubmitAnswerAsync(string userId, SubmissionRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var challenge = await context.Challenges
            .FirstOrDefaultAsync(c => c.Id == request.ChallengeId);

        if (challenge == null)
        {
            return new SubmissionResult
            {
                IsCorrect = false,
                Message = "Challenge not found.",
                AttemptNumber = 0,
                IsFirstCorrectSubmission = false
            };
        }

        var attemptCount = await GetUserAttemptCountAsync(userId, request.ChallengeId);
        var attemptNumber = attemptCount + 1;

        var isCorrect = ValidateAnswer(request.Answer, challenge);

        var submission = new UserSubmission
        {
            UserId = userId,
            ChallengeId = request.ChallengeId,
            SubmittedAnswer = request.Answer,
            IsCorrect = isCorrect,
            SubmittedAt = DateTime.UtcNow,
            AttemptNumber = attemptNumber
        };

        context.UserSubmissions.Add(submission);
        await context.SaveChangesAsync();

        var isFirstCorrect = false;
        if (isCorrect)
        {
            isFirstCorrect = await _progressService.MarkChallengeCompletedAsync(
                userId, request.ChallengeId);
        }
        else
        {
            await _progressService.UpdateProgressAttemptsAsync(userId, request.ChallengeId);
        }

        return new SubmissionResult
        {
            IsCorrect = isCorrect,
            Message = isCorrect
                ? "Correct! Well done!"
                : "Incorrect. Please try again.",
            AttemptNumber = attemptNumber,
            IsFirstCorrectSubmission = isFirstCorrect
        };
    }

    private bool ValidateAnswer(string submittedAnswer, Challenge challenge)
    {
        var submitted = submittedAnswer.Trim();
        var expected = challenge.ExpectedAnswer.Trim();

        if (!challenge.CaseSensitive)
        {
            submitted = submitted.ToLowerInvariant();
            expected = expected.ToLowerInvariant();
        }

        return challenge.AnswerFormat.ToLowerInvariant() switch
        {
            "number" => ValidateNumber(submitted, expected),
            "comma-separated" => ValidateCommaSeparated(submitted, expected),
            _ => submitted == expected
        };
    }

    private bool ValidateNumber(string submitted, string expected)
    {
        if (int.TryParse(submitted, out var submittedNum) &&
            int.TryParse(expected, out var expectedNum))
        {
            return submittedNum == expectedNum;
        }
        return false;
    }

    private bool ValidateCommaSeparated(string submitted, string expected)
    {
        var submittedItems = submitted
            .Split(',')
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .OrderBy(s => s)
            .ToList();

        var expectedItems = expected
            .Split(',')
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .OrderBy(s => s)
            .ToList();

        return submittedItems.SequenceEqual(expectedItems);
    }

    public async Task<List<UserSubmission>> GetUserSubmissionsAsync(string userId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.UserSubmissions
            .Include(s => s.Challenge)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();
    }

    public async Task<List<UserSubmission>> GetChallengeSubmissionsAsync(int challengeId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.UserSubmissions
            .Include(s => s.User)
            .Where(s => s.ChallengeId == challengeId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();
    }

    public async Task<int> GetUserAttemptCountAsync(string userId, int challengeId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.UserSubmissions
            .CountAsync(s => s.UserId == userId && s.ChallengeId == challengeId);
    }
}
