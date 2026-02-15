using DetectiveRoslynIO.Models.Enums;

namespace DetectiveRoslynIO.Data.Entities;

public class Challenge
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Instructions { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public ChallengeCategory Category { get; set; }
    public required string TargetRepoUrl { get; set; }
    public required string ExpectedAnswer { get; set; }
    public string AnswerFormat { get; set; } = "text"; // number, text, comma-separated
    public bool CaseSensitive { get; set; } = false;
    public string? RoslynDocsUrl { get; set; }
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? TrackId { get; set; }
    public int SequenceNumber { get; set; }

    public ChallengeTrack? Track { get; set; }
    public ICollection<Hint> Hints { get; set; } = new List<Hint>();
    public ICollection<UserSubmission> Submissions { get; set; } = new List<UserSubmission>();
    public ICollection<UserProgress> UserProgress { get; set; } = new List<UserProgress>();
}
