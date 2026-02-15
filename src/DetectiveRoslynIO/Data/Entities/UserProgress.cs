namespace DetectiveRoslynIO.Data.Entities;

public class UserProgress
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public int ChallengeId { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public int TotalAttempts { get; set; }
    public int HintsUsed { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public Challenge Challenge { get; set; } = null!;
}
