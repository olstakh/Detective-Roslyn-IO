using DetectiveRoslynIO.Data.Entities;
using DetectiveRoslynIO.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DetectiveRoslynIO.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/login", LoginAsync)
            .DisableAntiforgery();
        app.MapPost("/api/account/register", RegisterAsync)
            .DisableAntiforgery();
        app.MapPost("/api/account/logout", LogoutAsync)
            .DisableAntiforgery();
    }

    private static async Task<IResult> LoginAsync(
        [FromForm] string email,
        [FromForm] string password,
        [FromForm] string? rememberMe,
        [FromForm] string? returnUrl,
        SignInManager<ApplicationUser> signInManager)
    {
        var isPersistent = rememberMe?.ToLower() == "true";

        var result = await signInManager.PasswordSignInAsync(
            email,
            password,
            isPersistent,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var redirectUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            return Results.Redirect(redirectUrl);
        }

        return Results.Redirect($"/Account/Login?error=Invalid login attempt&email={Uri.EscapeDataString(email)}");
    }

    private static async Task<IResult> RegisterAsync(
        [FromForm] string displayName,
        [FromForm] string email,
        [FromForm] string password,
        [FromForm] string confirmPassword,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUnlockService unlockService)
    {
        if (password != confirmPassword)
        {
            return Results.Redirect($"/Account/Register?error=Passwords do not match");
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = displayName
        };

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "User");

            // Unlock first challenges in each track for new user
            await unlockService.UnlockFirstChallengesForNewUserAsync(user.Id);

            await signInManager.SignInAsync(user, isPersistent: false);
            return Results.Redirect("/");
        }

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return Results.Redirect($"/Account/Register?error={Uri.EscapeDataString(errors)}");
    }

    private static async Task<IResult> LogoutAsync(
        SignInManager<ApplicationUser> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Redirect("/");
    }
}
