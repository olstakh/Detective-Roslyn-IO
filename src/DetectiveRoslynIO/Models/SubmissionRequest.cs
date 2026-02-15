namespace DetectiveRoslynIO.Models;

public class SubmissionRequest
{
    public int ChallengeId { get; set; }
    public required string Answer { get; set; }
}
