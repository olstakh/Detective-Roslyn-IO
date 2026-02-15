namespace DetectiveRoslynIO.Data.Entities;

public class Hint
{
    public int Id { get; set; }
    public int ChallengeId { get; set; }
    public required string Content { get; set; }
    public int OrderIndex { get; set; }

    public Challenge Challenge { get; set; } = null!;
}
