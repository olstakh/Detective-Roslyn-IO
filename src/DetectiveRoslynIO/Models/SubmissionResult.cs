namespace DetectiveRoslynIO.Models;

public class SubmissionResult
{
    public bool IsCorrect { get; set; }
    public required string Message { get; set; }
    public int AttemptNumber { get; set; }
    public bool IsFirstCorrectSubmission { get; set; }
}
