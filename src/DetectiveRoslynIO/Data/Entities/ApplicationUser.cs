using Microsoft.AspNetCore.Identity;

namespace DetectiveRoslynIO.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }

    public ICollection<UserSubmission> Submissions { get; set; } = new List<UserSubmission>();
    public ICollection<UserProgress> Progress { get; set; } = new List<UserProgress>();
}
