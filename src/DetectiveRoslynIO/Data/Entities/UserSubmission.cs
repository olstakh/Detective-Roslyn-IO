namespace DetectiveRoslynIO.Data.Entities;

public class UserSubmission
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public int ChallengeId { get; set; }
    public required string SubmittedAnswer { get; set; }
    public bool IsCorrect { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public int AttemptNumber { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public Challenge Challenge { get; set; } = null!;
}
