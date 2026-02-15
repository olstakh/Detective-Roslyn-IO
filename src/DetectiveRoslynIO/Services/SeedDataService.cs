using DetectiveRoslynIO.Data;
using DetectiveRoslynIO.Data.Entities;
using DetectiveRoslynIO.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DetectiveRoslynIO.Services;

public class SeedDataService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedDataService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
        await SeedChallengesAsync();
    }

    private async Task SeedRolesAsync()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole("User"));
        }
    }

    private async Task SeedAdminUserAsync()
    {
        const string adminEmail = "admin@detectiveroslyn.io";
        const string adminPassword = "Admin123!";

        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                DisplayName = "Administrator",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

    private async Task SeedChallengesAsync()
    {
        if (await _context.Challenges.AnyAsync())
        {
            return; // Already seeded
        }

        var challenges = new[]
        {
            new Challenge
            {
                Title = "Find Unused Private Fields",
                Description = "Create a Roslyn analyzer that identifies private fields that are declared but never read in the code.",
                Instructions = @"1. Clone the target repository
2. Create a DiagnosticAnalyzer class
3. Register a syntax node action for SyntaxKind.FieldDeclaration
4. Check if the field is private and never referenced
5. Count the total number of violations found
6. Submit the count as your answer",
                Difficulty = DifficultyLevel.Beginner,
                Category = ChallengeCategory.Analyzer,
                TargetRepoUrl = "https://github.com/roslyn-detectives/sample-codebase-01",
                ExpectedAnswer = "7",
                AnswerFormat = "number",
                CaseSensitive = false,
                RoslynDocsUrl = "https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/",
                OrderIndex = 1,
                IsActive = true
            },
            new Challenge
            {
                Title = "Detect Empty Catch Blocks",
                Description = "Build an analyzer that finds catch blocks with no statements inside them, which is generally considered a code smell.",
                Instructions = @"1. Clone the target repository
2. Create a DiagnosticAnalyzer
3. Register for SyntaxKind.CatchClause
4. Check if the catch block body is empty
5. Count the violations
6. Submit your answer",
                Difficulty = DifficultyLevel.Beginner,
                Category = ChallengeCategory.Analyzer,
                TargetRepoUrl = "https://github.com/roslyn-detectives/sample-codebase-01",
                ExpectedAnswer = "3",
                AnswerFormat = "number",
                CaseSensitive = false,
                RoslynDocsUrl = "https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis",
                OrderIndex = 2,
                IsActive = true
            },
            new Challenge
            {
                Title = "Fix Naming Convention Violations",
                Description = "Create a code fix that renames private fields to use camelCase with underscore prefix (_fieldName).",
                Instructions = @"1. Clone the target repository
2. Create a CodeFixProvider
3. Register it for the naming convention diagnostic
4. Implement the fix to rename fields with proper casing
5. Count how many fields need renaming
6. Submit the count",
                Difficulty = DifficultyLevel.Intermediate,
                Category = ChallengeCategory.CodeFix,
                TargetRepoUrl = "https://github.com/roslyn-detectives/sample-codebase-02",
                ExpectedAnswer = "12",
                AnswerFormat = "number",
                CaseSensitive = false,
                RoslynDocsUrl = "https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/tutorials/how-to-write-csharp-analyzer-code-fix",
                OrderIndex = 3,
                IsActive = true
            }
        };

        _context.Challenges.AddRange(challenges);
        await _context.SaveChangesAsync();

        // Add hints for the first challenge
        var firstChallenge = challenges[0];
        var hints = new[]
        {
            new Hint
            {
                ChallengeId = firstChallenge.Id,
                Content = "Look at the IFieldSymbol interface to analyze field declarations.",
                OrderIndex = 1
            },
            new Hint
            {
                ChallengeId = firstChallenge.Id,
                Content = "Use DataFlowAnalysis to detect if a field is read anywhere in the code.",
                OrderIndex = 2
            },
            new Hint
            {
                ChallengeId = firstChallenge.Id,
                Content = "Remember to exclude fields that are only used in their own initializers.",
                OrderIndex = 3
            }
        };

        _context.Hints.AddRange(hints);
        await _context.SaveChangesAsync();
    }
}
