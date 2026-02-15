namespace DetectiveRoslynIO.Data.Entities;

public class ChallengeTrack
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? IconClass { get; set; }
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Challenge> Challenges { get; set; } = new List<Challenge>();
}
