namespace DetectiveRoslynIO.Data.Entities;

public class UserChallengeUnlock
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public int ChallengeId { get; set; }
    public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;
    public bool IsAutoUnlocked { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public Challenge Challenge { get; set; } = null!;
}
