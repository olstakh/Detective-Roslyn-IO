using DetectiveRoslynIO.Data.Entities;

namespace DetectiveRoslynIO.Models;

public class UserProgressViewModel
{
    public int TotalChallenges { get; set; }
    public int CompletedChallenges { get; set; }
    public List<ChallengeProgress> Challenges { get; set; } = new();
}

public class ChallengeProgress
{
    public required Challenge Challenge { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int TotalAttempts { get; set; }
    public int HintsUsed { get; set; }
}
